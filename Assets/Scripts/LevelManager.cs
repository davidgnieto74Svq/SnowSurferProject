using UnityEngine;
using UnityEngine.SceneManagement;

// One per level. Counts the level time, gives the time bonus at the finish line
// and puts the player back in place when coming back from the bonus round.
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private Transform player;
    [SerializeField] private float parTime = 40f;            // Finish faster than this to get a time bonus
    [SerializeField] private int pointsPerSecondSaved = 50;  // Bonus points for each second under parTime

    bool finished;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (GameSession.TryGetBonusReturn(out Vector3 returnPosition))
        {
            // Back from the bonus round: continue from the same place, with the same time and points
            player.position = returnPosition;
            player.rotation = Quaternion.identity;
        }
        else
        {
            GameSession.BeginLevel(SceneManager.GetActiveScene().name);
        }

        // Remove the snowflakes and bonus stars already taken
        foreach (Collectible item in FindObjectsByType<Collectible>())
        {
            if (GameSession.IsCollected(item.name)) Destroy(item.gameObject);
        }
        foreach (BonusPortal portal in FindObjectsByType<BonusPortal>())
        {
            if (GameSession.IsCollected(portal.name)) Destroy(portal.gameObject);
        }
    }

    void Update()
    {
        if (!finished)
        {
            GameSession.LevelTime += Time.deltaTime; // Time.deltaTime is 0 while the game is paused
        }
    }

    // Called by the finish line. Returns the time bonus.
    public int FinishLevel()
    {
        if (finished) return 0;
        finished = true;

        float secondsSaved = parTime - GameSession.LevelTime;
        int bonus = Mathf.Max(0, Mathf.RoundToInt(secondsSaved * pointsPerSecondSaved));
        GameSession.AddPoints(bonus);
        return bonus;
    }

    public float ParTime => parTime;
}
