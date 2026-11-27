using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class CrashDetector : MonoBehaviour
{
    [SerializeField] private ParticleSystem crashParticles;
    [SerializeField] private AudioClip crashSound;
    AudioSource audioSource;
    bool hasCrashed;
    bool gameOver;

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

        // In the bonus round a crash doesn't cost a life: it just ends the bonus
        if (BonusRound.Instance != null)
        {
            BonusRound.Instance.EndBonus();
            return;
        }

        // Lose a life (the score goes back to 0)
        bool livesLeft = GameSession.LoseLife();
        gameOver = !livesLeft;

        ScoreManager hud = FindAnyObjectByType<ScoreManager>();
        if (hud != null)
        {
            if (gameOver)
                hud.ShowMessage("GAME OVER\nVuelves al principio", 3f);
            else
                hud.ShowMessage("¡Has perdido una vida!\nTe quedan " + GameSession.Lives, 2f);
        }

        // Wait a bit so the player can read the message
        Invoke(nameof(ReloadScene), gameOver ? 3f : 2f);
    }

    void ReloadScene()
    {
        if (gameOver)
        {
            // No lives left: back to the beginning of the game
            GameSession.StartNewGame();
            SceneManager.LoadScene("Level1");
        }
        else
        {
            // Try the same level again
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
