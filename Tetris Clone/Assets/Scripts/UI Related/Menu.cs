using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField]
    private GameObject offensiveWordsDetector;
    private string[] offensiveWords = { "sex", "ass", "dik", "dic", "fuk", "fuc" };

    [SerializeField]
    private Button gameOverRestartButton;
    [SerializeField]
    private Button gameOverExitButton;

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
                PauseGame();
            }
        }

        if (gameIsPaused)
        {
            CheckForOffensiveWord(nameInput.text);
        }
    }    

    public void StartGame()
    {
        gameIsPaused = !gameIsPaused;
        mainMenu.SetActive(false);
        highscoreDisplay.anchoredPosition = new Vector2(-1450, 0);
    }

    public void PauseGame()
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
        string name;

        if (nameInput.text != "")
            name = nameInput.text;
        else
            name = "DEF";

        highscoreTable.AddHighscoreEntry(piece.finalLevel, piece.finalScore, name);
        SceneManager.LoadScene(0);
    }

    public void SimpleExit()
    {
        Application.Quit();
    }

    public void ExitGame()
    {
        string name;

        if (nameInput.text != "")
            name = nameInput.text;
        else
            name = "DEF";

        highscoreTable.AddHighscoreEntry(piece.finalLevel, piece.finalScore, name);
        Application.Quit();
    }

    private void CheckForOffensiveWord(string input)
    {
        bool offensiveWordDetected = false;

        foreach (string word in offensiveWords)
        {
            if (input.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Debug.Log("Offensive word detected: " + word);
                offensiveWordDetected = true;
                break; // Exit the loop if an offensive word is detected
            }
        }

        if (offensiveWordDetected)
        {
            offensiveWordsDetector.SetActive(true);

            gameOverRestartButton.interactable = false;
            gameOverExitButton.interactable = false;

            gameOverRestartButton.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.25f);
            gameOverExitButton.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.25f);
        }
        else
        {
            offensiveWordsDetector.SetActive(false);

            gameOverRestartButton.interactable = true;
            gameOverExitButton.interactable = true;

            gameOverRestartButton.GetComponentInChildren<Image>().color = Color.white;
            gameOverExitButton.GetComponentInChildren<Image>().color = Color.white;
        }
    }
}
