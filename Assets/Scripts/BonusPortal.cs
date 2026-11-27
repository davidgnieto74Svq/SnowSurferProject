using UnityEngine;

// Golden star: when the player touches it, it goes to the bonus round scene.
// After the bonus round the player comes back to this point of the level.
[RequireComponent(typeof(Collider2D))]
public class BonusPortal : MonoBehaviour
{
    [SerializeField] private string bonusScene = "Bonus";
    [SerializeField] private float pulseSpeed = 4f;
    [SerializeField] private float pulseAmount = 0.15f;
    [SerializeField] private Vector3 returnOffset = new Vector3(4f, 1.5f, 0f); // Where the player appears when coming back

    Vector3 startScale;
    bool used;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        // Pulse so it's easy to see
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = startScale * pulse;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;
        if (other.attachedRigidbody == null || !other.attachedRigidbody.CompareTag("Player")) return;

        used = true;
        GameSession.MarkCollected(name);
        GameSession.GoToBonus(bonusScene, transform.position + returnOffset);
    }
}
