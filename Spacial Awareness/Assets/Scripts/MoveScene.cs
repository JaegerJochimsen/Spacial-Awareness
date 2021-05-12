using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    [SerializeField] FlashImage _flashImage = null;
    public string nextScene;

    //void OnCollisionEnter(Collision other)
    void OnTriggerEnter(Collider other)
    {
        _flashImage.StartFlash(.25f, 1f, Color.green);
        if (other.gameObject.CompareTag("Player")) 
        {
            Invoke("NextLevel", 1.0f);  
        }
    }

    void NextLevel()
    {
        SceneManager.LoadScene(nextScene);
    }
}
