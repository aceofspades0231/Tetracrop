using UnityEngine;
using UnityEngine.Tilemaps;

public class Gameboard : MonoBehaviour
{
    public Tilemap tilemap {  get; private set; }
    public TetrominoData[] tetrominoes;
    public Piece activePiece {  get; private set; }
    public Vector3Int spawnPosition;

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for(int i = 0; i < tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    private void SpawnPiece()
    {
        int random = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        this.activePiece.Initialized(this, this.spawnPosition, data);
        Set(this.activePiece);
    }

    public void Set(Piece piece)
    {
        for(int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.spawnPosition;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
}
