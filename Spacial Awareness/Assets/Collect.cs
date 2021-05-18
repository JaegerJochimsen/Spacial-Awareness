using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public float speed;
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<Unlock>().acquiredKey = true;
        Destroy(gameObject);
    }
    private void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
