using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{

    // Reference to the particle system
    [SerializeField] private ParticleSystem finishParticles;
    [SerializeField] private AudioClip finishSound; // Reference to the finish sound effect

    AudioSource audioSource; // Reference to the AudioSource component
    bool hasFinished; // The player has several colliders: only finish once

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
    }

    // This method is called when another collider enters the trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasFinished) return;

        if (other.CompareTag("Player"))
        {
            hasFinished = true;

            //Play Particles
            finishParticles.Play();

            //Play Sound
            audioSource.PlayOneShot(finishSound);

            // Time bonus: the faster, the more points
            if (LevelManager.Instance != null)
            {
                int bonus = LevelManager.Instance.FinishLevel();
                ScoreManager hud = FindAnyObjectByType<ScoreManager>();
                if (hud != null)
                {
                    string time = ScoreManager.FormatTime(GameSession.LevelTime);
                    hud.ShowMessage("¡Meta! " + time + "\nBonus de tiempo +" + bonus, 3f);
                }
            }

            // Load the next level after a short delay
            Invoke(nameof(NextLevel), 2.5f);

        }
    }
void NextLevel()
    {
        // Load the next scene in the build settings
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);


    }
    /// <summary>
    /// Reloads the current scene after a delay.
    /// </summary>
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
