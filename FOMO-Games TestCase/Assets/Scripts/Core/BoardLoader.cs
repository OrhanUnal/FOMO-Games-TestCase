using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class BoardLoader
{
    private string LevelPath;
    private float cellGap;
    private LevelData levelData;
    private GameManager gameManager;

    public BoardLoader(int levelNumber)
    {
        LevelPath = Application.dataPath + $"/LevelsJson/Level{levelNumber}.json";
        gameManager = GameManager.instance;
        cellGap = gameManager.cellGap;
    }

    public void InitializeBoard()
    {
        DeserializeData();
        SpawnCells();
        SpawnBlocks();
        gameManager.moveLimit = levelData.MoveLimit;
    }

    private void SpawnCells()
    {
        float blockSize = gameManager.blockSize;
        Transform cellParent = new GameObject("Cells").transform;

        foreach (CellData cell in levelData.CellInfo)
        {
            Vector3 position = new Vector2(
                cell.Col * blockSize,
                -cell.Row * blockSize
            );

            GameObject cellObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cellObj.transform.position = position;
            cellObj.transform.localScale = new Vector2(
                blockSize * cellGap,
                blockSize * cellGap
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

    private void DeserializeData()
    {
        var json = File.ReadAllText(LevelPath);
        levelData = JsonConvert.DeserializeObject<LevelData>(json);
        Debug.Log(levelData.MovableInfo.Count);
    }
}
