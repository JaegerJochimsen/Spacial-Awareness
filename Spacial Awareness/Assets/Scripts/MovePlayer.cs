using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    // Oxigen/Health Level
    public int maxHealth = 100;
    public float currentHealth;

    public HealthBar healthBar;
    public float healthDecayCoef;
    // End health Variables
    private float timeStart = 0;

    public float turnSpeed = 15f;
    public float speed = 10f;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    private Animator anim;
    bool IsRunning = false;

    public Transform groundCheck;
    float groundDistance = 0.75f;
    public LayerMask groundMask;
    bool isGrounded = true;
    float inAirPenalty;

    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        // Health start
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        
        body = GetComponent<Rigidbody>();
        anim = gameObject.GetComponentInChildren<Animator>();
        body.centerOfMass = new Vector3(0f, 0.1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // check to see if our player is on the ground or a ground equivalent
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // set inAirPenalty for moveent
        if (isGrounded) { inAirPenalty = 1f; }
        else { inAirPenalty = 0.5f; }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool hasInput = (horizontal != 0f || vertical != 0f);

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();
        // account for in-air movement penalty
        m_Movement *= inAirPenalty;

        // only animate/hover if we are moving
        if (hasInput)
        {
            // here is our hover
            body.AddForce(new Vector3(0f, 10f, 0f));
            // used for animation
            IsRunning = true;

        }
        else 
        {
            // prevents strange rotation due to ground texture
            body.constraints = RigidbodyConstraints.FreezeRotation;
            // deactivate running anim
            IsRunning = false; 
        }

        // jump if press space and we have something to jump from
        // TODO BUG once you get on a platform you cannot jump again becuase player is not on ground
        if (Input.GetKeyDown("space") && isGrounded)
        {
            Jump();
        }

        anim.SetBool("IsRunning", IsRunning);
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
        body.MoveRotation(m_Rotation);
        body.MovePosition(transform.position + m_Movement * Time.deltaTime * speed);

        // Change health over time
        TakeDamage(healthDecayCoef * Time.deltaTime);
        // Kill the player when health is 0
        float health = healthBar.getHealth();
        if (health < 1f)
        {
            Debug.Log("Player must die now");
        }
        
    }

    void Jump()
    {
        Vector3 movement = new Vector3(0.0f, 3.3f, 0.0f);
        body.AddForce(movement * speed, ForceMode.Impulse);
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth((int)currentHealth);
    }
}
