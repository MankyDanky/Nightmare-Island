using UnityEngine;

public class GroundItem : MonoBehaviour
{

    public Item item;
    public float pickupDistance;
    public InventoryManager inventoryManager;
    Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
        if (pickupDistance == 0) {
            pickupDistance = 5;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float d = (player.position - transform.position).magnitude;
        if (d < pickupDistance && Input.GetKey(KeyCode.E)) {
            if (inventoryManager.AddItem(item)) {
                Destroy(gameObject);
            }
        }
    }
}
