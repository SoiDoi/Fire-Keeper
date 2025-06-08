using TMPro;
using UnityEngine;

public class InventoryStat : MonoBehaviour
{
    public Player player;
    public TMP_Text status;

    private void Update()
    {
        StatusUpdate();
    }
    private void StatusUpdate()
    {
        if (status != null && player != null)
        {
            status.text = "Hp: " + (int)player.currentplayerHP + "/" + player.character.HP.ToString() + "\n" + "Damage: " + player.attackDamage.ToString() + "\n" + "SPD: " + player.playerSPD.ToString();
        }
    }
        
}
