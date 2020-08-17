using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRed : Enemy
{
    float triangleInverter = 1f;
    float initYPos;
    [SerializeField] float movementRange = 30f;

    protected override void Start()
    {
        base.Start();
        //activeMoveSpeed = enemySpeed;
        initYPos = transform.position.y;
    }
    void FixedUpdate()
    {
        //Debug.Log(transform.position.y - initYPos);
        if (Mathf.Abs(transform.position.y - initYPos) > movementRange || transform.position.y > camBounds.topRightCorner.y || transform.position.y < camBounds.bottomLeftCorner.y)
        {
            triangleInverter *= -1f;
        }

        if (transform.position.x > camBounds.topRightCorner.x)
        {
            transform.Translate(-1f, 0f, 0f);
        }

        else 
        {
            // Slow move and saw wave movement
            transform.Translate(-activeMoveSpeed * Time.deltaTime, Time.deltaTime * triangleInverter * activeMoveSpeed, 0f);
        }
        

        CheckShoot(3.5f, 6f);
    }
}
