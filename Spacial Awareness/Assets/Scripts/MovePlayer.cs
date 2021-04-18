using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    // Movement variables
    public float turnSpeed = 15f;
    public float speed = 10f;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    bool hasInput = false;
    // End movement variables

    // Animation variables
    private Animator anim;
    bool IsRunning = false;
    // End animation variables

    // Ground-check variables
    public Transform groundCheck;
    float groundDistance = 0.65f;
    public LayerMask groundMask;
    public bool isGrounded = true;
    float inAirPenalty;
    // End ground check variables

    // JetPack variables
    public ParticleSystem JetParticles;
    //bool particleSystemPlayed = false;
    public bool flying = false;
    // End JetPack variables

    public Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {

        body = GetComponent<Rigidbody>();
        anim = gameObject.GetComponentInChildren<Animator>();

        // Jetpack
        JetParticles = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // check to see if our player is on the ground or a ground equivalent
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // determine if we need to reduce movement dexterity due to being in the air
        // set penalty value accordingly
        SetInAirPenalty();

        // handle input from keyboard, put into Vector3 for MoveAndLook() later, and apply inAirPenalty 
        HandleMovementInput();

        // check to see if we should play the animation, then set the bool
        SetRunningAnimBool();

        // constrain various player rotation/movement so that if we aren't moving we don't fall over or
        // fall off of the platform we've landed on because of the low poly mesh
        Constrain();

        // jump if press space and we have something to jump from
        if (Input.GetKeyDown("space") && isGrounded)
        {
            Jump();
        }

        // handle logic for using jetpack and apply force + play particle effects
        // TODO: possibly remove commented particle code ==> since this is Sam's jurisdiction I've left it commented out
        // it is up to Sam which of the changes I made we keep and how the final implementation works out
        JetPack();

        // apply player movement and look rotation to character model
        MoveAndLook();
    }


    /* MoveAndLook():
     * :description: transform keyboard input (stored in m_Movement) into rotation and movement vectors. Applies movement and rotaion
     *               to the character model based off of those vectors.
     * :param: n/a
     * :dependency: HandleMovementInput(): relies on function to fill m_Movement vector and apply inAirPenalty
     * 
     * :calls: n/a
     * :called by: Update().
     */
    void MoveAndLook()
    {
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
        body.MoveRotation(m_Rotation);
        body.MovePosition(transform.position + m_Movement * Time.deltaTime * speed);
    }

    /* SetInAirPenalty():
     * :description: set modifier (inAirPenalty) based off of grounded status. If player is in the air we reduce dexterity by 35%
     * :param: n/a
     * :dependency: relies on Physics.CheckSphere() call at the top of Update() to determine grounded status.
     * 
     * :calls: n/a
     * :called by: Update().
     */
    void SetInAirPenalty()
    {
        // set inAirPenalty for moveent
        if (isGrounded) { inAirPenalty = 1f; }
        else { inAirPenalty = 0.65f; }
    }

    /* HandleMovementInput():
     * :description: intake keyboard input and sets vector m_Movement based off of that input. Sets the hasInput bool. Applies inAirPenalty.
     * :param: n/a
     * :dependency: n/a
     * 
     * :calls: n/a
     * :called by: Update().
     */
    void HandleMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        hasInput = (horizontal != 0f || vertical != 0f);

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();
        // account for in-air movement penalty
        m_Movement *= inAirPenalty;
    }

    /* Constrain():
     * :description: applies movement and rotation constraints to player model so that uneven, low-poly terrain doesn't tip over or spin around
     *               the player. Also helps when landing on a platform/rock so that player doesn't slip off arbitrarily.
     * :param: n/a
     * :dependency: isGrounded: relies on isGrounded to determine when we need to constrain x and z position. This should happen so if we land on 
     *                          an asteroid/platform we don't slide off due to the shape of the platform. Set at the top of Update().
     *              hasInput: relies on hasInput so that we only apply constraints when the player isn't moving intentionally. We don't want to 
     *                        inhibit voluntary movement, only involuntary movement. Set in HandleMovementInput().
     *                        
     * :calls: n/a
     * :called by: Update().
     */
    void Constrain()
    {
        if (!hasInput)
        {
            body.constraints = RigidbodyConstraints.FreezeRotation;

            // if we land on a platform and stop, then make it so that we don't just slip off
            if (isGrounded)
            {
                body.constraints = RigidbodyConstraints.FreezePositionX;
                body.constraints = RigidbodyConstraints.FreezePositionZ;
            }
        }
    }

    /* SetRunningAnimBool():
     * :description: set IsRunning bool used to trigger running animation.
     * :param: n/a
     * :dependency: hasInput: if we have input then we are running/moving and should trigger the animation. Set in HandleMovementInput().
     * 
     * :calls: n/a
     * :called by: Update().
     */
    void SetRunningAnimBool()
    {
        // only animate/hover if we are moving
        if (hasInput)
        {
            // used for animation
            IsRunning = true;

        }
        else
        {
            // deactivate running anim
            IsRunning = false;
        }
        anim.SetBool("IsRunning", IsRunning);
    }

    /* Jump():
     * :description: apply jump force to character.
     * :param: n/a
     * :dependency: n/a
     * 
     * :calls: n/a
     * :called by: Update().
     */
    void Jump()
    {
        Vector3 jump = new Vector3(0.0f, 3.5f, 0.0f);
        body.AddForce(jump * speed, ForceMode.Impulse);
    }



    /* JetPack():
     * :description: implement jet pack ability; when space is held down apply vertical force to the character and play jet effect, when 
     *               space is released stop applying force and stop playing particle effect. Sets flying bool.
     * :param: n/a
     * :dependency: isGrounded: used because we should only be able to use the jet pack in the air so we need to see if we are grounded. Set at top of Update().
     *              
     * :calls: TakeDamage(): using jet pack ability reduces O2 stores and lowers player O2 level.
     * :called by: Update().
     */
    void JetPack()
    {
        // We are in the air, so use JetPack
        if (Input.GetKeyDown("space") && !isGrounded)
        {
            // TODO: Decide on an amount of damage to take
            FindObjectOfType<KillPlayer>().TakeDamage(0.5f);
            flying = true;
            //JetPack();
        }
        else if (Input.GetKeyUp("space"))
        {
            flying = false;
            JetParticles.Stop();
        }

        if (flying)
        {
            Vector3 fly = new Vector3(0.0f, 3.5f, 0.0f);
            body.AddForce(fly * speed, ForceMode.Force);
            JetParticles.Play();
        }
        // var dup_particle = Instantiate(JetParticles, JetParticles.transform.position, Quaternion.identity) as ParticleSystem;
        // dup_particle.Play();
        // Destroy(dup_particle, 1);

  //      if (!particleSystemPlayed)
    //    {
      //      JetParticles.Play();
        //    particleSystemPlayed = true;
//        } 
  //      else
    //    {
      //      particleSystemPlayed = false;

        //    JetParticles.Stop();
       // }
        // How to turn off

        //yield WaitForSeconds(5);
        //JetParticles.Stop();
    }
}
 