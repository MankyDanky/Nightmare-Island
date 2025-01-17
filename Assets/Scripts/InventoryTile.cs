using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryTile : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    public Item occupiedItem;
    public Image iconImage;
    public Image backgroundImage;
    public InventoryManager inventoryManager;

    public bool combineTile;
    public int combineSlot;
    
    void Start()
    {
        backgroundImage = this.GetComponent<Image>();
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
        if (occupiedItem != null) {
            iconImage.sprite = occupiedItem.icon;
        } else {
            iconImage.enabled = false;
        }
    }

    public void UpdateItem(Item newItem) {
        iconImage.enabled = true;
        occupiedItem = newItem;
        iconImage.sprite = newItem.icon;
    }

    public void RemoveItem() {
        iconImage.enabled = false;
        occupiedItem = null;
    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (occupiedItem != null && !combineTile) {
            inventoryManager.SelectItem(occupiedItem, this);
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        inventoryManager.hoveredTile = this;
    }

    public void OnPointerDown(PointerEventData pointerEventData) {
        if (occupiedItem != null) {
            Invoke("SetDragging", 0.25f);
        }
    }

    void SetDragging() {
        if (Input.GetMouseButton(0)) {
            inventoryManager.SetDragging(occupiedItem);
            RemoveItem();
            inventoryManager.sourceTile = this;
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        inventoryManager.hoveredTile = null;
    }
}
