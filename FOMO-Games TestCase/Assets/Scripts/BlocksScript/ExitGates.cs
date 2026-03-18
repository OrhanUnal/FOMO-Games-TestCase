using System;
using UnityEngine;

public class ExitGates : MonoBehaviour
{
    [SerializeField]
    Enums.Directions direction;

    private Enums.BlockColor colorID;

    public void Initialize(int col)
    {
        colorID = (Enums.BlockColor)col;
    }

    public bool IsMatchingColors(Enums.BlockColor colorOfTheBlock) 
    {
        if (colorID == colorOfTheBlock)
            return true;
        else return false;
    }
}
