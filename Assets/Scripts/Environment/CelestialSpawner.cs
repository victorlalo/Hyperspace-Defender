using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialSpawner : MonoBehaviour
{
    static CelestialSpawner _instance;
    public static CelestialSpawner Instance
    {
        get { return _instance; }
    }

    [SerializeField] GameObject bluePlanet;
    [SerializeField] GameObject redPlanet;
    [SerializeField] GameObject greenPlanet;

    //[SerializeField] float velocity = 0.5f;
    [SerializeField] float spawnPosRange = 10f;

    int maxPlanetsOnScreen = 3;
    int planetCount = 0;
    Vector3 spawnPosition;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        for(int i = 0; i < maxPlanetsOnScreen; i++)
        {

            SpawnNewPlanet();
        }
    }


    public void SpawnNewPlanet()
    {
        int randomInt = Random.Range(0, 100);
        spawnPosition = new Vector3(transform.position.x + Random.Range(-spawnPosRange, spawnPosRange),
                                    transform.position.y + Random.Range(-spawnPosRange, spawnPosRange),
                                    transform.position.z + +Random.Range(-spawnPosRange, spawnPosRange));

        GameObject planet;

        if (randomInt < 33)
        {
            planet = Instantiate(bluePlanet, spawnPosition, Quaternion.identity);
        }

        else if (randomInt < 67)
        {
            planet = Instantiate(redPlanet, spawnPosition, Quaternion.identity);
        }

        else
        {
            planet = Instantiate(greenPlanet, spawnPosition, Quaternion.identity);
        }

        planet.GetComponent<Planet>().Speed = Random.Range(6f, 9f);

    }
}
