using UnityEngine;

public class ToolAnimationManager : MonoBehaviour
{

    Animator animator;
    Player playerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = this.GetComponent<Animator>();
        playerMovement = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void StopDigging() {
        animator.SetBool("IsDigging", false);
        playerMovement.canMove = true;
    }

    public void StopAxing() {
        animator.SetBool("IsAxing", false);
        playerMovement.canMove = true;
    }

    public void StopMining() {
        animator.SetBool("IsMining", false);
        playerMovement.canMove = true;
    }
}
