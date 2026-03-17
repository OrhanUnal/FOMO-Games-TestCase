using UnityEngine;

[CreateAssetMenu(fileName = "BlockShapeSO", menuName = "Scriptable Objects/BlockShapeSO")]
public class BlockShapeSO : ScriptableObject
{
    public GameObject block1x1Prefab;
    public GameObject block1x2Prefab;

    public GameObject GetBlockPrefab(int length)
    {
        switch (length)
        {
            case 1: return block1x1Prefab;
            case 2: return block1x2Prefab;
            default:
                Debug.Log("There is not such block right now");
                return block1x1Prefab;
        }
    }
}
