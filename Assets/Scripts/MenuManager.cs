using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // "Play" button of the main menu: first show the screen that explains the game
    public void PlayGame()
    {
         SceneManager.LoadScene("HowToPlay");
    }

    // "Let's play" button of the HowToPlay screen
    public void StartGame()
    {
         GameSession.StartNewGame(); // New game: score starts at 0
         SceneManager.LoadScene("Level1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void GoToCharacters()
    {
        SceneManager.LoadScene("Characters");
    }

    public void GoToControls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SelectCharacter(int characterIndex)
    {
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        GoToMenu();
    }

}
