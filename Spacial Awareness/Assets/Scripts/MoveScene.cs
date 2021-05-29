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
        _flashImage.StartFlash(.25f, 1f, Color.white);
        if (other.gameObject.CompareTag("Player")) 
        {
            // Save high score if needed
            HighScore score_script = GameObject.Find("Canvas/Score_prefab").GetComponent<HighScore>();

           
            // Check we are in a level; ie not in the menu or ending scene
            bool in_level = score_script.highScoreKey == "L1" || score_script.highScoreKey == "L2" ||
                            score_script.highScoreKey == "L3";

            if (in_level)
            {
                Debug.Log("123");
                score_script.setHighScore();
            }


            Invoke("NextLevel", 1.0f);  
        }
    }


    void NextLevel()
    {
        SceneManager.LoadScene(nextScene);
    }
}


