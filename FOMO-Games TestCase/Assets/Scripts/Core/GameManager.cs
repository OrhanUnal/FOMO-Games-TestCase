using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int moveLimit { get; set; }
    public int blockSize { get; private set; }

    [Header("Level Setting")]
    [SerializeField] int levelNumber = 1;
    [SerializeField] public float cellGap = 0.95f;



    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        blockSize = 2;
    }

    void Start()
    {
        BoardLoader loader = new BoardLoader(levelNumber);
        loader.InitializeBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
