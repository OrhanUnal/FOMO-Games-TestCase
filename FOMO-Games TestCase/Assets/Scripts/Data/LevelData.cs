using System.Collections.Generic;

[System.Serializable]
public class LevelData
{
    public int MoveLimit;
    public int RowCount;
    public int ColCount;
    public List<CellData> CellInfo;
    public List<MovableData> MovableInfo;
    public List<ExitData> ExitInfo;
}

[System.Serializable]
public class CellData
{
    public int Row;
    public int Col;
}

[System.Serializable]
public class MovableData
{
    public int Row;
    public int Col;
    public List<int> Direction;
    public int Length;
    public int Colors;
}

[System.Serializable]
public class ExitData
{
    public int Row;
    public int Col;
    public int Direction;
    public int Colors;
}