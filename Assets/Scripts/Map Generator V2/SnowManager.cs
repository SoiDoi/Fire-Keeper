using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SnowManager : MonoBehaviour
{
    public Tilemap snowTilemap;
    public TileBase snowTile;
    public Transform fireTransform;
    public float tileSize = 1f;
    public float maxRadius = 20f;
    public float checkInterval = 1f;

    public float currentHeatRadius = 20f;

    void Start()
    {
        snowTilemap = GetComponent<Tilemap>();
        InvokeRepeating(nameof(UpdateSnowTiles), 0f, checkInterval);

    }
    private void FixedUpdate()
    {
        currentHeatRadius = fireTransform.GetComponent<CircleCollider2D>().radius;
    }

    void UpdateSnowTiles()
    {
        for (float x = -currentHeatRadius; x <= currentHeatRadius; x += tileSize) //remove in heat range
        {
            for (float y = -currentHeatRadius; y <= currentHeatRadius; y += tileSize)
            {
                Vector3 worldPos = fireTransform.position + new Vector3(x, y, 0);
                float distance = Vector3.Distance(fireTransform.position, worldPos);

                if (distance <= currentHeatRadius)
                {
                    Vector3Int cellPos = snowTilemap.WorldToCell(worldPos);
                    if (snowTilemap.HasTile(cellPos))
                        snowTilemap.SetTile(cellPos, null);
                }
            }
        }

        for (float x = -maxRadius; x <= maxRadius; x += tileSize) //add snow out heat range
        {
            for (float y = -maxRadius; y <= maxRadius; y += tileSize)
            {
                Vector3 worldPos = fireTransform.position + new Vector3(x, y, 0);
                float distance = Vector3.Distance(fireTransform.position, worldPos);

                if (distance > currentHeatRadius)
                {
                    Vector3Int cellPos = snowTilemap.WorldToCell(worldPos);
                    if (!snowTilemap.HasTile(cellPos))
                        snowTilemap.SetTile(cellPos, snowTile);
                }
            }
        }
    }
}
