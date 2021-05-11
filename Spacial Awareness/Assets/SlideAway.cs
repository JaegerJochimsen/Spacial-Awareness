using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideAway : MonoBehaviour
{
    public int slideThreshold = 3;
    public GameObject selfRef;
    //public GameObject otherRef;

    void Start()
    {
        //otherRef.SetActive(false);
        selfRef.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (slideThreshold == 0)
        {
            selfRef.SetActive(false);
            //otherRef.SetActive(true);
        }
    }
}
