using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PullPlayerOutExit>().isNearExit = false;
    }

    void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<PullPlayerOutExit>().isNearExit = true;
    }
}

