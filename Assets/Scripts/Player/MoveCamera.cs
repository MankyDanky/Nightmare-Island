using Unity.Mathematics;
using UnityEngine;


public class MoveCamera : MonoBehaviour
{
    public Transform cameraPostion;
    GameManager gameManager;
    Vector3 startingPosition;
    Vector3 startingRotation;


    void Start() {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();


    }

    private void Update() {
        if (gameManager.spawned) {
            transform.position = cameraPostion.position;
        } else {
            if (gameManager.spawning) {
                transform.position = Vector3.Lerp(cameraPostion.position, startingPosition, gameManager.spawnTimer);
                Quaternion newRotation = Quaternion.identity;
                newRotation.eulerAngles = Vector3.Lerp(Vector3.zero, startingRotation, gameManager.spawnTimer);
                transform.rotation = newRotation;
            } else if (gameManager.dying) {
                transform.position = Vector3.Lerp(new Vector3(0, 50, 0), cameraPostion.position, gameManager.spawnTimer);
                transform.rotation = Quaternion.identity;
            } else {
                transform.position = new Vector3(0, 50, 0);
                transform.Rotate(new Vector3(0, 25*Time.deltaTime, 0));
                startingPosition = transform.position;
                startingRotation = transform.rotation.eulerAngles;
            }
        }
       
    }
}
