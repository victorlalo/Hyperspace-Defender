using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] GameObject Shield;
    [SerializeField] GameObject SpreadShot;
    [SerializeField] GameObject LifeUp;
    [SerializeField] GameObject Bomb;
    [SerializeField] GameObject RegularShot;

    private static PickUpSpawner _instance;
    public static PickUpSpawner Instance
    {
        get { return _instance; }
    }

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

    public GameObject ShieldPickup
    {
        get { return Shield; }
    }
    public GameObject SpreadShotPickup
    {
        get { return SpreadShot; }
    }
    public GameObject LifeUpPickup
    {
        get { return LifeUp; }
    }
    public GameObject BombPickup
    {
        get { return Bomb; }
    }
    public GameObject RegularShotPickup
    {
        get { return RegularShot; }
    }

}
