using UnityEngine;
using TMPro;

// HUD: shows the total points, the level time (or the bonus round countdown)
// and big messages in the middle of the screen ("+50", "Time bonus"...).
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text messageText;

    float messageTimer;

    void Start()
    {
        if (messageText != null) messageText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (scoreText != null)
            scoreText.text = "Puntos: " + GameSession.Score;

        if (timeText != null)
        {
            if (BonusRound.Instance != null)
                timeText.text = "Bonus: " + Mathf.CeilToInt(BonusRound.Instance.TimeLeft) + " s";
            else
                timeText.text = "Tiempo: " + FormatTime(GameSession.LevelTime);
        }

        // Hide the message after a while (unscaled time, so it also works on the pause screen)
        if (messageText != null && messageText.gameObject.activeSelf)
        {
            messageTimer -= Time.unscaledDeltaTime;
            if (messageTimer <= 0f) messageText.gameObject.SetActive(false);
        }
    }

    public void AddPoints(int points)
    {
        GameSession.AddPoints(points);
    }

    public void ShowMessage(string text, float seconds)
    {
        if (messageText == null) return;
        messageText.text = text;
        messageText.gameObject.SetActive(true);
        messageTimer = seconds;
    }

    public static string FormatTime(float seconds)
    {
        int minutes = (int)(seconds / 60f);
        float rest = seconds - minutes * 60f;
        return minutes + ":" + rest.ToString("00.0");
    }
}
