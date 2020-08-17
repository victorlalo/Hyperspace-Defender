using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlue : Enemy
{
    float amplitude = .25f;
    float beginTime = 0;

    private void OnEnable()
    {
        // start time used as phase offset
        beginTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckShoot(2f, 4f);
        // move in sinusoidal path
        transform.Translate(-activeMoveSpeed * Time.deltaTime, amplitude * Mathf.Sin(activeMoveSpeed/5 * Time.time + beginTime), 0f);
    }
}
