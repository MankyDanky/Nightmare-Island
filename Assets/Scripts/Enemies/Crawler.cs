using UnityEngine;

public class Crawler : MonoBehaviour
{

    Transform player;
    Animator animator;
    public float attackRange;
    Rigidbody rb;
    public float moveSpeed;
    float delay;
    bool attackDelayStarted;
    bool walkDelayStarted;
    public int damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 targetPostition = new Vector3(player.position.x, 
                    this.transform.position.y, 
                    player.position.z );
        transform.LookAt(targetPostition);
        float distance = (player.position - transform.position).magnitude;
        if (distance < attackRange) {
            animator.SetBool("IsWalking", false);
            walkDelayStarted = false;
            if (attackDelayStarted) {
                if (delay <= 0) {
                    animator.SetBool("IsAttacking", true);
                } else {
                    delay -= Time.deltaTime * 500;
                }
                
            } else {
                attackDelayStarted = true;
                delay = 150;
            }
        } else {
            animator.SetBool("IsAttacking", false);
            attackDelayStarted = false;
            if (walkDelayStarted) {
                if (delay <= 0) {

                    rb.AddForce(transform.forward * moveSpeed * 25f, ForceMode.Force);
                    animator.SetBool("IsWalking", true);
                } else {
                    delay -= Time.deltaTime * 500;
                }
            } else {
                walkDelayStarted = true;
                delay = 150;
            }
            
        }
    }
}
