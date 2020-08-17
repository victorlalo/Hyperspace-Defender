using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGreen : Enemy
{
    GameObject targetPlayer;

    protected override void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        base.Start();
    }
    void FixedUpdate()
    {
        // home in on player at fast speed (kamikaze)
        Vector2 moveAmt = Vector2.MoveTowards(transform.position, targetPlayer.transform.position, activeMoveSpeed);
        transform.position = moveAmt;
    }
}
