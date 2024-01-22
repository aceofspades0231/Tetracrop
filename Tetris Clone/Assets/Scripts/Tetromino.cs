using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
    Carrot, // I Shape
    Eggplant, // J Shape
    Tomato, // L Shape
    Potato, // O Shape
    Mushroom, // S Shape
    Broccoli, // T Shape
    Corn, // Z Shape
}

[System.Serializable]
public struct TetrominoData
{
    public Tetromino tetromino;
    public Tile tile;
    public Vector2Int[] cells { get; private set; }

    public void Initialize()
    {
        this.cells = Data.Cells[this.tetromino];
    }
}