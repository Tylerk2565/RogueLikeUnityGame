using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public string itemName;
    public string itemType;
    public Sprite itemSprite;
}
