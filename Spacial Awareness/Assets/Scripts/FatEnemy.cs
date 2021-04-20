using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatEnemy : MonoBehaviour
{

    private Vector3 playerPos;
    private GameObject player;
    public float range;
    private Rigidbody playerBody;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GameObject.Find("Stylized Astronaut").GetComponent<Rigidbody>();
        player = GameObject.Find("Stylized Astronaut");
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        playerPos = GameObject.Find("Stylized Astronaut").transform.position;
        Vector3 DogPos = transform.position;

        DogPos -= playerPos;
   

        var x = range >= DogPos.x && DogPos.x >= -range;
        var y = range >= DogPos.y && DogPos.y >= -range;
        var z = range >= DogPos.z && DogPos.z >= -range;

        // Player is within attack range of Enemy
        if (x && y && z)
        {
            Attack(DogPos);
        }

        transform.LookAt(player.transform);
       
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
        playerBody.AddForce(knockback_dir * 3 * -1, ForceMode.Impulse);
       
        // TODO: Add text to the screen
    }
}


/* 
 * 3 Classes of Enemy:
 * 1: Fast + weak Damage
 * 2: Medium speed + medium Damage
 * 3: Stationary + Large Damage
 * Knock Back
 * 
 */