using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum EnemyID
{
    Blue = 0,
    Red = 1,
    Green = 2,
    UFO = 3
}
public class EnemySpawner : MonoBehaviour
{
    const int maxActiveEnemies = 50;
    Dictionary<EnemyID, int> spawnProbabilityDict = new Dictionary<EnemyID, int>()
    {
        { EnemyID.Blue, 40 },
        { EnemyID.Red, 90 },
        { EnemyID.Green, 70 },
        { EnemyID.UFO, 100 }
    };

    [SerializeField] float spawnTimeAmount = .25f; // seconds between spawns
    float timeElapsed = 0f;

    bool canSpawn = false;
    public bool CanSpawn 
    {
        get { return canSpawn; }
        set { canSpawn = value; }
    }

    public float SpawnTimeAmount
    {
        get
        { return spawnTimeAmount; }
        set
        { spawnTimeAmount = value; }
    }

    void Start()
    {
        //SpawnNewEnemy();
    }

    void Update()
    {
        if (!canSpawn)
        {
            return;
        }

        if (timeElapsed > spawnTimeAmount)
        {
            if (ObjectPool.Instance.GetActiveEnemyCount() <= maxActiveEnemies)
            {
                timeElapsed = 0f;
                SpawnNewEnemy();
            }
        }
        else
        {
            timeElapsed += Time.deltaTime;
        }
    }

    void SpawnNewEnemy()
    {
        EnemyID spawnID = EnemyID.Blue; // default
        int rng = Random.Range(0, 100);

        if (rng < spawnProbabilityDict[EnemyID.Blue])
        {
            spawnID = EnemyID.Blue;
        }
        else if (rng < spawnProbabilityDict[EnemyID.Green])
        {
            spawnID = EnemyID.Red;
        }
        else if (rng < spawnProbabilityDict[EnemyID.Red])
        {
            spawnID = EnemyID.Green;
        }
        else
        {
            spawnID = EnemyID.UFO;
        }

        ObjectPool.Instance.GetEnemy(spawnID);
    }
}
