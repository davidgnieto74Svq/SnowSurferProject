using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString("00000");
    }

}
