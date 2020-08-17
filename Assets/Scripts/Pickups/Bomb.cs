using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Bullet
{
    [SerializeField] float explosionRadius = 11f;
    [SerializeField] GameObject largeExplosionPrefab;

    protected override void Update()
    {
        float moveAmt = moveSpeed * Time.deltaTime;
        transform.Translate(moveAmt, 0, 0);

        transform.Rotate(200 * Time.deltaTime, 0f, 0f);
    }

    public void Explode()
    {
        // trigger explosion FX
        ObjectPool.Instance.GetLargeExplosion(transform.position);
        //Instantiate(largeExplosionPrefab, transform.position, Quaternion.identity);
        Camera.main.GetComponent<ScreenShake>().ShakeScreen(.7f, .52f);
        Camera.main.GetComponent<AudioManager>().PlaySound(Sounds.BOMB_EXPLODE);

        // blow up anything within given radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in colliders)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                GameObject.FindGameObjectWithTag("HUD").GetComponent<GameManager>().AddScore(col.GetComponent<Enemy>().DeathPoints);
                ObjectPool.Instance.ReturnEnemy(col.gameObject);
            }
        }

        // destroy itself
        ObjectPool.Instance.ReturnBomb(gameObject);
    }

    protected override void OnBecameInvisible()
    {
        ObjectPool.Instance.ReturnBomb(gameObject);
    }
}
