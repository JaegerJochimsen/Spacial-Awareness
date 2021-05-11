using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    bool pressable;
    void Start()
    {
        pressable = true;
    }
    void OnTriggerEnter(Collider col)
    {
        if (pressable)
        {
            FindObjectOfType<SlideAway>().slideThreshold -= 1;
            pressable = false;
        }
    }
}
