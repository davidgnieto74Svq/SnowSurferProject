using UnityEngine;

// Bonus round: the player has a few seconds to collect as many snowflakes as possible.
// When the time is over (or the player crashes) it goes back to the level.
public class BonusRound : MonoBehaviour
{
    public static BonusRound Instance { get; private set; }

    [SerializeField] private float duration = 15f;

    public float TimeLeft { get; private set; }
    public int PointsWon => GameSession.Score - pointsAtStart;

    int pointsAtStart;
    bool ended;
    ScoreManager hud;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        TimeLeft = duration;
        pointsAtStart = GameSession.Score;
        hud = FindAnyObjectByType<ScoreManager>();
        if (hud != null) hud.ShowMessage("¡BONUS!\nRecoge los copos", 2.5f);
    }

    void Update()
    {
        if (ended) return;

        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= 0f)
        {
            TimeLeft = 0f;
            EndBonus();
        }
    }

    public void EndBonus()
    {
        if (ended) return;
        ended = true;

        if (hud != null) hud.ShowMessage("Bonus: +" + PointsWon, 2f);
        Invoke(nameof(GoBack), 2f);
    }

    void GoBack()
    {
        GameSession.ReturnFromBonus();
    }
}
