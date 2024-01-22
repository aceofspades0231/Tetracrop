using UnityEngine;

public class Piece : MonoBehaviour
{
    public Gameboard board {  get; private set; }
    public Vector3Int spawnPosition {  get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }

    public void Initialized(Gameboard board, Vector3Int spawnPosition, TetrominoData data)
    {
        this.board = board;
        this.spawnPosition = spawnPosition;
        this.data = data;

        if(this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for(int i = 0; i < this.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }
}
