using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CameraBounds
{
    public Vector2 bottomLeftCorner;
    public Vector2 topRightCorner;
    public Vector2 centerOfScreen;

}

public class GameUtils : MonoBehaviour
{
    Camera cam;
    CameraBounds bounds;

    void Awake()
    {
        cam = Camera.main;
        bounds.topRightCorner = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        bounds.bottomLeftCorner = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));

        bounds.centerOfScreen = new Vector2(bounds.topRightCorner.x - bounds.bottomLeftCorner.x / 2f, bounds.topRightCorner.y - bounds.bottomLeftCorner.y / 2f);
    }

    private void Start()
    {
        ObjectPool.Instance.Initialize();

        Vector3 offScreenPos = new Vector3(-100, -100, 0);
        ObjectPool.Instance.GetLargeExplosion(offScreenPos);
        ObjectPool.Instance.GetPlayerExplosion(offScreenPos);
        ObjectPool.Instance.GetSmallExplosion(offScreenPos);
        ObjectPool.Instance.GetLargeExplosion(offScreenPos);
        ObjectPool.Instance.GetPlayerExplosion(offScreenPos);
        ObjectPool.Instance.GetSmallExplosion(offScreenPos);

    }

    public CameraBounds CameraBounds
    {
        get { return bounds; }
    }

    public Vector2 BottomLeftCorner
    {
        get { return bounds.bottomLeftCorner; }
    }

    public Vector2 TopRightCorner
    {
        get { return bounds.topRightCorner; }
    }
}
