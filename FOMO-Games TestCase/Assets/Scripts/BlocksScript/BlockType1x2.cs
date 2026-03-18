using System.Collections.Generic;
using UnityEngine;

public class BlockType1x2 : BlockBase
{
    override protected List<Vector3> GetRayOrigins()
    {
        Vector3 longAxis = transform.right * blockSize;
        List<Vector3> origins = new List<Vector3>();
        
        origins.Add(transform.position);
        origins.Add(transform.position + longAxis);
        
        return origins;
    }
}
