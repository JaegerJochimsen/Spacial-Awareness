using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCollect : MonoBehaviour
{
    public float speed;
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<Portal>().batteryAquired = true;
        Destroy(gameObject);
    }
}
