using UnityEngine;

public class SnowTrail : MonoBehaviour
{
    [SerializeField] private ParticleSystem snowTrailParticles;

     private Animator animator;

     void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);

        animator = transform.GetChild(0).GetChild(selectedCharacter).GetComponent<Animator>();
    }

void OnCollisionEnter2D(Collision2D collision)
    {
        int layer = LayerMask.NameToLayer("Floor");
        if (collision.gameObject.layer == layer)
        {
            snowTrailParticles.Play();
            animator.SetBool("isJumping", false);
        }
    }

void OnCollisionExit2D(Collision2D collision)
    {
        int layer = LayerMask.NameToLayer("Floor");
        if (collision.gameObject.layer == layer)
        {
            snowTrailParticles.Stop();
            animator.SetBool("isJumping", true);
        }
    }


}
