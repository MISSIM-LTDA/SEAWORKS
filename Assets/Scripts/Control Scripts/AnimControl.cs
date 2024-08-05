using UnityEngine;

public class AnimControl : MonoBehaviour
{
    [SerializeField] private GameObject mesh;
    [SerializeField] private Animator animator;

    [SerializeField] private KeyCode animationKey = KeyCode.P;

    private bool inIndicator;
    private bool reversePlay;
    void Update()
    {
        if(inIndicator && Input.GetKeyDown(animationKey) && 
            !AnimatorIsPlaying()) {
            PlayAnim();
        }
    }
    void PlayAnim() 
    {
        if(reversePlay) { animator.Play("1"); }

        else { animator.Play("2"); }

        reversePlay = !reversePlay;
    }
    private bool AnimatorIsPlaying()
    {
        float length = animator.GetCurrentAnimatorStateInfo(0).length;
        float time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime * length;

        return length > time || animator.IsInTransition(0);
    }
    private void OnTriggerEnter(Collider collider)
    {
       if (collider.gameObject.CompareTag("Trigger")) {
         mesh.AddComponent<MeshCollider>();
         inIndicator = true;
       }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Trigger")) {
          Destroy(mesh.GetComponent<MeshCollider>());
          inIndicator = false;
        }
    }
}
