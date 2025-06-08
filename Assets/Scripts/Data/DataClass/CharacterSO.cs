using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Scriptable Objects/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public string characterID;
    public string characterName;
    public string characterPrefab;
    public float HP;
    public float SP;
    public float Food;
    public float SPD;
    public float attackDamage;
}
