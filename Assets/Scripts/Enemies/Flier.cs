using UnityEngine;

public class Flier : MonoBehaviour
{
    Transform player;
    Animator animator;
    Rigidbody rb;
    public float moveSpeed;
    public float attackRange;
    float delay;
    bool attackDelayStarted;
    bool walkDelayStarted;
    bool chargeDelayStarted;
    public float attackDelay;
    public float walkDelay;
    public Vector3 chargeDirection;
    public int damage;
    public float chargeDuration;


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
        float distance = (player.position - transform.position).magnitude;
        if (chargeDelayStarted) {
            rb.AddForce(100f * moveSpeed * chargeDirection, ForceMode.Force);
            delay -= Time.deltaTime;
            if (delay <= 0) {
                chargeDelayStarted = false;
                attackDelayStarted = false;
            }
        } else if (distance < attackRange) {
            walkDelayStarted = false;
            if (attackDelayStarted) {
                if (delay <= 0) {
                    animator.SetBool("IsCharging", true);
                    chargeDelayStarted = true;
                    delay = chargeDuration;
                } else {
                    delay -= Time.deltaTime;
                    chargeDirection = transform.forward;
                    transform.LookAt(targetPostition);
                }
            } else {
                attackDelayStarted = true;
                delay = attackDelay;
            }
        } else {
            transform.LookAt(targetPostition);
            animator.SetBool("IsCharging", false);
            attackDelayStarted = false;
            if (walkDelayStarted) {
                if (delay <= 0) {
                    rb.AddForce(transform.forward * moveSpeed * 25f, ForceMode.Force);
                    if (transform.position.y < player.position.y - 1) {
                        rb.AddForce(Vector3.up * 25f, ForceMode.Force);
                    } else if (transform.position.y > player.position.y + 1) {
                        rb.AddForce(-Vector3.up * 25f, ForceMode.Force);
                    }
                } else {
                    delay -= Time.deltaTime;
                }
            } else {
                walkDelayStarted = true;
                delay = walkDelay;
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            rb.AddForce(-transform.right * 250f, ForceMode.Impulse);
            player.gameObject.GetComponent<Player>().Damage(damage);
        }
    }
}
