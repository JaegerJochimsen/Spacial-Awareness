using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlock : MonoBehaviour
{
    public bool locked = true;
    public bool acquiredKey = false;
    public bool openNest = false;
    Vector3 boltClosedPos = new Vector3(-1.56386e-17f, 2.605912f, 0.8099456f);
    Vector3 boltOpenPos = new Vector3(0.09f, 3.76f, 0.74f);
    Vector3 nestDoorClosed = new Vector3(-0.82f, -1.44f, -44.86f);
    Vector3 nestDoorOpen = new Vector3(-5.62f, -1.44f, -44.86f);
    float t = 0f;
    float r = 0f;
    public GameObject bolt;
    public GameObject nestLock;
    public Rigidbody player;
    // Start is called before the first frame update
    void Start()
    {
        bolt.transform.localPosition = boltClosedPos;
        nestLock.transform.localPosition = nestDoorClosed;
        locked = true;
        acquiredKey = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayer();
        Open();
    }

    void Open()
    {
        if (!locked)
        {
            if (t < 1f)
            {
                bolt.transform.localPosition = (1 - t) * boltClosedPos + t * boltOpenPos;
                t += 1f * Time.deltaTime;
            }
            else { openNest = true; }
        }

        if (openNest)
        {
            if(r < 1f)
            {
                nestLock.transform.localPosition = (1 - r) * nestDoorClosed + r * nestDoorOpen;
                r += 0.25f * Time.deltaTime;
            }
        }
    }
    void CheckForPlayer()
    {
        if ((player.transform.position - transform.position).sqrMagnitude < 49f)
        {
            if (acquiredKey)
            {
                locked = false;
            }
        }
    }
}
