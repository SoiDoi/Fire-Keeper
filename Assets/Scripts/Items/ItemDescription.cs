using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemTitle;
    [SerializeField] private TMP_Text itemDescription;

    private void Awake()
    {
        ResetDescription();
    }

    public void ResetDescription()
    {
        this.itemImage.gameObject.SetActive(false);
        this.itemTitle.text = "";
        this.itemDescription.text = "";
    }

    public void SetDescription(Sprite sprite, string itemTitle, string itemDescription)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.itemTitle.text = itemTitle;
        this.itemDescription.text = itemDescription;
    }

}
