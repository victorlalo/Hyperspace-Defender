using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool _instance;
    public static ObjectPool Instance
    {
        get { return _instance; }
    }

    //List<GameObject>[] objectPools;
    List<GameObject> bulletPool;
    [SerializeField] GameObject bulletPrefab;

    List<GameObject> enemyBulletPool;
    [SerializeField] GameObject enemyBulletPrefab;

    List<GameObject> bombPool;
    [SerializeField] GameObject bombPrefab;

    List<GameObject>[] enemyPool;
    [SerializeField] GameObject[] enemyPrefabs;

    List<GameObject> smallExplosionPool;
    [SerializeField] GameObject smallExplosionPrefab;

    List<GameObject> largeExplosionPool;
    [SerializeField] GameObject largeExplosionPrefab;

    List<GameObject> playerExplosionPool;
    [SerializeField] GameObject playerExplosionPrefab;

    int[] enemyActiveCounts;

    private void Awake()
    {
        // Singleton implementation (only 1 of this script)
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void Initialize()
    {
        bulletPool = new List<GameObject>(100);
        for (int i = 0; i < bulletPool.Capacity; i++)
        {
            GameObject b = Instantiate(bulletPrefab);
            //GameObject.DontDestroyOnLoad(b);
            b.SetActive(false);
            bulletPool.Add(b);
        }

        enemyBulletPool = new List<GameObject>(50);
        for (int i = 0; i < enemyBulletPool.Capacity; i++)
        {
            GameObject b = Instantiate(enemyBulletPrefab);
            //GameObject.DontDestroyOnLoad(b);
            b.SetActive(false);
            enemyBulletPool.Add(b);
        }

        bombPool = new List<GameObject>(5);
        for (int i = 0; i < bombPool.Capacity; i++)
        {
            GameObject bomb = Instantiate(bombPrefab);
            //GameObject.DontDestroyOnLoad(bomb);
            bomb.SetActive(false);
            bombPool.Add(bomb);
        }


        enemyPool = new List<GameObject>[enemyPrefabs.Length];
        for (int i = 0; i < enemyPool.Length; i++)
        {
            enemyPool[i] = new List<GameObject>(35);
            for (int j = 0; j < enemyPool[i].Capacity; j++)
            {
                GameObject e = Instantiate(enemyPrefabs[i]);
                //GameObject.DontDestroyOnLoad(e);
                e.SetActive(false);
                enemyPool[i].Add(e);
            }
        }

        enemyActiveCounts = new int[enemyPrefabs.Length];
        for (int i = 0; i < enemyActiveCounts.Length; i++)
        {
            enemyActiveCounts[i] = 0;
        }

        smallExplosionPool = new List<GameObject>(70);
        for (int i =0; i < smallExplosionPool.Capacity; i++)
        {
            GameObject exp = Instantiate(smallExplosionPrefab);
            exp.GetComponent<ParticleSystem>().Pause();
            exp.SetActive(false);
            smallExplosionPool.Add(exp);
        }
        
        largeExplosionPool = new List<GameObject>(10);
        for (int i =0; i < largeExplosionPool.Capacity; i++)
        {
            GameObject exp = Instantiate(largeExplosionPrefab);
            exp.GetComponent<ParticleSystem>().Pause();
            exp.SetActive(false);
            largeExplosionPool.Add(exp);
        }

        playerExplosionPool = new List<GameObject>(10);
        for (int i = 0; i <playerExplosionPool.Capacity; i++)
        {
            GameObject exp = Instantiate(playerExplosionPrefab);
            exp.GetComponent<ParticleSystem>().Pause();
            exp.SetActive(false);
            playerExplosionPool.Add(exp);
        }
        



    }

    public GameObject GetBullet(Vector3 pos, Quaternion quat)
    {
        GameObject b;

        if (bulletPool.Count > 0)
        {
            b = bulletPool[bulletPool.Count - 1];
            bulletPool.RemoveAt(bulletPool.Count - 1);

            b.SetActive(true);
            b.transform.position = pos;
            b.transform.rotation = quat;
        }

        else
        {
            bulletPool.Capacity++;
            b = Instantiate(bulletPrefab, pos, quat);
            
        }

        b.GetComponent<Bullet>().StartMoving();
        return b;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bullet.GetComponent<Bullet>().StopMoving();
        bulletPool.Add(bullet);
    }

    public GameObject GetEnemyBullet(Vector3 pos, Quaternion quat)
    {
        GameObject b;

        if (enemyBulletPool.Count > 0)
        {
            b = enemyBulletPool[enemyBulletPool.Count - 1];
            enemyBulletPool.RemoveAt(enemyBulletPool.Count - 1);

            b.SetActive(true);
            b.transform.position = pos;
            b.transform.rotation = quat;
        }

        else
        {
            enemyBulletPool.Capacity++;
            b = Instantiate(enemyBulletPrefab, pos, quat);

        }

        b.GetComponent<Bullet>().StartMoving();
        return b;
    }

    public void ReturnEnemyBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bullet.GetComponent<Bullet>().StopMoving();
        enemyBulletPool.Add(bullet);
    }

    public GameObject GetEnemy(EnemyID enemyID)
    {
        GameObject e;
        int eID = (int)enemyID;
        List<GameObject> enemySubPool = enemyPool[eID];

        if (enemySubPool.Count > 0)
        {
            e = enemySubPool[enemySubPool.Count - 1];
            enemySubPool.RemoveAt(enemySubPool.Count - 1);
        }

        else
        {
            enemySubPool.Capacity++;
            e = Instantiate(enemyPrefabs[eID]);
            e.SetActive(false);
        }

        enemyActiveCounts[eID]++;
        e.GetComponent<Enemy>().Activate();
        return e;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.GetComponent<Enemy>().Deactivate();
        int eID = (int)enemy.GetComponent<Enemy>().id;
        enemyPool[eID].Add(enemy);
        enemyActiveCounts[eID]--;
    }

    public int GetActiveEnemyCount()
    {
        return enemyActiveCounts.Sum();
    }

    public GameObject GetBomb(Vector3 pos, Quaternion quat)
    {
        GameObject bomb = bombPool[bombPool.Count - 1];
        bombPool.RemoveAt(bombPool.Count - 1);

        bomb.transform.position = pos;
        bomb.transform.rotation = quat;
        bomb.SetActive(true);

        bomb.GetComponent<Bomb>().StartMoving();

        return bomb;
    }

    public void ReturnBomb(GameObject bomb)
    {
        bomb.SetActive(false);
        bombPool.Add(bomb);
    }


    public GameObject GetSmallExplosion(Vector3 pos)
    {
        GameObject exp = smallExplosionPool[smallExplosionPool.Count - 1];
        smallExplosionPool.RemoveAt(smallExplosionPool.Count - 1);

        exp.transform.position = pos;
        exp.SetActive(true);
        exp.GetComponent<ParticleSystem>().Play();

        return exp;
    }

    public void ReturnSmallExplosion(GameObject exp)
    {
        ParticleSystem ps = exp.GetComponent<ParticleSystem>();
        ps.Pause();
        ps.time = 0.0f;
        
        exp.SetActive(false);
        smallExplosionPool.Add(exp);
    }

    public GameObject GetLargeExplosion(Vector3 pos)
    {
        GameObject exp = largeExplosionPool[largeExplosionPool.Count - 1];
        largeExplosionPool.RemoveAt(largeExplosionPool.Count - 1);

        exp.transform.position = pos;
        exp.SetActive(true);
        exp.GetComponent<ParticleSystem>().Play();

        return exp;
    }

    public void ReturnLargeExplosion(GameObject exp)
    {
        ParticleSystem ps = exp.GetComponent<ParticleSystem>();
        ps.Pause();
        ps.time = 0.0f;
        

        exp.SetActive(false);
        largeExplosionPool.Add(exp);
    }

    public GameObject GetPlayerExplosion(Vector3 pos)
    {
        GameObject exp = playerExplosionPool[playerExplosionPool.Count - 1];
        playerExplosionPool.RemoveAt(playerExplosionPool.Count - 1);

        exp.transform.position = pos;
        exp.SetActive(true);
        exp.GetComponent<ParticleSystem>().Play();

        return exp;
    }

    public void ReturnPlayerExplosion(GameObject exp)
    {
        ParticleSystem ps = exp.GetComponent<ParticleSystem>();
        ps.Pause();
        ps.time = 0.0f;
        
        exp.SetActive(false);
        playerExplosionPool.Add(exp);
    }
}
