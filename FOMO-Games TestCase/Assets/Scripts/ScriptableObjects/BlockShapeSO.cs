using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockShapeSO", menuName = "Scriptable Objects/BlockShapeSO")]
public class BlockShapeSO : ScriptableObject
{
    [System.Serializable]
    public class PrefabEntry
    {
        public int id;
        public GameObject prefab;
    }

    public List<PrefabEntry> prefabs;

    public GameObject GetPrefab(int id)
    {
        foreach (PrefabEntry entry in prefabs)
        {
            if (entry.id == id)
                return entry.prefab;
        }
        Debug.LogWarning($"No prefab found for id: {id}");
        return null;
    }
}
