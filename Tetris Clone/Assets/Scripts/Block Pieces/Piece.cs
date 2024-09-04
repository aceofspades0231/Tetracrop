using TMPro;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Gameboard board {  get; private set; }
    public Vector3Int position {  get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public int rotationIndex { get; private set; }

    public Menu menu;

    [Space]

    public float stepDelay = 1f;
    [Tooltip("When the piece locks in place")]
    public float lockDelay = 0.5f;

    [Tooltip("Handles the score for the whole game")]
    public int score = 0;
    // Increase level if player reach an increment score of 2500 (e.g 2500, 5000)
    private int scoreThreshold = 2500;
    private int previousScore;

    private int level = 1;

    [Tooltip("For showing the Final Score in the Game Over screen")]
    public int finalScore;
    [Tooltip("For showing the Final Level reached in the Game Over screen")]
    public int finalLevel;

    private float stepTime;
    private float lockTime;

    [Header("SFX")]
    // Plays sound effect
    [SerializeField]
    private AudioClip soundClip;
    [SerializeField]
    private AudioSource audioSource;

    private float speedMultiplier = 0.5f;
    private float currentStepDelay;

    [Header("UI Display")]
    [SerializeField]
    private TextMeshProUGUI gameLevelText;
    [SerializeField]
    private TextMeshProUGUI gameScoreText;

    [SerializeField]
    private TextMeshProUGUI finalLevelText;
    [SerializeField]
    private TextMeshProUGUI finalScoreText;
    
    private int checkOnce = 0;

    [Header("Movement Press")]
    [SerializeField]
    private float moveCooldown = 0.2f; // Adjust this value to change the movement speed
    [SerializeField]
    private float nextMoveTime = 0f;

    public void Initialized(Gameboard board, Vector3Int spawnPosition, TetrominoData data)
    {
        this.board = board;
        position = spawnPosition;
        this.data = data;
        rotationIndex = 0;
        stepTime = Time.time + stepDelay;
        lockTime = 0f;

        if (cells == null)
        {
            cells = new Vector3Int[data.cells.Length];
        }

        for(int i = 0; i < cells.Length; i++)
        {
            cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Start()
    {
        audioSource.clip = soundClip;
    }    

    public void Update()
    {
        if (!menu.gameIsPaused)
        {
            board.Clear(this);

            lockTime += Time.deltaTime;

            gameLevelText.text = level.ToString();
            gameScoreText.text = score.ToString();

            finalLevelText.text = level.ToString();
            finalScoreText.text = score.ToString();

            // Rotation of the Piece
            if (Input.GetKeyDown(KeyCode.W) || (Input.GetKeyDown(KeyCode.UpArrow)))
            {
                Rotate(1);
            }

            // To make sure the movement is not ultrafast
            if (Time.time >= nextMoveTime)
            {
                // Left and Right Piece Movement
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    Move(Vector2Int.left);
                    nextMoveTime = Time.time + moveCooldown;
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    Move(Vector2Int.right);
                    nextMoveTime = Time.time + moveCooldown;
                }

                // Soft drop
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    score += 10;    
                    Move(Vector2Int.down);
                    nextMoveTime = Time.time + moveCooldown;
                }
            }

            // Hard drop
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HardDrop();
            }

            if (score >= previousScore + scoreThreshold)
            {
                LevelIncrement();

                speedMultiplier += 0.1f;
                previousScore = score / scoreThreshold * scoreThreshold;
            }

            if (Time.time >= stepTime)
            {
                Step();
            }

            board.Set(this);

            if (board.gameOver == true)
            {
                checkOnce++;
                if(checkOnce == 1)
                {
                    finalLevel = level;
                    finalScore = score;

                    finalLevelText.text = finalLevel.ToString();
                    finalScoreText.text = finalScore.ToString();
                }
            }
        }        
    }    

    private void LevelIncrement()
    {
        level++;

        currentStepDelay = stepDelay / level;
    }

    private void Step()
    {
        Vector2Int translation = Vector2Int.down;
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        if (level == 1)
            stepTime = Time.time + stepDelay;
        else
            stepTime = Time.time + currentStepDelay * speedMultiplier;

        bool valid = board.IsValidPosition(this, newPosition);

        if (valid)
        {
            score += 10;
            position = newPosition;
            lockTime = 0f;
        }       

        if (lockTime >= lockDelay)
        {            
            Lock();
        }
    }

    private void Lock()
    {
        board.Set(this);
        board.ClearLines();
        board.SpawnPiece();
    }
    
    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;
        
        bool valid = board.IsValidPosition(this, newPosition);        

        if (valid)
        {
            audioSource.Play();
            position = newPosition;
            lockTime = 0f;
        }

        return valid;
    }

    private void HardDrop()
    {
        audioSource.Play();

        while (Move(Vector2Int.down)) 
        {
            score += 10;            
            continue;            
        }

        Lock();
    }

    // Function to activate the rotation of piece and wall kicks
    private void Rotate(int direction)
    {
        int originalRotation = rotationIndex;
        rotationIndex = Wrap(rotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);

        if (!TestWallKicks(rotationIndex, direction))
        {
            rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    // Applies the rotation to the piece
    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 cell = cells[i];

            int x, y;

            switch (data.tetromino)
            {
                // For I and O pieces
                case Tetromino.Carrot:
                case Tetromino.Potato:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                // For the rest of the pieces
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            cells[i] = new Vector3Int(x, y, 0);
        }
    }

    // Piece kicks wall, to prevent going out of bounds
    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for(int i = 0; i < data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = data.wallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
    }

    // Resets the rotationIndex to 0 or 3
    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}
