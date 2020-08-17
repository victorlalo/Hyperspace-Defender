using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float activeMoveSpeed = 40f;
    protected float moveSpeed = 15f;

    Rigidbody rb;

    public float BulletSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        float moveAmt = moveSpeed * Time.deltaTime;
        transform.Translate(0, -moveAmt , 0);
    }

    public void StartMoving()
    {
        moveSpeed = activeMoveSpeed;
    }

    public void StopMoving()
    {
        rb.velocity = Vector3.zero;
        moveSpeed = 0f;
    }

    protected virtual void OnBecameInvisible()
    {
        if (gameObject.CompareTag("Bullet"))
            ObjectPool.Instance.ReturnBullet(gameObject);
        else if (gameObject.CompareTag("EnemyBullet"))
            ObjectPool.Instance.ReturnEnemyBullet(gameObject);
    }
}
