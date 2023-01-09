using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour
{
    public string levelObjectNameContains = "level";

    public string soulsObjectNameContains = "souls";

    public string timeObjectNameContains = "time";

    private TMP_Text _levelsCompletedText;

    private TMP_Text _soulsCollectedText;

    private TMP_Text _timeSpentText;

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (NameContains(child, levelObjectNameContains))
                _levelsCompletedText = child.gameObject?.GetComponent<TMP_Text>();
            else if (NameContains(child, soulsObjectNameContains))
                _soulsCollectedText = child.gameObject?.GetComponent<TMP_Text>();
            else if (NameContains(child, timeObjectNameContains))
                _timeSpentText = child.gameObject?.GetComponent<TMP_Text>();
        }

        SetScores();
    }

    private bool NameContains(Transform child, string contains)
    {
        return child.name.Contains(contains, StringComparison.OrdinalIgnoreCase);
    }

    private void SetScores()
    {
        if (_levelsCompletedText != null)
            _levelsCompletedText.text = $"Levels completed: {GlobalVariables.levelsCompleted}";

        if (_soulsCollectedText != null)
            _soulsCollectedText.text = $"Souls collected: {GlobalVariables.totalSoulsCollected}";

        if (_timeSpentText != null)
        {
            var totalMinutes = GlobalVariables.TimeSpent().TotalMinutes;
            var minute = (int) totalMinutes;
            var second = (int) ((totalMinutes - minute) * 60);
            _timeSpentText.text = $"Time spent: {minute}min {second}s";
        }
    }
}
