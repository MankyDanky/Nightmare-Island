using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public bool spawned;
    public bool spawning;
    public GameObject menu;
    public GameObject crosshair;
    public float spawnTimer;
   
    public void StartGame() {
        spawnTimer = 1f;
        spawning = true;
        menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crosshair.SetActive(true);
    }

    void Update() {
        if (spawning) {
            if (spawnTimer > 0) {
                spawnTimer -= Time.deltaTime;
            } else {
                spawned = true;
                spawning = false;
            }
        } 
    }

    public void QuitGame() {
        Application.Quit();
    }
}
