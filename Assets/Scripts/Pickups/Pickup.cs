using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickUps
{
    SpreadShot,
    LifeUp,
    Bomb,
    Shield,
    RegularShot
}
public class Pickup : MonoBehaviour
{
    [SerializeField] PickUps pickupID;
    public PickUps PickupID
    {
        get { return pickupID; }
    }

    float moveSpeed = 17f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
    }
}
