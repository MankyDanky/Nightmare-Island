using UnityEngine;

public class CrawlerAnimationEvents : MonoBehaviour
{
    Player player;
    Crawler crawler;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        crawler = GetComponentInParent<Crawler>();
    }

    public void Attack() {
        if ((player.gameObject.transform.position - transform.parent.position).magnitude < crawler.attackRange) 
            player.Damage(crawler.damage);
    }
}
