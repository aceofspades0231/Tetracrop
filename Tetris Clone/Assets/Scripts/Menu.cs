using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Piece piece;

    // Check if the game is Paused
    public bool gameIsPaused = true;

    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the boolean variable
            gameIsPaused = !gameIsPaused;
        }
    }

    public void StartGame()
    {
        gameIsPaused = !gameIsPaused;
        canvas.gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
