using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lava : MonoBehaviour
{

    //public GameObject lavaParent;
    private Transform[] allChildren;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        allChildren = GetComponentsInChildren<Transform>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == player.name)
        {
            Debug.Log("touched lava");
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Don't need to check for lava if not in a lava zone
        if (player.transform.position.y > 75 && player.transform.position.y < 145)
        {
            foreach (Transform child in allChildren)
            {
                if (Vector3.Distance(player.transform.position, child.position) < 8)
                {
                    Damage();
                    break; // Only take damage once
                }
            }
        }
        
    }

    // Call the interface for doing damage to the player from KillPlayer.cs 
    void Damage()
    {
        MovePlayer playerBod = player.GetComponent<MovePlayer>();
        KillPlayer health = player.GetComponent<KillPlayer>();

        // if we are shielding then negate damage
        if (!playerBod.shielding)
        {
            health.TakeDamage(.5f);
        }
        // knock player into the air when they touch the lava
    }
}
