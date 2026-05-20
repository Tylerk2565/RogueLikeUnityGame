using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _stackLabel;

    public void SetItem(ItemData item, int count)
    {
        _icon.sprite = item.itemSprite;

        if (count > 1)
        {
            _stackLabel.text = count.ToString();
        }
        else
        {
            _stackLabel.text = string.Empty;
        }
    }
}
