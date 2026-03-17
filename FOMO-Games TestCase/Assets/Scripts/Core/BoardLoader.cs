using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class BoardLoader
{
    private string LevelPath;
    private float cellGap;
    private float blockSize;
    private float boardWidth;
    private float boardHeight;
    
    private LevelData levelData;
    private GameManager gameManager;

    public float BlockSize => blockSize;
    public int MoveLimit => levelData.MoveLimit;

    public BoardLoader(int levelNumber, float boardWidth, float boardHeight)
    {
        LevelPath = Application.dataPath + $"/LevelsJson/Level{levelNumber}.json";
        gameManager = GameManager.instance;
        cellGap = gameManager.cellGap;
        this.boardWidth = boardWidth;
        this.boardHeight = boardHeight;
    }

    public void InitializeBoard()
    {
        DeserializeData();
        CalculateBlockSize();
        SpawnCells();
        SpawnBlocks();
    }

    private void DeserializeData()
    {
        var json = File.ReadAllText(LevelPath);
        levelData = JsonConvert.DeserializeObject<LevelData>(json);
    }

    public void CalculateBlockSize()
    {
        float sizeFromWidth = boardWidth / levelData.ColCount;
        float sizeFromHeight = boardHeight / levelData.RowCount;
        blockSize = Mathf.Min(sizeFromWidth, sizeFromHeight);
    }

    private void SpawnCells()
    {
        Transform cellParent = new GameObject("Cells").transform;

        foreach (CellData cell in levelData.CellInfo)
        {
            Vector3 position = new Vector3(
                cell.Col * blockSize,
                -cell.Row * blockSize,
                0
            );

            GameObject cellObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cellObj.transform.position = position;
            cellObj.transform.localScale = new Vector3(
                blockSize * cellGap,
                blockSize * cellGap,
                blockSize
            );
            cellObj.transform.SetParent(cellParent);
            cellObj.name = $"Cell_{cell.Row}_{cell.Col}";
        }
    }

    private void SpawnBlocks()
    {
        foreach(MovableData block in levelData.MovableInfo)
        {
            Debug.Log($"Column is{block.Col} and row is {block.Row} ");
        }
    }
}
