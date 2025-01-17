using UnityEngine;

public enum SlotTag {None, Head, Chest, Legs, Feet, Tool}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public Sprite icon;
    public SlotTag itemTag;
    public string itemName;
    public string itemDescription;

    public GameObject itemObject;

}
