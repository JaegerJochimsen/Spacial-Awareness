using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullPlayerOutExit : MonoBehaviour
{

    public Rigidbody playerBody;
    public Transform exitPoint;
    public bool isNearExit = false;
    public float suckForce;

    // Update is called once per frame
    void Update()
    {
        if (isNearExit)
        {
            Vector3 dir = exitPoint.position - playerBody.transform.position;
            dir.Normalize();
            playerBody.AddForce(dir*suckForce, ForceMode.VelocityChange);
        }
    }
}
