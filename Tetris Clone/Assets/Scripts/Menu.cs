using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Check if the game is Paused
    public bool gameIsPaused = true;

    public Canvas mainMenuCanvas;
    public Canvas pausedCanvas;

    private void Update()
    {
        if(mainMenuCanvas.gameObject.activeSelf != true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameIsPaused = !gameIsPaused;
                if (gameIsPaused)
                {
                    pausedCanvas.gameObject.SetActive(true);
                }
                else
                {
                    pausedCanvas.gameObject.SetActive(false);
                }
            }
        }        
    }

    public void StartGame()
    {
        gameIsPaused = !gameIsPaused;
        mainMenuCanvas.gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        gameIsPaused = !gameIsPaused;
        pausedCanvas.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
