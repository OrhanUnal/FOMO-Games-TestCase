using System.Collections.Generic;
using UnityEngine;

public class MaterialFactory
{
    private Dictionary<string, Material> cache = new Dictionary<string, Material>();

    public Material GetMaterial(int colorId, int cubeType = 1, bool isVertical = true)
    {
        string colorName = ((Enums.BlockColor)colorId).ToString();
        if (colorName == null) return null;

        string orientation = isVertical ? "Up" : "Paralel";
        string textureName = $"Cube{cubeType:D2}{colorName}{orientation}TextureMap";
        string texturePath = $"Textures/Cube{cubeType}/{textureName}";

        if (cache.ContainsKey(textureName))
            return cache[textureName];

        Texture2D texture = Resources.Load<Texture2D>(texturePath);
        if (texture == null)
        {
            Debug.LogWarning($"Texture not found at: {texturePath}");
            return null;
        }

        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.SetTexture("_BaseMap", texture);
        mat.name = textureName;

        cache[textureName] = mat;
        return mat;
    }
}