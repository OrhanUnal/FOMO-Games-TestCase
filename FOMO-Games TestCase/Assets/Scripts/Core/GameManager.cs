using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public static event Action<bool> OnLevelFinished;
    public static event Action<int, int> UpdateCounters;

    private int blockCount;
    private bool hasMoveLimit;
    private int moveLimit;
    private int currentLevelNumber;

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
        UpdateCounters?.Invoke(moveLimit, currentLevelNumber);
        if (moveLimit <= 0)
            OnLevelFinished?.Invoke(false);
    }

    private void ChangeBlockCount(int amount)
    {
        blockCount += amount;
        if (blockCount <= 0)
            OnLevelFinished?.Invoke(true);
    }

    public void SetLevelData(int moveLimit, int currentLevelNumber)
    {
        this.moveLimit = moveLimit;
        this.currentLevelNumber = currentLevelNumber;
        hasMoveLimit = moveLimit > 0;
        blockCount = 0;
        UpdateCounters?.Invoke(moveLimit, currentLevelNumber);
    }
}
