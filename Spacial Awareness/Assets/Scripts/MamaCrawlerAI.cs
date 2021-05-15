using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MamaCrawlerAI : MonoBehaviour
{

    private Vector3 playerPos;
    private GameObject player;

    private Rigidbody body;
    private Rigidbody playerBody;

    public float speed;
    public float range;
    public Collider head;

    private GameObject[] objects;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Stylized Astronaut");
        body = GetComponent<Rigidbody>();
        objects = GameObject.FindGameObjectsWithTag("destroy");
        head = GetComponentInChildren<Collider>();

    }

    void FixedUpdate()
    {

        transform.LookAt(player.transform);
        playerPos = GameObject.Find("Stylized Astronaut").transform.position;
        
        Vector3 CrawlerPos = transform.position;
        CrawlerPos -= playerPos;

        // Zero out all the momentum for the enemy
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;

        body.AddForce(CrawlerPos * speed * -1f, ForceMode.Impulse);
    }


    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.CompareTag("destroy"))
        {
            Destroy(col.gameObject);
            return;
        }


        if (col == GameObject.Find("Stylized Astronaut").GetComponent<Collider>())
        {
            Kill();
        }
    }

    // Call the interface for doing damage to the player from KillPlayer.cs 
    void Kill()
    {
        
        MovePlayer playerBod = player.GetComponent<MovePlayer>();
        KillPlayer health = player.GetComponent<KillPlayer>();

        // if the player has a shield active then negate O2 damage
        if (!playerBod.shielding)
        {
            health.TakeDamage(100000000f);
        }
        
    }
}
