using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class TextTrigger : MonoBehaviour
{
    public bool triggerOnce = true;

    public int hidingDelayInSeconds = 5;

    public TextMeshProUGUI textField;

    public string text;

    private bool triggered = false;

    private GameObject textParent;

    // Start is called before the first frame update
    void Start()
    {
        textParent = textField.gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (triggerOnce && triggered)
            return;

        if (col.tag?.Equals("player", System.StringComparison.OrdinalIgnoreCase) ?? false)
        {
            Debug.Log($"Tell player: {text}");
            textField.text = text;
            textParent.SetActive(true);
            StartCoroutine(HideMessageInSeconds());
            triggered = true;
        }
    }

    IEnumerator HideMessageInSeconds()
    {
        yield return new WaitForSeconds(hidingDelayInSeconds);

        textField.text = string.Empty;
        textParent.SetActive(false);
    }
}
