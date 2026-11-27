using UnityEngine;

public class SnowTrail : MonoBehaviour
{
    [SerializeField] private ParticleSystem snowTrailParticles;

     private Animator animator;

     void Start()
    {
        // Same check as PlayerController: if the index doesn't exist, use the first character
        Transform characters = transform.GetChild(0);
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);
        if (selectedCharacter < 0 || selectedCharacter >= characters.childCount)
            selectedCharacter = 0;

        animator = characters.GetChild(selectedCharacter).GetComponent<Animator>();
    }

void OnCollisionEnter2D(Collision2D collision)
    {
        int layer = LayerMask.NameToLayer("Floor");
        if (collision.gameObject.layer == layer)
        {
            if (snowTrailParticles != null) snowTrailParticles.Play();
            if (animator != null) animator.SetBool("isJumping", false);
        }
    }

void OnCollisionExit2D(Collision2D collision)
    {
        int layer = LayerMask.NameToLayer("Floor");
        if (collision.gameObject.layer == layer)
        {
            if (snowTrailParticles != null) snowTrailParticles.Stop();
            if (animator != null) animator.SetBool("isJumping", true);
        }
    }


}
