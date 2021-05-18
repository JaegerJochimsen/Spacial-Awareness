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
    private Transform nest;

    // Nest flag: 1 for nest 1, 2 for 2nd nesr
    public int nestNumber;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GameObject.Find("Stylized Astronaut").GetComponent<Rigidbody>();
        player = GameObject.Find("Stylized Astronaut");
        crawlerBody = GetComponent<Rigidbody>();

        /* Super hacky solution:
         * The script was not grabing the correct nest object attachted
         * through unity, so I made a flag to determine which nest the crawler
         * is attachted to. Since we only have two nests it isnt a big deal.
         */

        if (nestNumber == 1)
        {
            nest = GameObject.Find("nest_1").GetComponent<Transform>();
        }
        else
        {
            nest = GameObject.Find("nest_2").GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        playerPos = GameObject.Find("Stylized Astronaut").transform.position;
        Vector3 CrawlerPos = transform.position;

       // crawlerBody.velocity = Vector3.zero;
        crawlerBody.angularVelocity = Vector3.zero;

        if (Vector3.Distance(playerPos, nest.position) <= attackZone)
        {
            if (Vector3.Distance(playerPos, CrawlerPos) <= attackRange)
            {
                Attack(CrawlerPos);
            } 
            else
            {
                //crawlerBody.AddForce(CrawlerPos - playerPos * 1, ForceMode.Impulse);
                transform.position += transform.forward * 2 * Time.deltaTime;
            } 
            //Debug.Log("In attack zone0");
            transform.LookAt(playerPos);
        }

        else if (Vector3.Distance(playerPos, nest.position) <= warningZone)
        {
            //Debug.Log("In warning zone0");
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
