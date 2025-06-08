using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int chunkSize, int seed)
    {
        // Create a PRNG from the seed
        System.Random prng = new System.Random(seed);

        // Generate random values based on the seed
        float scale = (float)(prng.NextDouble() * 80 + 20);
        int octaves = prng.Next(3, 8); // Range: 3 to 8
        float persistence = (float)(prng.NextDouble() * 0.5 + 0.3); // Range: 0.3 to 0.8
        float lacunarity = (float)(prng.NextDouble() * 2 + 1.5); // Range: 1.5 to 3.5
        Vector2 offset = new Vector2(prng.Next(-100000, 100000), prng.Next(-100000, 100000));

        return GenerateNoiseMap(chunkSize, seed, scale, octaves, persistence, lacunarity, offset);
    }

    private static float[,] GenerateNoiseMap(int chunkSize, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[chunkSize, chunkSize];

        System.Random prng = new System.Random(seed);
        Vector2[] octavesOffsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octavesOffsets[i] = new Vector2(offsetX, offsetY);
        }

        scale = Mathf.Max(scale, 0.0001f);
        float invScale = 1f / scale;

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = chunkSize / 2f;
        float halfHeight = chunkSize / 2f;

        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) * invScale * frequency + octavesOffsets[i].x;
                    float sampleY = (y - halfHeight) * invScale * frequency + octavesOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                noiseMap[x, y] = noiseHeight;
            }
        }

        // Normalize values to the range [0,1]
        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
