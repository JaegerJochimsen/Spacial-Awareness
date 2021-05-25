using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadingScreen", 5.0f);
    }

    void LoadingScreen()
    {
        SceneManager.LoadScene("Menu");
    }
}
