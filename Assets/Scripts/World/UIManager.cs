using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject livesPanel;

    private readonly List<GameObject> lives = new();

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < livesPanel.transform.childCount; i++)
        {
            lives.Add(livesPanel.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < GlobalVariables.playerLives; i++)
        {
            if (i >= lives.Count) return;

            var rawImage = lives[i].GetComponent<RawImage>();
            rawImage.color = Color.white;
        }
    }
}
