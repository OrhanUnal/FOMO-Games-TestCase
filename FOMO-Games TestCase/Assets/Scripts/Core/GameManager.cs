using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int moveLimit;
    private int blockCount;
    private bool hasMoveLimit;


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
    private void OnEnable()
    {
        BlockBase.OnBlockMoved += DecrementMoveLimit;
        BlockBase.OnBlockCountChanged += ChangeBlockCount;
    }
   
    private void OnDisable()
    {
        BlockBase.OnBlockMoved -= DecrementMoveLimit;
        BlockBase.OnBlockCountChanged -= ChangeBlockCount;
    }

    private void DecrementMoveLimit()
    {
        if (!hasMoveLimit) return;

        moveLimit -= 1;
        if (moveLimit <= 0)
        {
            //fail code
        }
    }

    private void ChangeBlockCount(int amount)
    {
        blockCount += amount;
        if (blockCount <= 0)
        {
            //win code
        }
    }

    public void SetLevelData(int moveLimit)
    {
        this.moveLimit = moveLimit;
        hasMoveLimit = moveLimit > 0;
    }
}
