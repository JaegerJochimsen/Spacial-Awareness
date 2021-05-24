using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeLightPulse : MonoBehaviour
{
    // Start is called before the first frame update
    Light current;
    GameObject[] lights;
    private float flickerMod;

    void Start()
    {
        if (lights == null)
        {
            lights = GameObject.FindGameObjectsWithTag("TubeLight");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // want the range of the lights to "pulse between 7 and 15, this rangedModifier will oscillate between 1 and 2
        // so we just need to make our default range 7.5

        foreach (GameObject light in lights)
        {
            flickerMod = Random.value;
            current = light.GetComponentInChildren<Light>();
            current.intensity = 3f * flickerMod;
        }
    }
}
