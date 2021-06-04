using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MamaCrawlerAI : MonoBehaviour
{

    [SerializeField] FlashImage _flashImage = null;

    private Vector3 playerPos;
    private GameObject player;

    private Rigidbody body;

    private float speedChange;

    public float speed;
    private Collider head;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Stylized Astronaut");
        body = GetComponent<Rigidbody>();
        head = GetComponentInChildren<Collider>();
        speedChange = 1f;

    }

    void FixedUpdate()
    {

        playerPos = GameObject.Find("Stylized Astronaut").transform.position;

        if (playerPos.y < transform.position.y)
        {
            Kill();
        }

        Vector3 CrawlerPos = transform.position;
        CrawlerPos -= playerPos;
        CrawlerPos.z = 0;

        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;

        Set_Speed();

        body.AddForce(CrawlerPos * speed * -1f * speedChange, ForceMode.Impulse);
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
            _flashImage.StartFlash(.25f, 1f, Color.red);
            Kill();
        }
    }

    // Call the interface for doing damage to the player from KillPlayer.cs 
    void Kill()
    {

        // KillPlayer health = player.GetComponent<KillPlayer>();
        //health.TakeDamage(410f);
        FindObjectOfType<GameManager>().EndGame();
    }

    // Change the speed of the crawler based on the postion of the player
    void Set_Speed()
    {
        if (player.transform.position.y >= 270)
        {
            speedChange = 2f;
        }
        else if (player.transform.position.y >= 145)
        {
            speedChange = 1.7f;
        }
        else
        {
            speedChange = 1f;
        }
    }
}
