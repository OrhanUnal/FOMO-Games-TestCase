using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    private int moveLimit;
    private int blockCount;
    private bool hasMoveLimit;

    public static event Action<bool> OnLevelFinished;
    public static event Action<int> OnMoveCountChanged;

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
            OnLevelFinished?.Invoke(false);
        OnMoveCountChanged?.Invoke(moveLimit);
    }

    private void ChangeBlockCount(int amount)
    {
        blockCount += amount;
        if (blockCount <= 0)
            OnLevelFinished?.Invoke(true);
    }

    public void SetLevelData(int moveLimit)
    {
        this.moveLimit = moveLimit;
        hasMoveLimit = moveLimit > 0;
        blockCount = 0;
        OnMoveCountChanged?.Invoke(moveLimit);
    }
}
