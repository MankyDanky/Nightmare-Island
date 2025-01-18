using System;
using UnityEngine;
using UnityEngine.Animations;

public class Enemy : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public GameObject hitEffect;
    public GameObject deathEffect;
    Rigidbody rb;
    public RectTransform healthbar;
    float maxHealthbarSize;
    GameManager gameManager;
    [Serializable] public class ItemDrop { public float chance; public GameObject item; }
    public ItemDrop[] itemDrops;
    public MonoBehaviour enemyBehaviour;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        maxHealthbarSize = healthbar.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.dying) {
            enemyBehaviour.enabled = false;
            GetComponentInChildren<Animator>().enabled = false;
        } else if (!gameManager.spawned) {
            Destroy(this.gameObject);
        }
    }

    public void Damage(float damage, Vector3 position, Vector3 direction) {
        Instantiate(hitEffect, position, Quaternion.Euler(-90, 0, 0));
        rb.AddForce(direction*50f, ForceMode.Impulse);
        health -= damage;
        if (health <= 0) {
            Die();
        }
        healthbar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxHealthbarSize*health/maxHealth);
    }

    void Die() {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        foreach (ItemDrop itemDrop in itemDrops) {
            if (UnityEngine.Random.Range(0f, 1f) < itemDrop.chance) {
                Instantiate(itemDrop.item, transform.position, Quaternion.Euler(-90, 0, 0));
            }
        }
        Destroy(gameObject);
    }
}
