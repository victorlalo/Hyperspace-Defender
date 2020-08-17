using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Regular,
    Spread,
    Laser,
    Bomb
};

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider collidr;

    [SerializeField] GameObject gunTipPos;
    [SerializeField] GameObject shipModel;
    float shipInitialXRot;

    int lives = 3;

    [SerializeField] float vertSpeed = 5f;
    [SerializeField] float horizSpeed = 5f;

    [SerializeField] GameObject normalBullet;
    [SerializeField] float bulletCooldownTime = 0.4f;
    bool canShoot = false;
    float bulletCooldownTimer = 0f;

    [SerializeField] GameObject shield;
    bool isShielded = false;

    [SerializeField] float spreadShotTime = 5f;
    float currSpreadShotTime = 0f;
    bool hasSpreadShot = false;

    [SerializeField] GameObject bombPrefab;
    [SerializeField] float bombCooldownTime = 1f;
    //int numBombs = 0;
    bool canShootBomb = false;
    float bombCooldownTimer = 0;

    public WeaponType currWeapon = WeaponType.Regular;

    [SerializeField] GameObject ExplosionPrefab;

    Vector2 topRightWorldCorner;
    Vector2 bottomLeftWorldCorner;

    Vector3 offScreenPosition;

    float colliderHalfWidth;
    float colliderHalfHeight;

    GameObject[] fireParticles;
    GameManager gameManager;
    AudioManager audioManager;

    bool canMove = false;
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collidr = GetComponent<BoxCollider>();

        colliderHalfWidth = collidr.size.x / 2f;
        colliderHalfHeight = collidr.size.y / 2f;

        bottomLeftWorldCorner = Camera.main.GetComponent<GameUtils>().BottomLeftCorner;
        topRightWorldCorner = Camera.main.GetComponent<GameUtils>().TopRightCorner;

        fireParticles = GameObject.FindGameObjectsWithTag("FireParticles");

        shield.SetActive(false);

        offScreenPosition = new Vector3(bottomLeftWorldCorner.x - 10f, 0, 0);

        gameManager = GameObject.FindGameObjectWithTag("HUD").GetComponent<GameManager>();
        gameManager.gameObject.SetActive(false);

        audioManager = Camera.main.GetComponent<AudioManager>();

        shipInitialXRot = shipModel.transform.localEulerAngles.x;

    }

    public void EnterScene()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //only execture if not on Start or End Screen
        if (!canMove)
        {
            return;
        }
        
        CheckInput();
        CheckFire();
        ClampToScreenEdges();

        if (!canShoot)
        {
            UpdateBulletCooldown();
        }

        if (!canShootBomb)
        {
            UpdateBombCooldown();
        }

        if (hasSpreadShot)
        {
            UpdateSpreadShootTimer();
        }
    }

    void CheckInput()
    {
        float vertMove = Input.GetAxis("Vertical");
        float horizMove = Input.GetAxis("Horizontal");
        Vector3 movePos = new Vector3(horizMove * horizSpeed * Time.deltaTime, vertMove * vertSpeed * Time.deltaTime, 0f);

        //shipModel.transform.rotation = Quaternion.RotateTowards(shipModel.transform.rotation, Quaternion.Euler(shipModel.transform.rotation.x + (vertMove * 5f *Time.deltaTime), 0f, 0f), .1f);
        if (Mathf.Abs(vertMove) > 0f)
        {
            Vector3 euler = shipModel.transform.localEulerAngles;
            euler.x = Mathf.Clamp(euler.x + vertMove * 0.45f, shipInitialXRot - 12f, shipInitialXRot + 12f);
            shipModel.transform.localEulerAngles = euler;
        }
        else
        {
            shipModel.transform.localRotation = Quaternion.RotateTowards(shipModel.transform.localRotation, Quaternion.Euler(shipInitialXRot, 0, 0), .35f);
        }



        transform.position = Vector3.MoveTowards(transform.position, transform.position + movePos, 1f);


        if (Mathf.Abs(vertMove + horizMove) > 0)
        {
            for (int i = 0; i < fireParticles.Length; i++)
            {
                fireParticles[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < fireParticles.Length; i++)
            {
                fireParticles[i].SetActive(false);
            }

        }
    }

    void CheckFire()
    {
        // normal fire and special fire
        if (Input.GetButton("Jump") && canShoot)
        {
            if (currWeapon == WeaponType.Regular)
            {
                ObjectPool.Instance.GetBullet(gunTipPos.transform.position, Quaternion.Euler(0, 0, 90));
            }

            // spread shot has 3 bullets coming out at different angles
            else if (currWeapon == WeaponType.Spread)
            {
                ObjectPool.Instance.GetBullet(gunTipPos.transform.position, Quaternion.Euler(0, 0, 83));
                ObjectPool.Instance.GetBullet(gunTipPos.transform.position, Quaternion.Euler(0, 0, 90));
                ObjectPool.Instance.GetBullet(gunTipPos.transform.position, Quaternion.Euler(0, 0, 98));
            }

            audioManager.PlaySound(Sounds.GUNSHOT);
            canShoot = false;
        }

        // SPECIAL SHOT (BOMB)
        if (Input.GetKeyDown(KeyCode.X) && canShootBomb && gameManager.Bombs > 0)
        {
            
            Instantiate(bombPrefab, gunTipPos.transform.position, Quaternion.identity);
            //numBombs--;
            gameManager.LoseBomb();
            canShootBomb = false;
            audioManager.PlaySound(Sounds.BOMB_SHOOT);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject col = collision.gameObject;
        bool collided = false;

        if (col.CompareTag("EnemyBullet"))
        {
            ObjectPool.Instance.ReturnEnemyBullet(col);
            collided = true;
        }
        else if (col.CompareTag("Enemy"))
        {
            ObjectPool.Instance.ReturnEnemy(col);
            collided = true;
        }

        if (collided)
        { 
            // Explosion FX

            if (isShielded)
            {
                isShielded = false;
                shield.SetActive(false);
            }

            else
            {
                ObjectPool.Instance.GetPlayerExplosion(transform.position);
                //Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
                Camera.main.GetComponent<ScreenShake>().ShakeScreen(0.37f, .35f);               
                gameManager.LoseLife();
                //lives--;

                transform.position = offScreenPosition;
                //if (lives < 0)
                //{
                //    canMove = false;
                //}


                audioManager.PlaySound(Sounds.LOSE_LIFE);
                //EnterScene();
            }
        }

        if (col.CompareTag("Pickup"))
        {
            ProcessPickup(col.gameObject.GetComponent<Pickup>().PickupID);
            audioManager.PlaySound(Sounds.PICKUP);
            Destroy(col.gameObject);
        }
    }

    void ProcessPickup(PickUps id)
    {
        switch (id)
        {
            case PickUps.Shield:
                AddShield();
                break;

            case PickUps.Bomb:
                //numBombs = Mathf.Clamp(numBombs + 1, 0, 5);
                gameManager.AddBomb();
                break;

            case PickUps.LifeUp:
                lives = Mathf.Clamp(lives++, 0, 5);
                gameManager.AddLife();
                break;

            case PickUps.SpreadShot:
                hasSpreadShot = true;
                currSpreadShotTime = 0f;
                currWeapon = WeaponType.Spread;
                break;

            case PickUps.RegularShot:
                hasSpreadShot = false;
                currSpreadShotTime = 0f;
                currWeapon = WeaponType.Regular;
                break;
        }
    }

    void ClampToScreenEdges()
    {
        float xPos = Mathf.Clamp(transform.position.x, bottomLeftWorldCorner.x + colliderHalfWidth, bottomLeftWorldCorner.x + 30f);
        float yPos = Mathf.Clamp(transform.position.y, bottomLeftWorldCorner.y + colliderHalfHeight + 5f, topRightWorldCorner.y - colliderHalfHeight);

        transform.position = new Vector2(xPos, yPos);

    }

    void AddShield()
    {
        if (isShielded)
        {
            return;
        }

        isShielded = true;
        shield.SetActive(true);
        
    }

    void UpdateSpreadShootTimer()
    {
        if (currSpreadShotTime < spreadShotTime)
        {
            currSpreadShotTime += Time.deltaTime;
        }
        else
        {
            currSpreadShotTime = 0f;
            hasSpreadShot = false;
            currWeapon = WeaponType.Regular;
        }
    }

    void UpdateBulletCooldown()
    {
        if (bulletCooldownTimer < bulletCooldownTime)
        {
            bulletCooldownTimer += Time.deltaTime;
        }

        else
        {
            canShoot = true;
            bulletCooldownTimer = 0f;
        }

    }

    void UpdateBombCooldown()
    {
        if (bombCooldownTimer < bombCooldownTime)
        {
            bombCooldownTimer += Time.deltaTime;
        }
        else
        {
            canShootBomb = true;
            bombCooldownTimer = 0f;
        }
    }
}
