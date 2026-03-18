using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BoardLoader : MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField] private float boardWidth = 6f;
    [SerializeField] private float boardHeight = 8f;
    [SerializeField] private float cellGap = 0.95f;
    [SerializeField] private int levelNumber = 1;

    [Header("Prefab Registries")]
    [SerializeField] private BlockShapeSO blockShapeSO;
    [SerializeField] private BlockShapeSO exitShapeSO;

    private float blockGap;
    private float blockSize;
    private Vector3 centerOfBoard;
    private LevelData levelData;
    private MaterialFactory materialFactory;
    private Transform cellParent;
    private Transform blockParent;
    private Transform exitParent;


    public void Start()
    {
        materialFactory = new MaterialFactory();
        blockGap = cellGap * 0.9f;
        InitializeBoard();
    }
    private void OnEnable()
    {
        UIManager.OnRetryClicked += RetryLevel;
        UIManager.OnNextLevelClicked += NextLevel;
    }

    private void OnDisable()
    {
        UIManager.OnRetryClicked -= RetryLevel;
        UIManager.OnNextLevelClicked -= NextLevel;
    }

    private void InitializeBoard()
    {
        DeserializeData();
        CalculateBlockSize();
        CalculateMiddleOfTheBoard();
        SpawnCells();
        SpawnBlocks();
        SpawnExits();
        GameManager.instance.SetLevelData(levelData.MoveLimit, levelNumber);
    }
    private void DeserializeData()
    {
        string levelPath = Application.dataPath + $"/LevelsJson/Level{levelNumber}.json";
        var json = File.ReadAllText(levelPath);
        levelData = JsonConvert.DeserializeObject<LevelData>(json);
    }

    private void CalculateBlockSize()
    {
        float sizeFromWidth = boardWidth / levelData.ColCount;
        float sizeFromHeight = boardHeight / levelData.RowCount;
        blockSize = Mathf.Min(sizeFromWidth, sizeFromHeight);
    }

    private void CalculateMiddleOfTheBoard()
    {
        centerOfBoard = new Vector3(
            (levelData.ColCount - 1) * 0.5f,
            -(levelData.RowCount - 1) * 0.5f, 
            0) * blockSize;
    }

    private void SpawnCells()
    {
        cellParent = new GameObject("Cells").transform;

        foreach (CellData cell in levelData.CellInfo)
        {
            Vector3 position = new Vector3(
                cell.Col * blockSize,
                -cell.Row * blockSize,
                10
            ) - centerOfBoard;

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
        blockParent = new GameObject("Blocks").transform;

        foreach (MovableData blocksData in levelData.MovableInfo)
        {

            GameObject prefab = blockShapeSO.GetPrefab(blocksData.Length);
            Vector3 position = new Vector3(
                blocksData.Col * blockSize,
                -blocksData.Row * blockSize,
                0
            ) - centerOfBoard;


            bool isVertical = blocksData.Direction.Contains(0) || blocksData.Direction.Contains(2);
            Vector3 rotationOfBlock = isVertical ? new Vector3(0, 0, -90) : Vector3.zero;
            GameObject blockObj = Instantiate(prefab, position, Quaternion.Euler(rotationOfBlock), blockParent);
            blockObj.transform.localScale = Vector3.one * blockSize * blockGap;
            blockObj.name = $"Block_{blocksData.Row}_{blocksData.Col}";


            Material mat = materialFactory.GetMaterial(blocksData.Colors, blocksData.Length, isVertical);
            Renderer renderer = blockObj.GetComponentInChildren<Renderer>();
            if (renderer != null && mat != null)
            {
                renderer.material = mat;
            }

            BlockBase block = blockObj.GetComponent<BlockBase>();
            block.Initialize(blocksData.Colors, blocksData.Direction, blocksData.Row, blocksData.Col, blockSize);
        }
    }

    private void SpawnExits()
    {
        exitParent = new GameObject("Exits").transform;

        foreach (ExitData exitsData in levelData.ExitInfo)
        {
            GameObject prefab = exitShapeSO.GetPrefab(exitsData.Direction);
            Vector3 position = new Vector3(
                exitsData.Col * blockSize,
                -exitsData.Row * blockSize,
                0
            ) - centerOfBoard;

            GameObject exitObj = Object.Instantiate(prefab, position, Quaternion.identity, exitParent);
            exitObj.transform.localScale = Vector3.one * cellGap * blockSize;
            exitObj.name = $"Exit_{exitsData.Row}_{exitsData.Col}";

            Material mat = materialFactory.GetMaterial(exitsData.Colors);
            Renderer renderer = exitObj.GetComponentInChildren<Renderer>();
            if (renderer != null && mat != null)
            {
                renderer.material = mat;
            }

            ExitGates exit = exitObj.GetComponent<ExitGates>();
            exit.Initialize(exitsData.Colors);
        }
    }
    private void RetryLevel()
    {
        ClearBoard();
        InitializeBoard();
    }

    private void NextLevel()
    {
        if (levelNumber == 4)
        {
            Debug.Log("Elimde baska level yok");
            return;
        }

        levelNumber++;
        ClearBoard();
        InitializeBoard();
    }

    private void ClearBoard()
    {
        if (cellParent) Destroy(cellParent.gameObject);
        if (blockParent) Destroy(blockParent.gameObject);
        if (exitParent) Destroy(exitParent.gameObject);
    }
}