using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class Tree : MonoBehaviour
{
    float health;
    public float maxHealth;
    public GameObject hitEffect;
    public GameObject log;
    public GameObject plankGroundItem;
    public GameObject appearEffect;
    bool hittable;
    Vector3 logPosition;
    Quaternion logRotation;
    Rigidbody rb;
    public float refreshTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        health = maxHealth;
        hittable = true;
        logPosition = log.transform.position;
        logRotation = log.transform.rotation;
    }

    void Start() {
        rb = log.GetComponent<Rigidbody>();
    }

    public void Hit(int damage, Vector3 position) {
        if (hittable) {
            health -= damage;
            Instantiate(hitEffect, position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            if (health <= 0) {
                rb.constraints = RigidbodyConstraints.None;
                rb.AddForce(-Camera.main.transform.right * 4, ForceMode.Impulse);
                hittable = false;
                Invoke("SpawnPlank", 4f);
                Invoke("Refresh", refreshTime);
            }
        }
    }

    public void Refresh() {
        hittable = true;
        rb.constraints = RigidbodyConstraints.FreezeAll; 
        log.SetActive(true);
        log.transform.SetPositionAndRotation(logPosition, logRotation);
        Instantiate(appearEffect, log.transform.Find("Center").position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        health = maxHealth;
    }

    void SpawnPlank() {
        Instantiate(plankGroundItem, log.transform.Find("Center").position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        Instantiate(appearEffect, log.transform.Find("Center").position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        log.SetActive(false);
    }   
}
