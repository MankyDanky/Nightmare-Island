using System.Collections;
using UnityEngine;

public class Trowel : MonoBehaviour
{
    Player playerMovement;
    public InventoryManager inventoryManager;
    public GameObject dirtGroundItem;
    public GameObject sandGroundItem;
    public GameObject sandDigEffect;
    public GameObject dirtDigEffect;

    void Start() {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<Player>();
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && playerMovement.grounded && !playerMovement.playerAnimator.GetBool("IsDigging")  && !inventoryManager.inventoryDisplayed) {
            RaycastHit hit;
            bool canDig = Physics.Raycast(transform.position, Vector3.down + playerMovement.orientation.forward, out hit, playerMovement.playerHeight, playerMovement.whatIsGround);
            if (canDig) {
                if (hit.collider.gameObject.name == "Dirt") {
                    StartCoroutine(spawnResource(dirtGroundItem, hit.point, dirtDigEffect));
                } else {
                    StartCoroutine(spawnResource(sandGroundItem, hit.point, sandDigEffect));
                }
                playerMovement.StopMoving();
                playerMovement.playerAnimator.SetBool("IsDigging", true);
                playerMovement.canMove = false;
            }
        }
    }

    IEnumerator spawnResource(GameObject resource, Vector3 spawnPosition, GameObject effect) {
        yield return new WaitForSeconds(0.5f);
        Instantiate(resource, spawnPosition, Quaternion.Euler(new Vector3(-90, 0, 0)));
        Instantiate(effect, spawnPosition, Quaternion.Euler(new Vector3(-90, 0, 0)));
    }
}
