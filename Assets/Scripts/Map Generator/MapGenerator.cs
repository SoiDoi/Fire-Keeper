using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persitance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpadate;

    public void GenerateMap() // Use for Compile noise map to terrain map
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight,seed, noiseScale,octaves,persitance,lacunarity,offset);

        MapDisplay display = FindAnyObjectByType<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
    }

    private void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }
        if (mapHeight < 1)
        {
            mapHeight = 1;
        }
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
