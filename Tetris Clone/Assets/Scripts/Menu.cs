using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Check if the game is Paused
    public bool gameIsPaused = true;

    public TMP_InputField nameInput;

    public RectTransform highscoreDisplay;
    public GameObject mainMenu;
    public GameObject pauseMenu;

    [SerializeField]
    private HighscoreTable highscoreTable;
    [SerializeField]
    private Piece piece;

    private void Awake()
    {
        nameInput.characterLimit = 3;
    }

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
        highscoreDisplay.anchoredPosition = new Vector2(1920, 0);
    }

    public void ResumeGame()
    {
        gameIsPaused = !gameIsPaused;
        pauseMenu.SetActive(false);
    }

    public void SimpleRestart()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        if(nameInput.text != null)
        {
            string name = nameInput.text;
            highscoreTable.AddHighscoreEntry(piece.finalLevel, piece.finalScore, name);

            SceneManager.LoadScene(0);
        }        
    }

    public void SimpleExit()
    {
        Application.Quit();
    }

    public void ExitGame()
    {
        if (nameInput.text != null)
        {
            string name = nameInput.text;

            highscoreTable.AddHighscoreEntry(piece.finalLevel, piece.finalScore, name);
            Debug.Log("Final Score: " + piece.finalScore + " Final Level:" + piece.finalLevel + " Text: " + nameInput.text);

            Application.Quit();
        }            
    }
}
