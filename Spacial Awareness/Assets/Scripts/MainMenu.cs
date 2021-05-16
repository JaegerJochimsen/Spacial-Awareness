using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame ()
    {
        Debug.Log("PLAY");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2); 
        // +2 becasue when building make the 3rd item in the build queue level 1
    }

    public void Levels()
    {
        Debug.Log("LEVELS");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame ()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
