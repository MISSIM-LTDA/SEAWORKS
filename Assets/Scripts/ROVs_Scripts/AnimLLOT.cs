using UnityEngine;

public class AnimLLOT : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private KeyCode lockLLOT = KeyCode.Y;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(lockLLOT) && !AnimatorIsPlaying()){
            LockAnim();
        }
    }

    private bool AnimatorIsPlaying() 
    {
        float lenght = animator.GetCurrentAnimatorStateInfo(0).length;
        float time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime * lenght;

        return lenght > time || animator.IsInTransition(0);
    }

    private void LockAnim()
    {
        animator.SetBool("lock", 
            !animator.GetBool("lock"));
    }
}