using System.Collections;
using System.Collections.Generic;
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

        // Is the player within range of the crawler
        var x = range >= CrawlerPos.x && CrawlerPos.x >= -range;
        var y = range >= CrawlerPos.y && CrawlerPos.y >= -range;
        var z = range >= CrawlerPos.z && CrawlerPos.z >= -range;

        if (x && y && z)
        {
            Kill();
        }
        
        foreach (GameObject i in objects)
        {
            if (i != null)
            {
                Distance(i);
            }  
        }
        
        
        // Zero out all the momentum for the enemy
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;

        body.AddForce(CrawlerPos * speed * -1f, ForceMode.Impulse);
    }

    
    /*
     * Check if the distance from the crawler is close enough 
     * to the object that it needs to be destroyed.
     * 
     * 3 postions on the crawler are checked: The head, right and left legs
     * 
     */

    void Distance(GameObject rock)
    {

        /*Vector3 CrawlerPos = transform.position;

        CrawlerPos.y += 13.2f;
        CrawlerPos.z += -4.8f;

        CrawlerPos -= rock.transform.position;
        */

        //var GetComponentInChildren<Collider>();
        var absolute_pos = head.transform.position + GameObject.Find("Crawler").transform.position;

        // check if the head of the crawler is within range of the object
        var x = range >= absolute_pos.x && absolute_pos.x >= -range;
        var y = range >= absolute_pos.y && absolute_pos.y >= -range;
        var z = range >= absolute_pos.z && absolute_pos.z >= -range;

        if (x && y && z)
        {
            Destroy(rock);
        }

        /*
         
        Below is for later refinement
         
        CrawlerPos.x += 1f;
        CrawlerPos.y += .1f;
        CrawlerPos.z += -1f;

        // check if the right leg of the crawler is within range of the object
        x = range >= CrawlerPos.x && CrawlerPos.x >= -range;
        y = range >= CrawlerPos.y && CrawlerPos.y >= -range;
        z = range >= CrawlerPos.z && CrawlerPos.z >= -range;

        if (x && y && z)
        {
            Destroy(rock);
        }

        CrawlerPos.x += -2f;

        // check if the left leg of the crawler is within range of the object
        x = range >= CrawlerPos.x && CrawlerPos.x >= -range;
        y = range >= CrawlerPos.y && CrawlerPos.y >= -range;
        z = range >= CrawlerPos.z && CrawlerPos.z >= -range;

        if (x && y && z)
        {
            Destroy(rock);
        }
        */

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
