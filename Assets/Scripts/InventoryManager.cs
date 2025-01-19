using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Button equipButton;
    public Button combineButton;
    public Button removeButton;

    public InventoryTile[] inventoryTiles;
    public Item selectedItem;
    public Item draggingItem;
    public Image draggingItemImage;
    public TMP_Text selectedItemTitle;
    public InventoryTile hoveredTile;
    public InventoryTile sourceTile;
    public InventoryTile selectedTile;
    public TMP_Text selectedItemDescription;
    public bool inventoryDisplayed;
    public Animator inventoryAnimator;
    public Item[] items;
    public Sprite selectedTileSprite;
    public Sprite unselectedTileSprite;
    public GameObject crosshair;
    Item toolEquipped;
    GameObject toolObject;
    Item combineSlot1;
    Item combineSlot2;
    public InventoryTile combineTile1;
    public InventoryTile combineTile2;
    Player player;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        // Update inventory tiles to current inventory
        for (int i = 0; i < items.Length; i++) {
            if (items[i] != null) {
                inventoryTiles[i].UpdateItem(items[i]);
            }
        }
    }

    // Add item to inventory
    public bool AddItem(Item newItem) {
        for (int i = 0; i < items.Length; i++) {
            if (items[i] == null) {
                items[i] = newItem;
                inventoryTiles[i].UpdateItem(newItem);
                return true;
            }
        }
        return false;
    }

    // Select item
    public void SelectItem(Item newSelectedItem, InventoryTile newSelectedTile) {
        if (selectedTile != null) {
            selectedTile.backgroundImage.sprite = unselectedTileSprite;
        }
        selectedTile = newSelectedTile;
        selectedTile.backgroundImage.sprite = selectedTileSprite;
        selectedItem = newSelectedItem;
        selectedItemTitle.text = newSelectedItem.itemName;
        selectedItemDescription.text = newSelectedItem.itemDescription;
        removeButton.gameObject.SetActive(true);
        if (newSelectedItem.itemTag != SlotTag.None) {
            equipButton.gameObject.SetActive(true);
            switch (newSelectedItem.itemTag) {
                case SlotTag.Tool:
                    if (toolEquipped == newSelectedItem) {
                        equipButton.GetComponentInChildren<TMP_Text>().text = "Unequip";
                    } else {
                        equipButton.GetComponentInChildren<TMP_Text>().text = "Equip";
                    }
                    break;
            }
        }

    }
    
    public void DeselectItem() {
        if (selectedTile != null) {
            selectedTile.backgroundImage.sprite = unselectedTileSprite;
            selectedTile = null;
            selectedItem = null;
            selectedItemTitle.text = "";
            selectedItemDescription.text = "";
            equipButton.gameObject.SetActive(false);
            removeButton.gameObject.SetActive(false);
        }
    }

    public void SetDragging(Item newDraggingItem) {
        draggingItem = newDraggingItem;
        draggingItemImage.transform.position = Input.mousePosition;
        draggingItemImage.enabled = true;
        draggingItemImage.sprite = newDraggingItem.icon;
        DeselectItem();
    }

    void RemoveDragging() {
        draggingItem = null;
        draggingItemImage.enabled = false;
    }

    public void RemoveSelected() {
        switch (selectedItem.itemTag) {
            case SlotTag.Tool:
                Destroy(toolObject);
                toolObject = null;
                toolEquipped = null;
                break;
        }
        for (int i = 0; i < 12; i++) {
            if (selectedTile == inventoryTiles[i]) {
                selectedTile.RemoveItem();
                DeselectItem();
                items[i] = null;
                break;
            }
        }
    }

    public void Combine() {
        AddItem(Recipes.fetchItem(combineSlot1.itemName + combineSlot2.itemName));
        combineTile1.RemoveItem();
        combineSlot1 = null;
        combineTile2.RemoveItem();
        combineSlot2 = null;
        combineButton.gameObject.SetActive(false);
    }

    public void Equip() {
        switch (selectedItem.itemTag) {
            case SlotTag.Tool:
                if (toolEquipped != selectedItem) {
                    if (toolObject != null) {
                        Destroy(toolObject);
                    }
                    toolObject = Instantiate(selectedItem.itemObject, GameObject.FindWithTag("RightHand").transform);
                    toolEquipped = selectedItem;
                    equipButton.GetComponentInChildren<TMP_Text>().text = "Unequip";
                } else {
                    Destroy(toolObject);
                    toolObject = null;
                    toolEquipped = null;
                    equipButton.GetComponentInChildren<TMP_Text>().text = "Equip";
                }
                break;
        }
    }

    public void CloseInventory() {
        inventoryAnimator.SetBool("InventoryDisplayed", false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crosshair.SetActive(true);
    }

    void Update()
    {
        // Hide/display inventory
        if (Input.GetKeyDown(KeyCode.I) && gameManager.spawned) {
            inventoryDisplayed = !inventoryDisplayed;
            if (inventoryDisplayed) {
                inventoryAnimator.SetBool("InventoryDisplayed", true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                crosshair.SetActive(false);
                player.StopMoving();
            } else {
                CloseInventory();
            }
        }
        
        if (draggingItem != null) {
            draggingItemImage.transform.position = Input.mousePosition;
        }

        if (draggingItem != null && !Input.GetMouseButton(0)) {
            if (hoveredTile && hoveredTile.occupiedItem == null) {
                hoveredTile.UpdateItem(draggingItem);
                for (int i = 0; i < 12; i++) {
                    if (!sourceTile.combineTile && sourceTile == inventoryTiles[i]) {
                        items[i] = null;
                    } else if (sourceTile.combineTile) {
                        switch(sourceTile.combineSlot) {
                            case 1:
                                combineSlot1 = null;
                                combineButton.gameObject.SetActive(false);
                                break;
                            case 2:
                                combineSlot2 = null;
                                combineButton.gameObject.SetActive(false);
                                break;
                        }
                    }

                    if (!hoveredTile.combineTile && hoveredTile == inventoryTiles[i]) {
                        items[i] = draggingItem;
                    } else if (hoveredTile.combineTile) {
                        switch(hoveredTile.combineSlot) {
                            case 1:
                                combineSlot1 = draggingItem;
                                if (combineSlot2 != null) {
                                    combineButton.gameObject.SetActive(true);
                                }
                                break;
                            case 2:
                                combineSlot2 = draggingItem;
                                if (combineSlot1 != null) {
                                    combineButton.gameObject.SetActive(true);
                                }
                                break;
                        }
                    }
                }
                RemoveDragging();
            } else {
                sourceTile.UpdateItem(draggingItem);
                RemoveDragging();
            }
            
        }
    }
}
