using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    // Movement variables
    float turnSpeed = 12f;
    float speed = 15f;
    float jumpHeight = 50f;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    bool hasInput = false;
    bool doubleJumped = false;
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
    bool isGrounded = true;
    // End ground check variables

    // Dash vars
    private bool onCoolDown = false;
    private int dashCoolDown = 3;
    private float dashLastUse = 0f;
    // End Dash vars

    // JetPack variables
    public ParticleSystem JetParticles;
    float jetForce = 2f;
    bool flying = false;
    // End JetPack variables

    // Shield variables
    public GameObject ForceField;
    public bool shielding = false;
    float t = 0f;
    private Vector3 shrunkSize = new Vector3(0.01f, 0.01f, 0.01f);
    private Vector3 fullSize = new Vector3(0.5f, 0.5f, 0.5f);
    float shieldGrowSpeed = 0.85f;
    float shieldShrinkSpeed = 0.6f;
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

        DoubleJump();

        Dash();

        Shield();
        // apply player movement and look rotation to character model
        MoveAndLook();

        // handle logic for using jetpack and apply force + play particle effects
        // TODO: possibly remove commented particle code ==> since this is Sam's jurisdiction I've left it commented out
        // it is up to Sam which of the changes I made we keep and how the final implementation works out
        JetPack();

        // jump if press space and we have something to jump from
        if (Input.GetKeyDown("space") && isGrounded)
        {
            Jump();
        } 
    }
    /* DoubleJump():
     * :description: allow the player to double jump; maintain x and z velocity but add to y velocity. Sets doubleJumped bool 
     * :param: n/a
     * :dependency: isGrounded: only allow the player to double jump if we aren't on the ground (if we are on the ground then just jump as normal)
     * 
     * :calls: n/a
     * :called by: Update().
     */
    void DoubleJump()
    {

        if (Input.GetKeyDown("space") && !doubleJumped && !isGrounded)
        {

            body.AddForce(new Vector3(body.velocity.x, jumpHeight + Mathf.Abs(body.velocity.y), body.velocity.z), ForceMode.VelocityChange);
            doubleJumped = true;
        }
        if (isGrounded) { doubleJumped = false; } 
    }

    /* Dash()
     *  Dash forward and slightly up with a 3 second cool down
     * :param: n/a
     * :dependency: n/a
     * 
     * :calls: n/a
     * :called by: Update().
     */

    void Dash()
    {
        
        if (onCoolDown) 
        {
            dashLastUse += Time.deltaTime;
            if (dashLastUse <= dashCoolDown)
            {
                return;
            }

            dashLastUse = 0;
            onCoolDown = false;
            
        }

        // We need to check the y rotation and set it to 270 or 90 so the player doesn'y
        // go off the playing area
        if (Input.GetKeyDown("left ctrl"))
        {
            int new_x = 0;
            Vector3 rot = transform.position;

            if (body.transform.rotation.y >= 0) 
            { 
                new_x = 10; 
            }
            else  
            {
                new_x = -10; 
            }

            rot.x += new_x;
            transform.LookAt(rot);

            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;

            body.velocity = new Vector3(10 * new_x, 40, 0);
            onCoolDown = true;
        } 
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

        // toggle shield on/off 
        if (Input.GetKeyDown("q"))
        {
            // alternate between on and off
            shielding ^= true;

            // reset for next lerp
            t = 0f;

        }

        // if we want to shrink and we are still bigger than our minimum size continue to shrink
        if (shielding && (ForceField.transform.localScale.x < fullSize.x))
        {
            ForceField.transform.localScale *= Mathf.Lerp(1f, 3f, t);
            t += shieldGrowSpeed * Time.deltaTime;
        }

        // if we are just about done growing or we have overgrown
        if ((Mathf.Approximately(ForceField.transform.localScale.x, fullSize.x)) || (ForceField.transform.localScale.x > fullSize.x))
        {
            ForceField.transform.localScale = fullSize;
        }

        // if we want to shrink
        if (!shielding)
        {
            // if we still have room to grow, i.e. we are smaller than our full size
            if (ForceField.transform.localScale.x > shrunkSize.x)
            {
                ForceField.transform.localScale /= Mathf.Lerp(1f, 3f, t);
                t += shieldShrinkSpeed * Time.deltaTime;
            }
            // also, if we have accidentally grown too much or we are ESSENTIALLY done growing, grow back to our usual size
            if ((Mathf.Approximately(ForceField.transform.localScale.x, shrunkSize.x)))
            {
                ForceField.transform.localScale = shrunkSize;
            }
        }
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
        if (hasInput)
        {
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement.normalized, turnSpeed * Time.deltaTime, 0f);
            // zero our y rotation, this keeps player upright/from looking at the ground
            desiredForward.y = 0f;
            m_Rotation = Quaternion.LookRotation(desiredForward);
            body.MoveRotation(m_Rotation);
            body.velocity = m_Movement;
        }
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

        // change to velocity based vector
        m_Movement = new Vector3(horizontal * speed, body.velocity.y, vertical * speed);
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
        Vector3 jump = new Vector3(body.velocity.x, jumpHeight, body.velocity.z);
        body.velocity = jump;
    }

    /* JetPack():
     * :description: implement jet pack ability; when left shift is held down apply vertical force to the character and play jet effect, when 
     *               left shift is released stop applying force and stop playing particle effect. Sets flying bool.
     * :param: n/a
     * :dependency: isGrounded: used because we should only be able to use the jet pack in the air so we need to see if we are grounded. Set at top of Update().
     *              
     * :calls: TakeDamage(): using jet pack ability reduces O2 stores and lowers player O2 level.
     * :called by: Update().
     */
    void JetPack()
    {
        // We are in the air, so use JetPack
        if (Input.GetKeyDown("left shift") && !isGrounded)
        {
            flying = true;
            anim.SetBool("IsFlying", flying);
        }
        else if (Input.GetKeyUp("left shift"))
        {
            flying = false;
            anim.SetBool("IsFlying", flying);
            JetParticles.Stop();
        }

        if (flying)
        {
            // TODO: Decide on an amount of damage to take
            FindObjectOfType<KillPlayer>().TakeDamage(0.3f);

            Vector3 fly = new Vector3(0f, jetForce, 0f);
            body.AddForce(fly, ForceMode.VelocityChange);

            //body.velocity = fly;
            JetParticles.Play();
        }
    }
}
 