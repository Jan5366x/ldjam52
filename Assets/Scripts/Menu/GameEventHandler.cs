using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEventHandler : MonoBehaviour
{
    public static string lastLevel;
    public string nextLevel = "World1";
    public bool isFinal = false;

    public void OnDeath()
    {
        LoadScene("DeathScene");
    }

    public void OnStart()
    {
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
            lastLevel = nextLevel;
            LoadScene(nextLevel);
        }
    }

    public static void OnFinalVictory()
    {
        LoadScene("WinScene");
    }

    public void RetryLastLevel()
    {
        LoadScene(lastLevel);
    }

    public static void RetryLastLevelStatic()
    {
        LoadScene(lastLevel);
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
