using UnityEngine;
using UnityEngine.UIElements;

public class Tree : MonoBehaviour
{
    float health;
    public GameObject hitEffect;
    public GameObject log;
    public GameObject plankGroundItem;
    public GameObject appearEffect;
    bool hittable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hittable = true;
        health = 100;
    }

    public void Hit(int damage, Vector3 position) {
        if (hittable) {
            health -= damage;
            Instantiate(hitEffect, position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            if (health <= 0) {
                Rigidbody rb = log.AddComponent<Rigidbody>();
                rb.AddForce(-Camera.main.transform.right * 4, ForceMode.Impulse);
                hittable = false;
                Invoke("SpawnPlank", 4f);
            }
        }
    }

    void SpawnPlank() {
        Instantiate(plankGroundItem, log.transform.Find("Center").position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        Instantiate(appearEffect, log.transform.Find("Center").position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        Destroy(log);
    }   
}
