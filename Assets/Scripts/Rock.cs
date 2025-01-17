using UnityEngine;

public class Rock : MonoBehaviour
{
float health;
    public GameObject hitEffect;
    public GameObject stoneGroundItem;
    public GameObject appearEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 100;
    }

    public void Hit(int damage, Vector3 position) {
        health -= damage;
        Instantiate(hitEffect, position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        if (health <= 0) {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            Instantiate(stoneGroundItem, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            Instantiate(appearEffect, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        }
    }
}
