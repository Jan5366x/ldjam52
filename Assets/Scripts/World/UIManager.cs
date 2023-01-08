using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject soulsPanel;

    private readonly List<GameObject> souls = new();

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < soulsPanel.transform.childCount; i++)
        {
            souls.Add(soulsPanel.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < GlobalVariables.soulsCollected; i++)
        {
            if (i >= souls.Count) return;

            var rawImage = souls[i].GetComponent<RawImage>();
            rawImage.color = Color.white;
        }
    }
}
