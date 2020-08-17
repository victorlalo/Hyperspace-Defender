using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPromptGlow : MonoBehaviour
{
    Text keyPromptText;
    float alphaInverter = 1f;
    public float glowSpeed = 10f;

    void Start()
    {
        keyPromptText = GetComponent<Text>();
    }


    void Update()
    {
        if (isActiveAndEnabled)
        {
            if (keyPromptText.color.a > .9f)
            {
                alphaInverter = -1f;
            }
            else if (keyPromptText.color.a < .1f)
            {
                alphaInverter = 1f;
            }
            keyPromptText.color = new Color(keyPromptText.color.r, keyPromptText.color.g, keyPromptText.color.b, (keyPromptText.color.a + glowSpeed * alphaInverter * Time.deltaTime));
        }
        
    }
}
