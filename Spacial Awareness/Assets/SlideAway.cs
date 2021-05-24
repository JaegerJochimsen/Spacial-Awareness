using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideAway : MonoBehaviour
{
    public int slideThreshold = 3;
    public GameObject selfRef;
    Vector3 DoorClosed;
    Vector3 DoorOpen;
    float r;
    public bool triggered = false;
    //public GameObject otherRef;
    void Start()
    {
        DoorClosed = new Vector3(transform.position.x, transform.position.y, 0.05f);
        DoorOpen = new Vector3(transform.position.x, transform.position.y, 4.8f);
        r = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (triggered)
        {
            if (r < 1f)
            {
                transform.localPosition = (1 - r) * DoorClosed + r * DoorOpen;
                r += 0.001f * Time.deltaTime;
            }
        }
    }
}
