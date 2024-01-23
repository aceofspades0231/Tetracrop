using UnityEngine;
using UnityEngine.Audio;

public class Piece : MonoBehaviour
{
    public Gameboard board {  get; private set; }
    public Vector3Int position {  get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public int rotationIndex { get; private set; }

    public Menu menu;

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;

    // Plays sound effect
    public AudioClip soundClip;
    public AudioSource audioSource;

    public void Initialized(Gameboard board, Vector3Int spawnPosition, TetrominoData data)
    {
        this.board = board;
        this.position = spawnPosition;
        this.data = data;
        this.rotationIndex = 0;
        stepTime = Time.time + this.stepDelay;
        lockTime = 0f;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for(int i = 0; i < this.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Start()
    {
        audioSource.clip = soundClip;
    }

    public void Update()
    {
        this.board.Clear(this);

        this.lockTime += Time.deltaTime;

        if (!menu.gameIsPaused)
        {
            // Rotation of the Piece
            if (Input.GetKeyDown(KeyCode.W) || (Input.GetKeyDown(KeyCode.UpArrow)))
            {
                Rotate(1);
            }

            // Left and Right Piece Movement
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                audioSource.Play();
                Move(Vector2Int.left);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                audioSource.Play();
                Move(Vector2Int.right);
            }

            // Soft drop
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                audioSource.Play();
                Move(Vector2Int.down);
            }

            // Hard drop
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HardDrop();
            }

            if (Time.time >= this.stepTime)
            {
                Step();
            }

            this.board.Set(this);
        }        
    }

    private void Step()
    {
        this.stepTime = Time.time + stepDelay;

        Move(Vector2Int.down);

        if (this.lockTime >= this.lockDelay)
        {
            Lock();
        }
    }

    private void Lock()
    {
        this.board.Set(this);
        this.board.ClearLines();
        this.board.SpawnPiece();
    }
    
    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition);        

        if (valid)
        {
            this.position = newPosition;
            this.lockTime = 0f;
        }

        return valid;
    }

    private void HardDrop()
    {
        audioSource.Play();

        while (Move(Vector2Int.down)) 
        {
            continue;
        }

        Lock();
    }

    // Function to activate the rotation of piece and wall kicks
    private void Rotate(int direction)
    {
        int originalRotation = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.rotationIndex, direction))
        {
            this.rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    // Applies the rotation to the piece
    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];

            int x, y;

            switch (this.data.tetromino)
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

            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    // Piece kicks wall, to prevent going out of bounds
    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for(int i = 0; i < this.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];

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

        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
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
