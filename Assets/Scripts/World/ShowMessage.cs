using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShowMessage : MonoBehaviour
{
    public GameObject textBox;
    public bool wasTriggered = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (this.wasTriggered)
        {
            return;
        }

        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Triggered " + textBox.name);
            textBox.SetActive(true);
            this.wasTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Left " + textBox.name);
            textBox.SetActive(false);
        }
    }
}