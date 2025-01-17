using UnityEngine;

public class Hatchet : MonoBehaviour
{

    public float range;
    Player player;
    InventoryManager inventoryManager;
    
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && player.grounded && !player.playerAnimator.GetBool("IsAxing") && !inventoryManager.inventoryDisplayed) {
            player.StopMoving();
            player.playerAnimator.SetBool("IsAxing", true);
            player.canMove = false;
            Invoke("Hit", 0.35f);
        }
    }

    void Hit() {
        RaycastHit hit;
        bool reached = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range);

        if (reached) {
            if (hit.collider.gameObject.CompareTag("Tree")) {
                hit.collider.gameObject.GetComponent<Tree>().Hit(30, hit.point);
            }
            else if (hit.collider.transform.parent && hit.collider.transform.parent.CompareTag("Tree")) {
                hit.collider.transform.parent.GetComponent<Tree>().Hit(30, hit.point);
            }
            else if (hit.collider.transform.parent && hit.collider.transform.parent.CompareTag("Enemy")) {
                hit.collider.transform.parent.GetComponent<Enemy>().Damage(30, hit.point, player.orientation.forward);
            }
        }
    }
}
