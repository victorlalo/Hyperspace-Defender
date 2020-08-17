using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //ObjectPool.Instance.Initialize();

        //ObjectPool.Instance.GetLargeExplosion(Vector3.zero);
        //ObjectPool.Instance.GetPlayerExplosion(Vector3.zero);
        //ObjectPool.Instance.GetSmallExplosion(Vector3.zero);

        SceneManager.LoadScene("Main");

    }
}
