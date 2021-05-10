using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseLights : MonoBehaviour
{
    // Start is called before the first frame update
    Light current;
    GameObject[] lights;
    private float rangeModifier;
    
    void Start()
    {
        if(lights == null)
        {
            lights = GameObject.FindGameObjectsWithTag("RedLight");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // want the range of the lights to "pulse between 7 and 15, this rangedModifier will oscillate between 1 and 2
        // so we just need to make our default range 7.5
        rangeModifier = Mathf.Abs(Mathf.Sin(Time.time)) + 1;

        foreach (GameObject light in lights)
        {
            current = light.GetComponentInChildren<Light>();
            current.range = 5f * rangeModifier;
        }
    }
}
