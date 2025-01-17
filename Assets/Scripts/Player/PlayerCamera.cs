using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerCamera : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public InventoryManager inventoryManager;
    GameManager gameManager;

    float xRotation;
    float yRotation;
    DepthOfField depthOfField;
    Volume volume;
    Vector3 startPosition;
    Quaternion startRotation;

    private void Start() 
    {
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out depthOfField);
    }

    private void Update() {
        if (!inventoryManager.inventoryDisplayed && gameManager.spawned) {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            depthOfField.active = false;
            transform.localPosition = new Vector3(0f, 0.5f, -0.057f);
        } else if(!gameManager.spawned) {
            if (gameManager.spawning) {
                transform.localPosition = Vector3.Lerp(new Vector3(0f, 0.5f, -0.057f), startPosition, gameManager.spawnTimer);
                depthOfField.focalLength = new ClampedFloatParameter(Mathf.Lerp(1, 200, gameManager.spawnTimer), 1, 300, true);
                transform.rotation = Quaternion.Lerp(Quaternion.identity, startRotation, gameManager.spawnTimer);
            } else {
                transform.localPosition = new Vector3(0, 0, 75);
                transform.LookAt(new Vector3(0, -15, 0), Vector3.up);
                depthOfField.active = true;
                startPosition = transform.localPosition;
                startRotation = transform.rotation;
            }
        }
    }
}
