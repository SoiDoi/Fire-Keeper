using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

[CreateAssetMenu(menuName = "Sprites/RuleTiles/FireKeeperRuleTiles")]
public class FireKeeperRuleTiles : RuleTile<FireKeeperRuleTiles.Neighbor> 
{
    public enum TileType
    {
        Snow,
        Dirt,
        Stone,
        Ice
    }

    public TileType tileType;

    public class Neighbor : RuleTile.TilingRule.Neighbor 
    {
        

        public const int Nothing = 3;
        public const int Snow = 4;
        public const int Dirt = 5;
        public const int Stone = 6;
        public const int Ice = 7;
        public const int NotIsSnow= 8;
        public const int NotIsSnowAndDirt= 9;
        
    }

    public override bool RuleMatch(int neighbor, TileBase tile) 
    {
        FireKeeperRuleTiles other = tile as FireKeeperRuleTiles;
        switch (neighbor) 
        {
            case Neighbor.Nothing: return Check_Nothing(tile);
            case Neighbor.Snow: return other != null && other.tileType == TileType.Snow;
            case Neighbor.Dirt: return other != null && other.tileType == TileType.Dirt;
            case Neighbor.Stone: return other != null && other.tileType == TileType.Stone;
            case Neighbor.Ice: return other != null && other.tileType == TileType.Ice;
            case Neighbor.NotIsSnow: return other != null && other.tileType != TileType.Snow;
            case Neighbor.NotIsSnowAndDirt: return other != null && other.tileType != TileType.Snow && other.tileType != TileType.Dirt;


        }
        return base.RuleMatch(neighbor, tile);
    }
    bool Check_Nothing(TileBase tile)
    {
        return tile == null;
    }


}