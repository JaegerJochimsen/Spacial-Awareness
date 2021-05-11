using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    bool pressable;
    public Material pressedMat;
    void Start()
    {
        pressable = true;
    }
    void OnTriggerEnter(Collider col)
    {
        if (pressable)
        {
            FindObjectOfType<SlideAway>().slideThreshold -= 1;
            this.GetComponent<MeshRenderer>().material = pressedMat;
            pressable = false;
        }
    }
}
