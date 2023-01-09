using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject soulsPanel;

    public GameObject heartPanel;

    public Sprite soulCollected;

    public Sprite heartEmpty;

    public Sprite heartCollected;

    private readonly List<GameObject> souls = new();

    private readonly List<GameObject> hearts = new();

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < soulsPanel.transform.childCount; i++)
        {
            souls.Add(soulsPanel.transform.GetChild(i).gameObject);
        }

        for (var i = 0; i < heartPanel.transform.childCount; i++)
        {
            hearts.Add(heartPanel.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < GlobalVariables.soulsCollected; i++)
        {
            if (i >= souls.Count) return;

            var rawImage = souls[i].GetComponent<Image>();
            rawImage.sprite = soulCollected;
        }

        if (GlobalVariables.fillingHearts ||
            GlobalVariables.world == GlobalVariables.World.NormalDimention)
        {
            for (var i = 0; i < GlobalVariables.totalHearts; i++)
            {
                if (i >= hearts.Count) return;

                var rawImage = hearts[i].GetComponent<Image>();
                rawImage.sprite = i < GlobalVariables.hearts ? heartCollected : heartEmpty;
            }
        }
    }
}
