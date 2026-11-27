using UnityEngine;

// Snowflake that gives points when the player touches it.
// It spins and floats up and down so it's easy to see.
[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    [SerializeField] private int points = 50;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private float spinSpeed = 60f;
    [SerializeField] private float floatHeight = 0.3f;
    [SerializeField] private float floatSpeed = 3f;

    Vector3 startPosition;
    float floatOffset;
    bool collected;

    void Start()
    {
        startPosition = transform.position;
        floatOffset = Random.Range(0f, 6f); // so they don't all move at the same time
    }

    void Update()
    {
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
        float y = Mathf.Sin(Time.time * floatSpeed + floatOffset) * floatHeight;
        transform.position = startPosition + Vector3.up * y;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (other.attachedRigidbody == null || !other.attachedRigidbody.CompareTag("Player")) return;

        collected = true;
        GameSession.AddPoints(points);
        GameSession.MarkCollected(name);

        ScoreManager hud = FindAnyObjectByType<ScoreManager>();
        if (hud != null) hud.ShowMessage("+" + points, 0.6f);

        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position);

        Destroy(gameObject);
    }
}
