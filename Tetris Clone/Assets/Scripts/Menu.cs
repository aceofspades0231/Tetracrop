using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Check if the game is Paused
    public bool gameIsPaused = true;

    public GameObject mainMenu;
    public GameObject pauseMenu;

    private void Start()
    {
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if(mainMenu.activeSelf != true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameIsPaused = !gameIsPaused;
                if (gameIsPaused)
                {
                    pauseMenu.SetActive(true);
                }
                else
                {
                    pauseMenu.SetActive(false);
                }
            }
        }        
    }

    public void StartGame()
    {
        gameIsPaused = !gameIsPaused;
        mainMenu.SetActive(false);
    }

    public void ResumeGame()
    {
        gameIsPaused = !gameIsPaused;
        pauseMenu.SetActive(false);
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
