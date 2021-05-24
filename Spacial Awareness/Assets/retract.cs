using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class retract : MonoBehaviour
{
    Vector3 DoorClosed;
    Vector3 DoorOpen;
    public float r = 0f;
    public float slideThreshold = 3f;
    // Start is called before the first frame update
    void Start()
    {
        DoorClosed = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.05f);
        DoorOpen = new Vector3(transform.localPosition.x, transform.localPosition.y, 4.8f);
    }

    // Update is called once per frame
    void Update()
    {
        if (slideThreshold == 0f)
        {
            if(r < 1f)
            {
                transform.localPosition = (1 - r) * DoorClosed + r * DoorOpen;
                r += 0.2f * Time.deltaTime;
                Debug.Log("R: " + r.ToString());
            }
        }
    }
}
