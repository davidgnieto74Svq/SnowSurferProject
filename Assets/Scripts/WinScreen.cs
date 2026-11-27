using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Final screen: shows the total score, asks for the player's name if it's a top 3 score
// and shows the table with the 3 best players.
public class WinScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text highScoresText;
    [SerializeField] private GameObject nameEntry;        // Panel with the input field and the save button
    [SerializeField] private TMP_InputField nameInput;

    bool saved;

    void Start()
    {
        finalScoreText.text = "Puntos totales: " + GameSession.Score;

        bool isHighScore = HighScores.IsHighScore(GameSession.Score);
        nameEntry.SetActive(isHighScore);
        if (isHighScore)
        {
            nameInput.text = "";
            nameInput.Select();
            nameInput.ActivateInputField();
        }

        ShowHighScores();
    }

    // Called by the "Guardar" button (and when pressing Enter in the input field)
    public void SaveName()
    {
        if (saved) return;
        saved = true;

        string playerName = nameInput.text.Trim();
        if (playerName.Length == 0) playerName = "Jugador";
        if (playerName.Length > 12) playerName = playerName.Substring(0, 12);

        HighScores.Add(playerName, GameSession.Score);
        nameEntry.SetActive(false);
        ShowHighScores();
    }

    void ShowHighScores()
    {
        List<HighScores.Entry> entries = HighScores.Load();
        string text = "MEJORES PUNTUACIONES\n";
        for (int i = 0; i < HighScores.MaxEntries; i++)
        {
            if (i < entries.Count)
                text += "\n" + (i + 1) + ". " + entries[i].name + "   " + entries[i].score;
            else
                text += "\n" + (i + 1) + ". ---";
        }
        highScoresText.text = text;
    }
}
