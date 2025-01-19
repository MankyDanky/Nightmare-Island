using UnityEngine;
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
    public Transform playerSpawn;
    Player player;
    float startTime;
   
    void Start() {
        highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString() + " Waves";
        highscoreTextOutline.text = highscoreText.text;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
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
                timeManager.dayTime = Mathf.Lerp(2*Mathf.PI, startTime, spawnTimer);
            } else {
                timeManager.dayTime = 0;
                dying = false;
                menu.SetActive(true);
                playerHealthBar.SetActive(false);

                // Refresh resource gameobjects
                foreach (GameObject tree in GameObject.FindGameObjectsWithTag("Tree")) {
                    tree.GetComponent<Tree>().Refresh();
                }
                foreach (GameObject rock in GameObject.FindGameObjectsWithTag("Rock")) {
                    rock.GetComponent<Rock>().Refresh();
                }
            }
        }
    }

    void SpawnMonster() {
        if (timeManager.dayTime < Math.PI) {
            return;
        }
        Transform spawnPosition = monsterSpawnPositions[UnityEngine.Random.Range(0, monsterSpawnPositions.Length)];
        int index = UnityEngine.Random.Range(1, 4);
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
        startTime = timeManager.dayTime;
        wave = 0;
        highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString() + " Waves";
        highscoreTextOutline.text = highscoreText.text;
        player.maxHealth = 100;
        player.health = 100;
        inventoryManager.ClearInventory();
        player.transform.SetPositionAndRotation(playerSpawn.position, playerSpawn.rotation);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
