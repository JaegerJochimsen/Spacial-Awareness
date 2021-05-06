using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private Vector3 playerPos;
    private GameObject player;

    //Specific to the instance of the AI (mini vs Medium)
    public float range;
    public float damage;
    public float speed;

    private Rigidbody body;
    private Rigidbody playerBody;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Stylized Astronaut");
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        transform.LookAt(player.transform);
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
        // Zero out all the momentum for the enemy
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;

        //Debug.Log(DogPos * speed * -1f);
        Debug.Log(DogPos);
        body.AddForce(DogPos * speed * -1f, ForceMode.Impulse);
    }

    
    void OnCollisionEnter(Collision other) 
    {

        // If we run into something not the player go in the opposite direction
        if (other.gameObject.CompareTag("Rock") || other.gameObject.CompareTag("Enemy"))
        {
            // Push the Enemy away from the obstruction at an angle
            Vector3 redirect = other.gameObject.transform.position - body.transform.position;
            redirect.x *= 17;

            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;

            body.AddForce(redirect * -30f, ForceMode.Force);

        } 
    }

    // Call the interface for doing damage to the player from KillPlayer.cs 
    void Attack(Vector3 knockback_dir)
    {
        MovePlayer playerBod = player.GetComponent<MovePlayer>();
        KillPlayer health = player.GetComponent<KillPlayer>();

        // if the player has a shield active then negate O2 damage
        if (!playerBod.shielding)
        {
            health.TakeDamage(damage);
        }
        // -1 is so Player goes away from enemy instead of towards
        player.GetComponent<Rigidbody>().AddForce(knockback_dir * 3 * -1, ForceMode.Impulse);



        // TODO: Add text to the screen
    }
}
