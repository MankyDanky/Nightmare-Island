using System;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Movement")]
    GameManager gameManager;
    public float moveSpeed;
    public bool canMove = true;
    public InventoryManager inventoryManager;
    public Animator playerAnimator;
    public Transform orientation;
    public float airMultiplier;
    public Transform playerObj;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    bool readyToJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;
    float horizontalInput;
    float verticalInput;
    [Header("Combat")]
    public float health;
    public float maxHealth;
    public Image hitEffect;

    Vector3 moveDirection;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        rb.freezeRotation = true;
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
    }

    private void MyInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 || verticalInput != 0) {
            playerAnimator.SetBool("IsWalking", true);
        } else {
            playerAnimator.SetBool("IsWalking", false);
        }

        if (Input.GetKey(jumpKey) && readyToJump && grounded) {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    void Update()
    {
        if (!inventoryManager.inventoryDisplayed && canMove && gameManager.spawned) {
            MyInput();
            SpeedControl();
            RaycastHit hit;
            grounded = Physics.Raycast(transform.position, Vector3.down, out hit,playerHeight * 0.5f + 0.2f, whatIsGround);

            if (grounded) 
                rb.linearDamping = groundDrag;
            else
                rb.linearDamping = 0;
        }
        if (hitEffect.color.a > 0) {
            Color color = hitEffect.color;
            color.a -= 1f*Time.deltaTime;
            hitEffect.color = color;
        }
    }

     void FixedUpdate() {
        if (!inventoryManager.inventoryDisplayed && canMove && gameManager.spawned) {
            MovePlayer();
            playerObj.eulerAngles = new Vector3(
                0,
                orientation.eulerAngles.y,
                0
            );
        }
    }

    private void MovePlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 100f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * 100f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl() {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude > moveSpeed) {
            Vector3 newVelocity = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(newVelocity.x, rb.linearVelocity.y, newVelocity.z);
        }
    }

    private void Jump() {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump() {
        readyToJump = true;
    }

    public void StopMoving() {
        playerAnimator.SetBool("IsWalking", false);
    }

    public void Damage(int damage) {
        health -= damage;
        Color color = hitEffect.color;
        color.a = 0.8f;
        hitEffect.color = color;
        if (health <= 0) {
            gameManager.Die();
        }
    }
}
