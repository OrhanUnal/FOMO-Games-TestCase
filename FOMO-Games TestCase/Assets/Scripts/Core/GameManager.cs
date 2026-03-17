using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int moveLimit;
    public float BlockSize { get; private set; }

    [Header("Level Setting")]
    [SerializeField] int levelNumber = 1;
    [SerializeField] public float cellGap = 0.95f;
    [SerializeField] private float boardWidth = 6f;
    [SerializeField] private float boardHeight = 8f;
    [SerializeField] private BlockShapeSO blockShapeSO;
    [SerializeField] private BlockShapeSO exitsSO;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        BoardLoader loader = new BoardLoader(levelNumber, boardWidth, boardHeight, blockShapeSO, exitsSO);
        loader.InitializeBoard();
        moveLimit = loader.MoveLimit;
        BlockSize = loader.blockSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecrementMoveLimit()
    {
        moveLimit -= 1;
    }
}
