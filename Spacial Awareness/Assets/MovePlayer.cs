using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float speed = 15f;
    private Animator anim;
    private Rigidbody body;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        anim = gameObject.GetComponentInChildren<Animator>();
        body.centerOfMass = new Vector3(0f, 0.1f, 0f) ;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool hasInput = (horizontal != 0f || vertical != 0f);

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();
        //bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        //bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        //bool isRunning = hasHorizontalInput || hasVerticalInput;
        //anim.SetBool("IsRunning", isRunning);

        if (hasInput)
        {
            body.AddForce(new Vector3(0f, 17f, 0f));

            // Just for rudementry testing
            body.AddForce(new Vector3(0f, 1f, 0f), ForceMode.Impulse);

        }

        /* if (Keyboard.current.spaceKey.wasPressedThisFrame) {

             Vector3 movement = new Vector3(0.0f, 10.0f, 0.0f);
             rb.AddForce(movement * speed, ForceMode.Impulse);
         } */



        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
        body.MoveRotation(m_Rotation);
        body.MovePosition(transform.position + m_Movement * Time.deltaTime * speed);
        
    }


}
