using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Gameboard : MonoBehaviour
{
    public Tilemap tilemap {  get; private set; }
    public TetrominoData[] tetrominoes;
    public Piece activePiece {  get; private set; }

    public Menu menu;

    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);

    [Header("SFX")]
    // Plays sound effect
    public AudioClip soundClip;
    public AudioSource audioSource;

    [Header("Game Over check")]
    public bool gameOver = false;

    [SerializeField] private RectTransform gameOverMenu;

    [Tooltip("For the movement of the Game Over screen")]
    [SerializeField] private float moveDuration = 0.25f;
    private Vector2 targetPosition = Vector2.zero;

    public RectInt Bounds { 
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        } 
    }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();

        for(int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
        audioSource.clip = soundClip;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            HoldPiece();
        }
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];

        activePiece.Initialized(this, spawnPosition, data);
        
        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            // If Pieces reach the spawnPosition the game will be over
            GameOver();
        }
    }

    private void HoldPiece()
    {
        // Will Update in the Future
    }

    private void GameOver()
    {
        menu.gameIsPaused = true;
        gameOver = true;

        menu.nameInput.text = "";

        StartCoroutine(MoveLoseMenu(targetPosition));
    }

    // Sets and show Piece onto the Tilemap
    public void Set(Piece piece)
    {
        for(int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    // Binds the movement of the Pieces inside the Gameboard
    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        for(int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            if(tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        while (row < bounds.yMax)
        {
            if (isLineFull(row))
            {
                audioSource.Play();
                activePiece.score += 100;
                LineClear(row);
            }
            else
            {
                row++;
            }
        }        
    }

    // Checks if the row is full and can be cleared
    private bool isLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if(!tilemap.HasTile(position)) 
            {
                return false;
            }
        }

        return true;
    }

    private void LineClear(int row)
    {
        RectInt bounds = Bounds;

        // Clears whole line
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);            
            tilemap.SetTile(position, null);
        }

        // Make the above rows fall down
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    }

    IEnumerator MoveLoseMenu(Vector2 targetPosition)
    {
        Vector2 startingPosition = gameOverMenu.anchoredPosition;

        for (float elapsedTime = 0f; elapsedTime < moveDuration; elapsedTime += Time.deltaTime)
        {
            float t = elapsedTime / moveDuration;
            gameOverMenu.anchoredPosition = Vector2.Lerp(startingPosition, targetPosition, t);
            yield return null;
        }

        gameOverMenu.anchoredPosition = targetPosition;
    }
}
