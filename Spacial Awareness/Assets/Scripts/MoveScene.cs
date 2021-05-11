using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public string nextScene;

    //void OnCollisionEnter(Collision other)
    void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Player")) 
        {
            SceneManager.LoadScene(nextScene);
           
        }
    }
}
