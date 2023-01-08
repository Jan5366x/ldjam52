using UnityEngine;

public class KillZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GameObject.FindWithTag("MainCamera").GetComponent<GameEventHandler>().OnDeath();
        }
    }
}