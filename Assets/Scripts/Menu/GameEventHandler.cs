using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEventHandler : MonoBehaviour
{
    public string nextLevel = "World1";
    public bool isFinal = false;

    public void OnDeath()
    {
        LoadScene("DeathScene");
    }

    public void OnStart()
    {
        GlobalVariables.ResetGame();
        LoadScene("World1");
    }

    public void OnVictory()
    {
        if (isFinal || nextLevel == "")
        {
            OnFinalVictory();
        }
        else
        {
            GlobalVariables.lastLevel = nextLevel;
            LoadScene(nextLevel);
        }
    }

    public static void OnFinalVictory()
    {
        LoadScene("WinScene");
    }

    public void RetryLastLevel()
    {
        RetryLastLevelStatic();
    }

    public static void RetryLastLevelStatic()
    {
        GlobalVariables.ResetLevel();
        LoadScene(GlobalVariables.lastLevel);
    }

    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
