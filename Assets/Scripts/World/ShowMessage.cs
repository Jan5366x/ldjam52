using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShowMessage : MonoBehaviour
{
    public GameObject textBox;
    
    public bool wasTriggered = false;
    
    public bool triggerOnce = true;

    private void OnTriggerEnter2D(Collider2D col)
    {
        TriggerEntered(col);
    }

    protected void TriggerEntered(Collider2D col)
    {
        if (triggerOnce && wasTriggered)
        {
            return;
        }

        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Triggered " + textBox.name);
            textBox.SetActive(true);
            wasTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        TriggerExited(col);
    }

    protected void TriggerExited(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Left " + textBox.name);
            textBox.SetActive(false);
        }
    }
}