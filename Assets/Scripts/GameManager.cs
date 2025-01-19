using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{

    public bool spawned;
    public bool spawning;
    public bool dying;
    public GameObject menu;
    public GameObject crosshair;
    public float spawnTimer;
    TimeManager timeManager;
    public GameObject spider;
    public GameObject bat;
    public int wave;
    public GameObject scorpion;
    public Transform[] monsterSpawnPositions;
    InventoryManager inventoryManager;
    public GameObject playerHealthBar;
    public TMP_Text highscoreText;
    public TMP_Text highscoreTextOutline;
   
    void Start() {
        highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString() + " Waves";
        highscoreTextOutline.text = highscoreText.text;
    }

    public void StartGame() {
        spawnTimer = 1f;
        spawning = true;
        menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crosshair.SetActive(true);
        timeManager = GameObject.FindWithTag("TimeManager").GetComponent<TimeManager>();
        timeManager.nightfall.AddListener(SpawnMonster);
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
        timeManager.dawn.AddListener(UpdateWave);
        
    }

    void UpdateWave() {
        wave += 1;
        if (PlayerPrefs.GetInt("Highscore", 0) < wave) {
            PlayerPrefs.SetInt("Highscore", wave);
        }
    }

    void Update() {
        if (spawning) {
            if (spawnTimer > 0) {
                spawnTimer -= Time.deltaTime;
            } else {
                spawned = true;
                spawning = false;
                playerHealthBar.SetActive(true);
            }
        }
        if (dying) {
            if (spawnTimer > 0) {
                spawnTimer -= Time.deltaTime;
            } else {
                dying = false;
                menu.SetActive(true);
                playerHealthBar.SetActive(false);
            }
        }
    }

    void SpawnMonster() {
        if (timeManager.dayTime < Math.PI) {
            return;
        }
        Transform spawnPosition = monsterSpawnPositions[UnityEngine.Random.Range(0, monsterSpawnPositions.Length)];
        int index = UnityEngine.Random.Range(1, 4);
        Debug.Log(index);
        switch (index) {
            case 1:
                Instantiate(spider, spawnPosition.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(scorpion, spawnPosition.position, Quaternion.identity);
                break;
            case 3:
                Instantiate(bat, spawnPosition.position, Quaternion.identity);
                break;
        }
        Invoke("SpawnMonster", UnityEngine.Random.Range(5, 15));
    }

    public void Die() {
        inventoryManager.CloseInventory();
        spawned = false;
        dying = true;
        spawnTimer = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        crosshair.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
