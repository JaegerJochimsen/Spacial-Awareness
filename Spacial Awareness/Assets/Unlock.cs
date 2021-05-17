using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlock : MonoBehaviour
{
    public bool locked = true;
    public bool acquiredKey = false;
    Vector3 closedLockPos = new Vector3(-1.56386e-17f, 2.605912f, 0.8099456f);
    Vector3 boltOpenPos = new Vector3(0.17f, 3.64f, 1.15f);
    public GameObject bolt;
    public Rigidbody player;
    // Start is called before the first frame update
    void Start()
    {
        bolt.transform.position = closedLockPos;
        locked = true;
        acquiredKey = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckForPlayer()
    {
        if ((player.transform.position - transform.position).sqrMagnitude < 1f)
        {
            if (acquiredKey)
            {
                StartCoroutine("Open");
            }
        }
    }

    IEnumerator "Open"
}
