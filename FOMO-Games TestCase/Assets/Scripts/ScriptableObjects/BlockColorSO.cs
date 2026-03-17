using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockColorSO", menuName = "Scriptable Objects/BlockColorSO")]
public class BlockColorSO : ScriptableObject
{
    [System.Serializable]
    public class ColorEntry
    {
        public int colorId;
        public string colorName;
    }

    public List<ColorEntry> colorEntries;
    //eger bir rengi ve sekli daha onceden kullanmissa direkt kullan
    private Dictionary<string, Material> materialCache = new Dictionary<string, Material>();

    public string GetColorName(int colorId)
    {
        foreach (ColorEntry entry in colorEntries)
        {
            if (entry.colorId == colorId)
                return entry.colorName;
        }
        Debug.LogWarning($"No color name found for colorId: {colorId}");
        return null;
    }

    public Material GetMaterial(int colorId, int cubeType, bool isVertical)
    {
        string colorName = GetColorName(colorId);
        if (colorName == null) return null;

        string orientation = isVertical ? "Up" : "Paralel";
        string textureName = $"Cube{cubeType:D2}{colorName}{orientation}TextureMap";
        string texturePath = $"Textures/Cube{cubeType}/{textureName}";

        if (materialCache.ContainsKey(textureName))
            return materialCache[textureName];

        Texture2D texture = Resources.Load<Texture2D>(texturePath);
        if (texture == null)
        {
            Debug.LogWarning($"Texture not found at: {texturePath}");
            return null;
        }

        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.SetTexture("_BaseMap", texture);
        mat.name = textureName;

        materialCache[textureName] = mat;
        return mat;
    }
}
