using UnityEngine;

public class KillZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<GameEventHandler>().OnDeath();
        }
    }
}