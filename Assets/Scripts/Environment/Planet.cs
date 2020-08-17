using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    float speed = 10f;
    float screenLeftEdge;

    public float Speed
    {
        set { speed = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        screenLeftEdge = Camera.main.GetComponent<GameUtils>().BottomLeftCorner.x;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfOffscreen();
        transform.Translate(-speed * Time.deltaTime, 0, 0);
    }

    void CheckIfOffscreen()
    {
        if (transform.position.x + 15f < screenLeftEdge)
        {
            CelestialSpawner.Instance.SpawnNewPlanet();
            Destroy(gameObject);
        }
    }
}
