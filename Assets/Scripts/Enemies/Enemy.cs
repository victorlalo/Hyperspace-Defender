using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MoveStyle
{
    sin,
    circular,
    sawWave,
    random,
    kamikaze
}

public class Enemy : MonoBehaviour
{
    [SerializeField] float deathPoints = 10f;
    public float DeathPoints
    {
        get { return deathPoints; }
    }

    [SerializeField] protected float enemySpeed = 0f;
    [SerializeField] float bulletSpeed = 25f;
    [SerializeField] int spawnDropProbability = 5;

    [SerializeField] GameObject explosionFX;

    protected float activeMoveSpeed = 0f;
    const float startXPos = 5f;
    const float startXWiggleRoom = 5f;

    protected float shootTimerAmt = 3f;
    protected float currShootTime = 0f;

    protected bool canShoot = false;

    bool flashing = false;

    [SerializeField] protected float health = 10f;

    public EnemyID id;
    //[SerializeField] MoveStyle moveStyle;

    public CameraBounds camBounds;

    protected virtual void Start()
    {
        camBounds = Camera.main.GetComponent<GameUtils>().CameraBounds;

        transform.position = new Vector3(camBounds.topRightCorner.x + startXPos,
                                         camBounds.bottomLeftCorner.y + Random.Range(camBounds.centerOfScreen.y / 6, 5 * camBounds.centerOfScreen.y / 6), 0f);
    }

    public void Activate()
    {
        activeMoveSpeed = enemySpeed;
        flashing = false;
        transform.position = new Vector3(camBounds.topRightCorner.x + startXPos,
                                         camBounds.bottomLeftCorner.y + Random.Range(camBounds.centerOfScreen.y / 6, 5 * camBounds.centerOfScreen.y / 6), 0f);
        gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        
        activeMoveSpeed = 0;
        if (id == EnemyID.Blue || id == EnemyID.Green)
        {
            health = 10f;
        }
        else if (id == EnemyID.Red)
        {
            health = 30f;
        }
        else if (id == EnemyID.UFO)
        {
            health = 50f;
        }
        transform.position = new Vector3(camBounds.topRightCorner.x + startXPos,
                                         camBounds.bottomLeftCorner.y, 0f);
        gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (transform.position.x < camBounds.bottomLeftCorner.x - 5f)
        {
            ObjectPool.Instance.ReturnEnemy(gameObject);
        }

        canShoot = (transform.position.x < camBounds.topRightCorner.x);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (transform.position.x > camBounds.topRightCorner.x)
            return;

        bool collided = false;
        GameObject col = collision.gameObject;


        if (col.CompareTag("Bullet"))
        {
            collided = true;
            health -= 10f;
            //Debug.Log(collision.gameObject.tag);
            ObjectPool.Instance.ReturnBullet(col);
        }

        else if (col.CompareTag("Bomb"))
        {
            collided = true;
            health -= 50f;
            col.GetComponent<Bomb>().Explode();
            ObjectPool.Instance.ReturnBomb(col);
        }

        if (collided)
        {
            if (health > 0f)
            {
                Camera.main.GetComponent<AudioManager>().PlaySound(Sounds.ENEMY_WEAK);
                if ((id == EnemyID.Red || id == EnemyID.UFO) && !flashing) { }
                    StartCoroutine(EnemyHitAnim());
            }

            else
            {
                if (id == EnemyID.Blue || id == EnemyID.Green)
                {
                    Camera.main.GetComponent<AudioManager>().PlaySound(Sounds.ENEMY_WEAK);
                }
                else
                {
                    Camera.main.GetComponent<AudioManager>().PlaySound(Sounds.ENEMY_STRONG);
                }

                float rng = Random.Range(0, 100);

                float levelDropMultiplier = GameObject.FindGameObjectWithTag("HUD").GetComponent<GameManager>().SpawnDropMultiplier;

                if (rng * levelDropMultiplier < spawnDropProbability * 0.56f)
                {
                    DropPowerUp();
                }

                GameObject.FindGameObjectWithTag("HUD").GetComponent<GameManager>().AddScore(deathPoints);
                ObjectPool.Instance.GetSmallExplosion(transform.position);
                //Instantiate(explosionFX, transform.position, Quaternion.identity);
                ObjectPool.Instance.ReturnEnemy(gameObject);
            }

            
        }
    }

    void DropPowerUp()
    {
        float rng = Random.Range(0, 100);
        if (rng < 33)
        {
            Instantiate(PickUpSpawner.Instance.ShieldPickup, transform.position, Quaternion.identity);
        }
        else if (rng < 68)
        {
            Instantiate(PickUpSpawner.Instance.SpreadShotPickup, transform.position, Quaternion.identity);
        }
        else if (rng < 94)
        {
            Instantiate(PickUpSpawner.Instance.BombPickup, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(PickUpSpawner.Instance.LifeUpPickup, transform.position, Quaternion.identity);
        }
    }

    protected void CheckShoot(float minInterval, float maxInterval)
    {
        if (currShootTime > shootTimerAmt && canShoot)
        {
            currShootTime = 0f;
            shootTimerAmt = Random.Range(minInterval, maxInterval);
            GameObject b = ObjectPool.Instance.GetEnemyBullet(transform.position, Quaternion.Euler(0, 0, -90));
            b.GetComponent<Bullet>().BulletSpeed = bulletSpeed;
        }
        else
        {
            currShootTime += Time.deltaTime;
        }
    }

    IEnumerator EnemyHitAnim()
    {
        if (!isActiveAndEnabled)
        {
            yield break;
        }

        int count = 0;
        bool toggle = false;
        flashing = true;
        while (count < 6)
        {
            yield return new WaitForSeconds(0.15f);
            GetComponentInChildren<MeshRenderer>().enabled = toggle;
            toggle = !toggle;
            count++;
        }
    }

}
