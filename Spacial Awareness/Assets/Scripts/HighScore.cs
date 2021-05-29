using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScore : MonoBehaviour
{

    float highScore = 0;
    public string highScoreKey;
    float score = 0;

    Text myText;

    // Start is called before the first frame update
    void Start()
    {

        myText = GameObject.Find("Canvas/Score").GetComponent<Text>();
        string scene = SceneManager.GetActiveScene().name;

        if (scene == "Level 1")
        {
            highScoreKey = "L1";
            highScore = PlayerPrefs.GetFloat(highScoreKey, 0);
        }
        else if (scene == "Level 2")
        {
            highScoreKey = "L2";
            highScore = PlayerPrefs.GetFloat(highScoreKey, 0);
        }
        else if (scene == "Level 3")
        {
            highScoreKey = "L3";
            highScore = PlayerPrefs.GetFloat(highScoreKey, 0);
        }
    }

    void Update()
    {
        score += Time.deltaTime;
        string formated_output = string.Format("Current: {0:#####.##}\nFastest: {1:#####.##}", score,  highScore);
        myText.text = formated_output;
    }

    public void setHighScore()
    {
        if (score < highScore)
        {
            Debug.Log("here");
            score = Mathf.Round(score * 100.0f) / 100.0f;
            PlayerPrefs.SetFloat(highScoreKey, score);
            PlayerPrefs.Save();
        }
    } 
}
