using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [Header("Movement Variables")]  // used for headers in Unity editor
    // Movement Variables
    float turnSpeed = 40f;
    float speed = 15f;
    float jumpHeight = 50f;
    public float lavaKnock;
    public float lavaDamage;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    bool hasInput = false;
    public bool doubleJumped = false;
    // End movement variables

    [Header("Animation Variables")]
    // Animation variables
    private Animator anim;
    bool IsRunning = false;
    // End animation variables

    [Header("Groundcheck/Lavacheck Variables")]
    // Ground-check variables
    public Transform groundCheck;
    float groundDistance = 0.5f;
    public LayerMask groundMask;
    public LayerMask lavaMask;
    bool isGrounded = true;
    public bool isOnLava = false;
    // End ground check variables

    [Header("Dash Variables")]
    // Dash vars
    public float dashSpeed;
    public float dashDelay;
    bool dashing = false;
    bool hasDashed = false;
    // End Dash vars

    [Header("JetPack Variables")]
    // JetPack variables
    public ParticleSystem JetParticles;
    float jetForce = 2f;
    public bool flying = false;
    // End JetPack variables

    // Shield health bar variables
    public HealthBar shieldBar;
    int shieldHits = 0;

    [Header("Shield Variables")]
    // Shield variables
    public GameObject ForceField;
    public bool shielding = false;
    public float shieldCharge = 3f;
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

        // Jetpack particle system
        JetParticles = GetComponentInChildren<ParticleSystem>();

        shieldCharge = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        // check to see if our player is on the ground or a ground equivalent
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        anim.SetBool("IsGrounded", isGrounded);

        // check to see if player stepped on lava
        isOnLava = Physics.CheckSphere(groundCheck.position, groundDistance, lavaMask);

        // if we aren't dashing then move as usual, otherwise we need to wait until we finish dashing to move again
        if (!dashing)
        {
            // handle input from keyboard, put into Vector3 for MoveAndLook() later, and apply inAirPenalty 
            HandleMovementInput();

            // if on lava, then jump and take damage
            HandleLavaCheck();

            // check to see if we should play the animation, then set the bool
            SetRunningAnimBool();

            DoubleJump();

            Shield();
            // apply player movement and look rotation to character model
            MoveAndLook();

            // handle logic for using jetpack and apply force + play particle effects
            JetPack();
            // jump if press space and we have something to jump from; if the time since we left the ground is less than 
            // the coyoteTime threshold, then go ahead and jump and update prevGroundTime
            if (Input.GetKeyDown("space") && isGrounded)
            {
                Jump();
            }
        }

        Dash();

    }

    void HandleLavaCheck()
    {
        if (isOnLava)
        {
            // TakeDamage() handles the particulars of damage taking/negation due to shielding
            GetComponent<KillPlayer>().TakeDamage(lavaDamage);

            // knock player into the air when they touch the lava
            Vector3 knockUp = new Vector3(0f, lavaKnock, 0f);
            body.AddForce(knockUp, ForceMode.Impulse);
        }
    }

    /* DashWait(float waitTime):
     * :description: add force to player body and then wait for the dash to finish. After the dash has finished zero out body velocity 
     *               and set bool to say we are no longer dashing.
     * :param: float waitTime: the duration we should wait before returning control to main loop/accepting new player input.
     * :dependency: must be invoked by using StartCoroutine(DashWait(dashDelay));
     * :side effects: sets dashing bool, affects player velocity
     * 
     * :calls: n/a
     * :called by: Dash()
     */
    IEnumerator DashWait(float waitTime)
    {
        // apply force to body
        body.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
        // wait for dash to complete
        yield return new WaitForSeconds(waitTime);
        // zero out body velocity so we kind of fall after dash
        body.velocity = Vector3.zero;
        dashing = false;
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
     * :description: Dash forward and slightly up; activates with same conditions as double jumped (i.e. has not already dashed
     *               and is off the ground)
     * :param: n/a
     * :dependency: n/a
     * 
     * :calls: n/a
     * :called by: Update().
     */

    void Dash()
    {

        // We need to check the y rotation and set it to 270 or 90 so the player doesn'y
        // go off the playing area
        if (Input.GetKeyDown("k") && !isGrounded && !hasDashed)
        {
            Vector3 rot = transform.position;

            // access euler angle representation of rotation (0 - 360 degrees)
            if ((body.transform.eulerAngles.y >= 0f) && (body.transform.eulerAngles.y <= 180f)) 
            {
                transform.rotation = Quaternion.Euler(body.rotation.x, 90f, body.rotation.z);
            }
            else  
            {
                transform.rotation = Quaternion.Euler(body.rotation.x, -90f, body.rotation.z);
            }
            
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;

            // start coroutine that will enact the dash and then delay until dash completes
            StartCoroutine(DashWait(dashDelay));
            hasDashed = true;
            dashing = true;
        }
        // reset once we hit the ground
        if (isGrounded) { hasDashed = false; }
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

        // toggle shield on/off if we have charge left
        if (Input.GetKeyDown("q") && (shieldCharge > 0f))
        {
            // alternate between on and off
            shielding ^= true;

            // reset for next lerp
            t = 0f;

        }
        // if we are out of shield charges, then we cannot shield
        if(shieldCharge <= 0f && shielding) { shielding = false; t = 0f; }

        // if we want to grow and we are still smaller than our max size continue to grow
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

        // if we want to shrink or we have run out of shield charges
        if (!shielding)
        {
            // if we still have room to shrink, i.e. we are bigger than our small size
            if (ForceField.transform.localScale.x > shrunkSize.x)
            {
                ForceField.transform.localScale /= Mathf.Lerp(1f, 3f, t);
                t += shieldShrinkSpeed * Time.deltaTime;
            }
            // also, if we have accidentally shrunk too much or we are ESSENTIALLY done shrinking, shrink back to our shrunk size
            if ((Mathf.Approximately(ForceField.transform.localScale.x, shrunkSize.x)))
            {
                ForceField.transform.localScale = shrunkSize;
            }
        }
    }

    /* ReduceShieldCharge():
     * :description: and auxillary function to reduce the charge on the shield when the player takes damage (or would have if they weren't shielding)
     * 
     * :param: int reduction: a reduction factor
     * :dependency: n/a
     * 
     * :calls: n/a
     * :called by: KillPlayer.TakeDamage()
     */

    public void ReduceShieldCharge(int reduction)
    {
        shieldCharge -= reduction*Time.deltaTime*50;
        shieldBar.SetHealth(shieldCharge);
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

        //body.velocity = Vector3.zero;
        //body.angularVelocity = Vector3.zero;
        // Attempt to fix dash issue

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
        if (Input.GetKeyDown("j") && !isGrounded)
        {
            flying = true;
            anim.SetBool("IsFlying", flying);
        }
        else if (Input.GetKeyUp("j"))
        {
            flying = false;
            anim.SetBool("IsFlying", flying);
            JetParticles.Stop();
        }

        if (flying)
        {
            // TODO: Decide on an amount of damage to take
            FindObjectOfType<KillPlayer>().TakeDamage(0.4f);

            Vector3 fly = new Vector3(0f, jetForce, 0f);
            body.AddForce(fly, ForceMode.VelocityChange);

            //body.velocity = fly;
            JetParticles.Play();
        }
    }
}
 