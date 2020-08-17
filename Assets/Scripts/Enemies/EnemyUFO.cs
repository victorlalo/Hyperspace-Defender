using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUFO : Enemy
{ 
    float moveTimerAmt = 2f;
    float currMoveTime = 0f;

    Vector2 targetPos;
    [SerializeField] float distanceToJump = 50f;
    
    protected override void Start()
    {
        base.Start();
        targetPos = transform.position;
    }
    protected override void Update()
    { 
        CheckMove();
        CheckShoot(1.5f, 5f);

        base.Update();
    }

    void CheckMove()
    {
        transform.Rotate(0, 100 * Time.deltaTime, 0);

        if (Mathf.Abs(targetPos.sqrMagnitude - transform.position.sqrMagnitude) > 1f)
        {
            transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime/5f);
        }

        if (currMoveTime > moveTimerAmt)
        {
            currMoveTime = 0f;
            moveTimerAmt = Random.Range(1f, 3.5f);

            // Move to new random location within bounds
            float newX = Mathf.Clamp(transform.position.x + Random.Range(-distanceToJump, distanceToJump), camBounds.bottomLeftCorner.x + 10f, camBounds.topRightCorner.x - 10f);
            float newY = Mathf.Clamp(transform.position.y + Random.Range(-distanceToJump, distanceToJump), camBounds.bottomLeftCorner.y + 10f, camBounds.topRightCorner.y - 10f);
            targetPos = new Vector2(newX, newY);
        }
        else
        {
            currMoveTime += Time.deltaTime;
        }
    }
}
