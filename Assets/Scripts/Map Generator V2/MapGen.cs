using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Tilemaps.Tilemap;

public class MapGen : MonoBehaviour
{
    public const int mapChunkSize = 101;
    public int seed;


    [Header("GridLayer")]
    [SerializeField] public Tilemap Ground_Tile;
    [SerializeField] public Tilemap Snow_Tile;
    [SerializeField] public Tilemap Build_Tile;

    [Header("Tiles")]
    [SerializeField] public TileBase waterTile;
    [SerializeField] public TileBase dirtTile;
    [SerializeField] public TileBase stoneTile;
    [SerializeField] public TileBase snowTile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        seed = Random.Range(0, 100000000);
    }
    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap() // Use for Compile noise map to terrain map
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize,seed);

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                Vector3Int position = new Vector3Int(x - 50, y -50, 0);
                if (position == new Vector3(-1, 0, 0) || position == new Vector3(0, 0, 0)|| position == new Vector3(1, 0, 0) 
                 || position == new Vector3(-1, -1, 0)|| position == new Vector3(0, -1, 0)|| position == new Vector3(1, -1, 0)
                 || position == new Vector3(-1, -2, 0)|| position == new Vector3(0, -2, 0)|| position == new Vector3(1, -2, 0))
                {
                    SetMap(0.4f, position);
                }
                else
                {
                    SetMap(noiseMap[x, y], position);
                }
                    
                
            }
        }
    }

    private void SetMap(float noisevalue, Vector3Int position)
    {
        if (noisevalue < 0.2f)
        {

            Ground_Tile.SetTile(position, waterTile);
        }
        else if (noisevalue < 0.6f)
        {
            Ground_Tile.SetTile(position,dirtTile);
        }
        else
        {
            Ground_Tile.SetTile(position,stoneTile);
        }
        Snow_Tile.SetTile(position, snowTile);
    }
}
