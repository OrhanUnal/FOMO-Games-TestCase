using System.Collections.Generic;
using UnityEngine;

public class BlockBase : MonoBehaviour
{
    private int colorId;
    private List<int> directions;
    private int row;
    private int col;

    public void Initialize(int ColorID, List<int> Directions, int Row, int Col)
    {
        colorId = ColorID;
        directions = Directions;
        row = Row;
        col = Col;
    }
}