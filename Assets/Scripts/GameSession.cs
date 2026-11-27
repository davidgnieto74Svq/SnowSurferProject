using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Keeps the game data between scenes: total score, level time and the bonus round.
// It's a static class, so it isn't attached to any GameObject and it survives scene changes.
public static class GameSession
{
    public const int MaxLives = 3;

    public static int Score { get; private set; }
    public static int Lives { get; private set; } = MaxLives;
    public static float LevelTime { get; set; }
    public static bool ReturningFromBonus { get; private set; }

    static int scoreAtLevelStart;
    static string currentLevel = "";

    // Where to go back after the bonus round
    static string bonusReturnScene;
    static Vector3 bonusReturnPosition;
    static float bonusReturnTime;

    // Snowflakes and bonus stars already taken in this level (so they don't appear again after the bonus round)
    static readonly HashSet<string> collectedItems = new HashSet<string>();

    // Reset everything when Play starts (also works if "Domain Reload" is disabled in the editor)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetOnPlay()
    {
        StartNewGame();
    }

    public static void StartNewGame()
    {
        Score = 0;
        Lives = MaxLives;
        scoreAtLevelStart = 0;
        currentLevel = "";
        LevelTime = 0f;
        ReturningFromBonus = false;
        collectedItems.Clear();
    }

    public static void AddPoints(int points)
    {
        Score += points;
    }

    // The player crashed: one life less and the score goes back to 0.
    // Returns true if there are lives left (false = game over).
    public static bool LoseLife()
    {
        Lives = Mathf.Max(0, Lives - 1);
        Score = 0;
        scoreAtLevelStart = 0;
        return Lives > 0;
    }

    // Called when a level scene starts.
    // If it's the same level again (the player crashed), the points won in that level are lost.
    public static void BeginLevel(string levelName)
    {
        if (levelName == currentLevel)
        {
            Score = scoreAtLevelStart;
        }
        else
        {
            currentLevel = levelName;
            scoreAtLevelStart = Score;
        }

        LevelTime = 0f;
        collectedItems.Clear();
    }

    public static bool IsCollected(string id)
    {
        return collectedItems.Contains(id);
    }

    public static void MarkCollected(string id)
    {
        collectedItems.Add(id);
    }

    public static void GoToBonus(string bonusScene, Vector3 returnPosition)
    {
        bonusReturnScene = SceneManager.GetActiveScene().name;
        bonusReturnPosition = returnPosition;
        bonusReturnTime = LevelTime;
        SceneManager.LoadScene(bonusScene);
    }

    public static void ReturnFromBonus()
    {
        ReturningFromBonus = true;
        SceneManager.LoadScene(bonusReturnScene);
    }

    // Used by the LevelManager when the level loads after the bonus round
    public static bool TryGetBonusReturn(out Vector3 position)
    {
        position = bonusReturnPosition;
        if (!ReturningFromBonus) return false;

        ReturningFromBonus = false;
        LevelTime = bonusReturnTime;
        return true;
    }
}
