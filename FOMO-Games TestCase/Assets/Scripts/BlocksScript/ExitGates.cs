using System;
using UnityEngine;

public class ExitGates : MonoBehaviour
{
    [SerializeField]
    Enums.Directions direction;

    private Enums.BlockColor colorID;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
