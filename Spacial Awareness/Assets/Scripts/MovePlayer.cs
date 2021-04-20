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
    public bool IsRunning = false;
    public bool IsJumping = false;
    public bool IsLanding = false;
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
    bool flying = false;
    // End JetPack variables

    // Shield variables
    public GameObject ForceField;
    public float forceFieldStrength;
    public bool shielding = false;
    MeshRenderer render;
    Color color;
    public float fullAlpha = 103f;
    public float minAlpha = 0f;
    public float t = 0f;
    public float fadeSpeed = 1f;
    // End Shield variables

    public Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {

        body = GetComponent<Rigidbody>();
        anim = gameObject.GetComponentInChildren<Animator>();

        // Forcefield
        render = ForceField.GetComponent<MeshRenderer>();
        color = render.material.color;

        // Jetpack
        JetParticles = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // check to see if our player is on the ground or a ground equivalent
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        anim.SetBool("IsGrounded", isGrounded);
     
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

        // toggle shield on/off 
        if(Input.GetKeyDown("left shift"))
        {
            shielding ^= true;
            //Shield();
        }

        if (shielding)
        {
            if (render.material.color.a < 103f)
            {
                color.a += Time.deltaTime * fadeSpeed;
                render.material.color = color;
            }
            if(Mathf.Approximately(render.material.color.a, 103f))
            {
                ForceField.SetActive(true);
            }
        }
        if(!shielding)
        {
            if(render.material.color.a > 0f)
            {
                color.a -= Time.deltaTime * fadeSpeed;
                render.material.color = color;
            }
            if (Mathf.Approximately(render.material.color.a, 0f))
            {
                ForceField.SetActive(false);
            }
        }


        // handle logic for using jetpack and apply force + play particle effects
        // TODO: possibly remove commented particle code ==> since this is Sam's jurisdiction I've left it commented out
        // it is up to Sam which of the changes I made we keep and how the final implementation works out
        JetPack();

        // apply player movement and look rotation to character model
        MoveAndLook();
    }

    /* Shield():
     * :description: activate/deactivate bubble shield that will prevent incoming damage from enemies; knockback from being attacked still applies.
     *               Sets (shielding) boolean that is used in enemy AI script to deal damage in Attack() function.
     *               
     * :param: n/a
     * :dependency: n/a
     * 
     * :calls: n/a
     * :called by: Update();
     */
    void Shield()
    {
        // TODO: add energy/O2 cost
        // alternate between on and off
        shielding ^= true;
        // this just activates/deactivates the GameObject
        ForceField.SetActive(shielding);
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
     * :description: set animation bool used to trigger running animations.
     * :param: n/a
     * :dependency: hasInput: if we have input then we are running/moving and should trigger the animation. Set in HandleMovementInput().
     * 
     * :calls: n/a
     * :called by: Update().
     */
    void SetRunningAnimBool()
    {

        // only animate/hover if we are moving
        if (hasInput && isGrounded)
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
            flying = true;
            anim.SetBool("IsFlying", flying);
        }
        else if (Input.GetKeyUp("space"))
        {
            flying = false;
            anim.SetBool("IsFlying", flying);
            JetParticles.Stop();
        }

        if (flying)
        {
            // TODO: Decide on an amount of damage to take
            FindObjectOfType<KillPlayer>().TakeDamage(0.1f);
            Vector3 fly = new Vector3(0.0f, 3.5f, 0.0f);
            body.AddForce(fly * speed, ForceMode.Force);
            JetParticles.Play();
        }
    }
}
 