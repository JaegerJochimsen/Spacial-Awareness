using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public string nextScene;
    private bool isGrounded = false;
    public Transform groundCheck;
    float groundDistance = 0.8f;
    public LayerMask groundMask;


    void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.CompareTag("Player")) 
        {
            Debug.Log("Here2");
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            //if (isGrounded)
            //{
                Debug.Log("Here");
                SceneManager.LoadScene(nextScene);
            //}
        }
    }
}
