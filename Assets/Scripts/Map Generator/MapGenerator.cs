using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap,ColourMap};
    public DrawMode drawMode;

    const int mapChunkSize = 256;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persitance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpadate;

    public TerrainType[] regions;

    public void GenerateMap() // Use for Compile noise map to terrain map
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale,octaves,persitance,lacunarity,offset);

        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
        MapDisplay display = FindAnyObjectByType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
    }

    private void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
        if (noiseScale < 0)
        {
            noiseScale = 0.01f;
        }
    }
}


[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}