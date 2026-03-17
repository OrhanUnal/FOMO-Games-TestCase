using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class BoardLoader
{
    private string LevelPath;
    private float cellGap;
    private float blockGap;
    private float blockSize;
    private float boardWidth;
    private float boardHeight;
    private BlockShapeSO blockShapeSO;
    private BlockColorSO colorSO;

    private LevelData levelData;
    private GameManager gameManager;

    public float BlockSize => blockSize;
    public int MoveLimit => levelData.MoveLimit;

    public BoardLoader(int levelNumber, float boardWidth, float boardHeight, BlockShapeSO blockShapeSO, BlockColorSO colorSO)
    {
        LevelPath = Application.dataPath + $"/LevelsJson/Level{levelNumber}.json";
        gameManager = GameManager.instance;
        cellGap = gameManager.cellGap;
        blockGap = cellGap * 0.9f;
        this.blockShapeSO = blockShapeSO;
        this.colorSO = colorSO;
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
                10
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
        Transform blockParent = new GameObject("Blocks").transform;

        foreach (MovableData data in levelData.MovableInfo)
        {
            //boyunu ve cinsini ayarla
            GameObject prefab = blockShapeSO.GetBlockPrefab(data.Length);
            Vector3 position = new Vector3(
                data.Col * blockSize,
                -data.Row * blockSize,
                0
            );

            GameObject blockObj = Object.Instantiate(prefab, position, Quaternion.identity, blockParent);
            blockObj.transform.localScale = Vector3.one * blockSize * blockGap;
            blockObj.name = $"Block_{data.Row}_{data.Col}";

            // Rengini ve yonunu yap
            bool isVertical = data.Direction.Contains(0) || data.Direction.Contains(2);
            Material mat = colorSO.GetMaterial(data.Colors, data.Length, isVertical);
            Renderer renderer = blockObj.GetComponentInChildren<Renderer>();
            if (renderer != null && mat != null)
            {
                renderer.material = mat;
            }

            //spawn et
            BlockBase block = blockObj.GetComponent<BlockBase>();
            block.Initialize(data.Colors, data.Direction, data.Row, data.Col);
        }
    }
}
