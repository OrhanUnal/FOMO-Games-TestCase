using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BoardLoader
{
    private string LevelPath;
    private float cellGap;
    private float blockGap;
    private float boardWidth;
    private float boardHeight;
    private BlockShapeSO blockShapeSO;
    private BlockShapeSO exitShapeSO;

    private LevelData levelData;
    private GameManager gameManager;

    private Dictionary<string, Material> materialCache = new Dictionary<string, Material>();

    public int MoveLimit => levelData.MoveLimit;
    public float blockSize;

    public BoardLoader(int levelNumber, float boardWidth, float boardHeight, BlockShapeSO blockShapeSO, BlockShapeSO exitsSO)
    {
        LevelPath = Application.dataPath + $"/LevelsJson/Level{levelNumber}.json";
        gameManager = GameManager.instance;
        cellGap = gameManager.cellGap;
        blockGap = cellGap * 0.9f;
        this.blockShapeSO = blockShapeSO;
        this.boardWidth = boardWidth;
        this.boardHeight = boardHeight;
        this.exitShapeSO = exitsSO;
    }

    public void InitializeBoard()
    {
        DeserializeData();
        CalculateBlockSize();
        SpawnCells();
        SpawnBlocks();
        SpawnExits();
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

        foreach (MovableData blocksData in levelData.MovableInfo)
        {
            //boyunu ve cinsini ayarla
            GameObject prefab = blockShapeSO.GetPrefab(blocksData.Length);
            Vector3 position = new Vector3(
                blocksData.Col * blockSize,
                -blocksData.Row * blockSize,
                0
            );

            //oyuna koy
            bool isVertical = blocksData.Direction.Contains(0) || blocksData.Direction.Contains(2);
            Vector3 rotationOfBlock = isVertical ? new Vector3(0, 0, -90) : Vector3.zero;
            GameObject blockObj = Object.Instantiate(prefab, position, Quaternion.Euler(rotationOfBlock), blockParent);
            blockObj.transform.localScale = Vector3.one * blockSize * blockGap;
            blockObj.name = $"Block_{blocksData.Row}_{blocksData.Col}";

            // Rengini ve yonunu yap
            Material mat = GetMaterial(blocksData.Colors, blocksData.Length, isVertical);
            Renderer renderer = blockObj.GetComponentInChildren<Renderer>();
            if (renderer != null && mat != null)
            {
                renderer.material = mat;
            }

            //init et
            BlockBase block = blockObj.GetComponent<BlockBase>();
            block.Initialize(blocksData.Colors, blocksData.Direction, blocksData.Row, blocksData.Col, blockSize);
        }
    }

    private void SpawnExits()
    {
        Transform exitParent = new GameObject("Exits").transform;

        foreach (ExitData exitsData in levelData.ExitInfo)
        {
            //boyunu ve cinsini ayarla
            GameObject prefab = exitShapeSO.GetPrefab(exitsData.Direction);
            Vector3 position = new Vector3(
                exitsData.Col * blockSize,
                -exitsData.Row * blockSize,
                0
            );

            //oyuna koy
            GameObject exitObj = Object.Instantiate(prefab, position, Quaternion.identity, exitParent);
            exitObj.transform.localScale = Vector3.one * cellGap * blockSize;
            exitObj.name = $"Exit_{exitsData.Row}_{exitsData.Col}";

            // Rengini ayarla
            Material mat = GetMaterial(exitsData.Colors);
            Renderer renderer = exitObj.GetComponentInChildren<Renderer>();
            if (renderer != null && mat != null)
            {
                renderer.material = mat;
            }

            //init et
            ExitGates exit = exitObj.GetComponent<ExitGates>();
            exit.Initialize(exitsData.Colors);
        }
    }

    private Material GetMaterial(int colorId, int cubeType = 1, bool isVertical = true)
    {
        string colorName = ((Enums.BlockColor)colorId).ToString();
        if (colorName == null) return null;

        string orientation = isVertical ? "Up" : "Paralel";
        string textureName = $"Cube{cubeType:D2}{colorName}{orientation}TextureMap";
        string texturePath = $"Textures/Cube{cubeType}/{textureName}";

        if (materialCache.ContainsKey(textureName))
            return materialCache[textureName];

        Texture2D texture = Resources.Load<Texture2D>(texturePath);
        if (texture == null)
        {
            Debug.LogWarning($"Texture not found at: {texturePath}");
            return null;
        }

        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.SetTexture("_BaseMap", texture);
        mat.name = textureName;

        materialCache[textureName] = mat;
        return mat;
    }
}
