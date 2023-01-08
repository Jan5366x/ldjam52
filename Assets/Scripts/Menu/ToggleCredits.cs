using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleCredits : MonoBehaviour
{
    public GameObject howToPlay;
    public GameObject credits;
    public Button toggleButton;

    private bool showCredits = false;

    public void toggleCredits()
    {
        showCredits = !showCredits;
        howToPlay.SetActive(!showCredits);
        credits.SetActive(showCredits);

        toggleButton.GetComponentInChildren<TMP_Text>().text = showCredits ? "How to play" : "Credits";
    }
}
