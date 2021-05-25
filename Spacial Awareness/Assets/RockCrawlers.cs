using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCrawlers : MonoBehaviour
{

    private GameObject player;
    private Transform pos;
    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Stylized Astronaut");
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        pos = GetComponent<Transform>();

        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(target);

        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
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
        health.TakeDamage(3f);

    }
}
