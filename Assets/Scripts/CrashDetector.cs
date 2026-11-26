using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class CrashDetector : MonoBehaviour
{
    [SerializeField] private ParticleSystem crashParticles;
    [SerializeField] private AudioClip crashSound;
    AudioSource audioSource;
    bool hasCrashed;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        int layer = LayerMask.NameToLayer("Floor");

        if (other.gameObject.layer == layer)
        {
            Crash();
        }
    }

    // Public so other scripts (e.g. Enemy) can make the player crash
    public void Crash()
    {
        // Avoid playing the effects and reloading the scene more than once
        if (hasCrashed) return;
        hasCrashed = true;

        //Play Sound
        if (audioSource != null && crashSound != null)
            audioSource.PlayOneShot(crashSound);

        //Play Particles
        if (crashParticles != null)
            crashParticles.Play();

        //Reload Scene after 1.5 second
        Invoke(nameof(ReloadScene), 1.5f);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);    
    }

}
