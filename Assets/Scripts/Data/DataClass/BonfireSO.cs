using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewBonfire", menuName = "Scriptable Objects/BonfireSO")]
public class BonfireSO : ScriptableObject
{
    [Header("Data")]
    public string bonfireName;
    public float bonfirePower;
    public float decayRate; // perSec
    public float bonfireRadius;
    public int bonfireHP;

    [Header("Prefab")]
    public Tilemap bonfireTile;
    public GameObject bonfirePrefab;
}
