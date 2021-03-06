using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool batteryAquired = false;
    public GameObject deactivePortal;
    public GameObject activePortal;
    public GameObject PostExitFX;
    // Update is called once per frame
    private void Start()
    {
        deactivePortal.SetActive(true);
        activePortal.SetActive(false);
        PostExitFX.SetActive(false);
    }
    void Update()
    {
        // once we acquire the battery
        if (batteryAquired)
        {
            deactivePortal.SetActive(false);
            activePortal.SetActive(true);
            PostExitFX.SetActive(true);
        }
    }
}
