using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false; // This is so you cannot die more than once
    public float restartDelay = 3f;
    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("GAME OVER");
            // Now restart the game and reset gameHasEnded. With a time delay
            // Fixed lighting issue by going to window ==> rendering ==> lighting ==> generate lighting
            // TODO during the delay show Game Over Screne!
            Invoke("Restart", restartDelay);
            
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
