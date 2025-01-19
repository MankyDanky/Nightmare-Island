using UnityEngine;

public class Rock : MonoBehaviour
{
    float health;
    public float maxHealth;
    public GameObject hitEffect;
    public GameObject stoneGroundItem;
    public GameObject appearEffect;
    public float refreshTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;;
    }

    public void Hit(int damage, Vector3 position) {
        health -= damage;
        Instantiate(hitEffect, position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        if (health <= 0) {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            Instantiate(stoneGroundItem, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            Instantiate(appearEffect, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            Invoke("Refresh", refreshTime);
        }
    }

    public void Refresh() {
        Instantiate(appearEffect, transform.position, Quaternion.identity);
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        health = maxHealth;
    }
}
