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
    public bool doubleJumped = false;
    // End movement variables

    // Animation variables
    private Animator anim;
    bool IsRunning = false;
    bool IsJumping = false;
    bool IsLanding = false;
    // End animation variables

    // Ground-check variables
    public Transform groundCheck;
    float groundDistance = 0.8f;
    public LayerMask groundMask;
    public bool isGrounded = true;
    // End ground check variables

    // Dash vars
    float dashForce = 10f;
    //

    // JetPack variables
    public ParticleSystem JetParticles;
    bool flying = false;
    // End JetPack variables

    // Shield variables
    public GameObject ForceField;
    public bool shielding = false;
    public float t = 0f;
    private Vector3 shrunkSize = new Vector3(0.01f, 0.01f, 0.01f);
    private Vector3 fullSize = new Vector3(0.5f, 0.5f, 0.5f);
    // End Shield variables

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
        anim.SetBool("IsGrounded", isGrounded);
        

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

        //DoubleJump();

        Shield();
        
        // handle logic for using jetpack and apply force + play particle effects
        // TODO: possibly remove commented particle code ==> since this is Sam's jurisdiction I've left it commented out
        // it is up to Sam which of the changes I made we keep and how the final implementation works out
        JetPack();        
        
        // apply player movement and look rotation to character model
        MoveAndLook();
    }

    void DoubleJump()
    {
        if (Input.GetKeyDown("space") && !doubleJumped && !isGrounded)
        {
            Jump();
            doubleJumped = true;
        }
        if (!isGrounded) { doubleJumped = false; }
    }

    /* Shield():
     * :description: activate/deactivate bubble shield that will prevent incoming damage from enemies; knockback from being attacked still applies.
     *               Handles growing and shrinking effect of the bubble shield as well.
     *               Sets (shielding) boolean that is used in enemy AI script to deal damage in Attack() function.
     *               
     * :param: n/a
     * :dependency: n/a
     * 
     * :calls: n/a
     * :called by: Update();
     * 
     * :credit: idea for shrink/grow came from Emilee McDonald
     */
    void Shield()
    {
        // TODO: add energy/O2 cost

        // toggle shield on/off 
        if (Input.GetKeyDown("left shift"))
        {
            // alternate between on and off
            shielding ^= true;
        }

        // if we want to shrink and we are still bigger than our minimum size continue to shrink
        if (shielding && (ForceField.transform.localScale.x < fullSize.x))
        {
            ForceField.transform.localScale *= Mathf.Lerp(1f, 3f, t);
            t += 0.1f * Time.deltaTime;
        }

        // if we want to shrink
        if (!shielding)
        {
            // if we still have room to grow, i.e. we are smaller than our full size
            if (ForceField.transform.localScale.x > shrunkSize.x)
            {
                ForceField.transform.localScale /= Mathf.Lerp(1f, 3f, t);
                t += 0.1f * Time.deltaTime;
            }
            // also, if we have accidentally grown too much or we are ESSENTIALLY done growing, grow back to our usual size
            if ((ForceField.transform.localScale.x < shrunkSize.x) || (Mathf.Approximately(ForceField.transform.localScale.x, shrunkSize.x)))
            {
                ForceField.transform.localScale = shrunkSize;
                // we're done growing/shrinking so disable lerp
                t = 1f;
            }
        }
        // reset for next lerp
        if (t == 1f) { t = 0f; }
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

        // Lock Z
        // Add constraints into the movement so that we just don't execute a move if we stray too far
        if ((transform.position.z > 10f) || (transform.position.z < -10f))
        {
            // handle signage
            float newZ = (transform.position.z > 0) ? 9.9f : -9.9f;
            body.MovePosition(new Vector3(transform.position.x, transform.position.y, newZ));
            
        }
        // Lock X
        else if ((transform.position.x > 25f) || (transform.position.x < -25f))
        {
            float newX = (transform.position.x > 0) ? 24.9f : -24.9f;
            body.MovePosition(new Vector3(newX, transform.position.y, transform.position.z));

        }
        // if we are within the bounds move as normal
        else { body.MovePosition(transform.position + m_Movement * Time.deltaTime * speed); }
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
        body.constraints = RigidbodyConstraints.FreezeRotation;

        if (!hasInput)
        {

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
        Vector3 jump = new Vector3(0.0f, 7f, 0.0f);
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
            FindObjectOfType<KillPlayer>().TakeDamage(0.3f);
            Vector3 fly = new Vector3(0.0f, 10f, 0.0f);
            body.AddForce(fly * speed, ForceMode.Force);
            JetParticles.Play();
        }
    }
}
 