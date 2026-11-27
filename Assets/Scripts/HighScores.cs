using System.Collections.Generic;
using UnityEngine;

// Saves the 3 best scores (name + points) with PlayerPrefs, so they stay after closing the game.
public static class HighScores
{
    public const int MaxEntries = 3;

    public struct Entry
    {
        public string name;
        public int score;
    }

    public static List<Entry> Load()
    {
        List<Entry> entries = new List<Entry>();
        for (int i = 0; i < MaxEntries; i++)
        {
            if (!PlayerPrefs.HasKey("HighScoreName" + i)) break;
            entries.Add(new Entry
            {
                name = PlayerPrefs.GetString("HighScoreName" + i),
                score = PlayerPrefs.GetInt("HighScoreValue" + i)
            });
        }
        return entries;
    }

    // True if this score is good enough to enter the top 3
    public static bool IsHighScore(int score)
    {
        if (score <= 0) return false;
        List<Entry> entries = Load();
        return entries.Count < MaxEntries || score > entries[entries.Count - 1].score;
    }

    public static void Add(string playerName, int score)
    {
        List<Entry> entries = Load();
        entries.Add(new Entry { name = playerName, score = score });
        entries.Sort((a, b) => b.score.CompareTo(a.score)); // highest first

        for (int i = 0; i < MaxEntries; i++)
        {
            if (i < entries.Count)
            {
                PlayerPrefs.SetString("HighScoreName" + i, entries[i].name);
                PlayerPrefs.SetInt("HighScoreValue" + i, entries[i].score);
            }
        }
        PlayerPrefs.Save();
    }
}
