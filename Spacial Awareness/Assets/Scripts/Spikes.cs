using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Stylized Astronaut");
    }

    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.CompareTag("Player"))
        {
            Damage();
        }
    }

    void Damage()
    {

        KillPlayer health = player.GetComponent<KillPlayer>();
        health.TakeDamage(.6f);

    }
}
