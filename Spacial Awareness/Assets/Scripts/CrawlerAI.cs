using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerAI : MonoBehaviour
{

    private Vector3 playerPos;
    private GameObject player;
    private Rigidbody playerBody;
    private Rigidbody crawlerBody;

    public float attackRange;

    // Radius from the nest that the crawlers warn the player or attack
    public float attackZone;
    public float warningZone;

    // Location of the nest
    public Transform nest;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GameObject.Find("Stylized Astronaut").GetComponent<Rigidbody>();
        player = GameObject.Find("Stylized Astronaut");
        nest = GetComponent<Transform>();
        crawlerBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        playerPos = GameObject.Find("Stylized Astronaut").transform.position;
        Vector3 CrawlerPos = transform.position;

       // crawlerBody.velocity = Vector3.zero;
        //crawlerBody.angularVelocity = Vector3.zero;

        if (Vector3.Distance(playerPos, nest.position) <= attackZone)
        {
            if (Vector3.Distance(playerPos, CrawlerPos) <= attackRange)
            {
                Attack(CrawlerPos);
            } 
            else
            {
                crawlerBody.AddForce(CrawlerPos - playerPos * 10, ForceMode.Impulse);
            }
        }

        else if (Vector3.Distance(playerPos, nest.position) <= warningZone)
        {
            transform.LookAt(playerPos);
            // TODO: Insert a warning sound or particle effect here.
        }
    }

    // Call the interface for doing damage to the player from KillPlayer.cs 
    void Attack(Vector3 knockback_dir)
    {
        MovePlayer playerBod = player.GetComponent<MovePlayer>();
        KillPlayer health = player.GetComponent<KillPlayer>();

        // if we are shielding then negate damage
        if (!playerBod.shielding)
        {
            health.TakeDamage(1f);
        }

        // -1 is so Player goes away from enemy instead of towards
        //playerBody.AddForce(knockback_dir * 3, ForceMode.Impulse);

    }
}
