using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ExplosionType { SMALL, LARGE, PLAYER};
public class ExplosionTimer : MonoBehaviour
{
    [SerializeField] ExplosionType type;
    ParticleSystem ps;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
       if (!ps.isEmitting)
        {
            //print("DONE");
            if (type == ExplosionType.SMALL)
                ObjectPool.Instance.ReturnSmallExplosion(gameObject);
            else if (type == ExplosionType.LARGE)
                ObjectPool.Instance.ReturnLargeExplosion(gameObject);
            else if (type == ExplosionType.PLAYER)
                ObjectPool.Instance.ReturnPlayerExplosion(gameObject);
        }
    }
}
