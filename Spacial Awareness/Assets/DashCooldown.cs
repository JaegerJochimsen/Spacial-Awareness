using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCooldown : MonoBehaviour
{
    public GameObject dashCooldown;
    public bool isCooldown;
    public int seconds = 3;
    // Start is called before the first frame update
    void Start()
    {
        dashCooldown.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (isCooldown == false)
            {
                dashCooldown.SetActive(true);

//                dashCooldown.SetActive(false);
            } 
        }
    }
}
