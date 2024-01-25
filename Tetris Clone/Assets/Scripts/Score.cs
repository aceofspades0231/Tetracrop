using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public Piece piece;
    public Menu menu;

    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI levelText;

    void Update()
    {
        if (!menu.gameIsPaused)
        {
            scoreText.text = piece.score.ToString();
            levelText.text = piece.level.ToString();
        }
    }
}
