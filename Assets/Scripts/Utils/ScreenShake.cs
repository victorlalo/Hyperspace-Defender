using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    float screenShakeTimeAmount = 2f;
    float currTimer = 0f;

    float intensity = 2f;
    bool shakeScreen = false;


    Vector3 originalPos;

    void Start()
    {
        originalPos = transform.position;
    }

    public void ShakeScreen(float amt, float length)
    {
        intensity = amt;
        screenShakeTimeAmount = length;

        shakeScreen = true;
        currTimer = 0f;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    shakeScreen = true;
        //}

        if (shakeScreen)
        {
            if (currTimer < screenShakeTimeAmount)
            {
                transform.position = originalPos + Random.insideUnitSphere * intensity;
                currTimer += Time.deltaTime;
            }
            else
            {
                transform.position = originalPos;
                shakeScreen = false;
                currTimer = 0f;
            }
        }
    }
}
