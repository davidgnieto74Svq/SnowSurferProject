using UnityEngine;

// Static obstacle (e.g. a rock). If the player hits it, the player crashes.
// The player has to jump over it.
[RequireComponent(typeof(Collider2D))]
public class Obstacle : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        TryCrash(collision.rigidbody);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        TryCrash(other.attachedRigidbody);
    }

    void TryCrash(Rigidbody2D body)
    {
        if (body == null || !body.CompareTag("Player")) return;

        CrashDetector crashDetector = body.GetComponentInChildren<CrashDetector>();
        if (crashDetector != null)
        {
            crashDetector.Crash();
        }
    }
}
