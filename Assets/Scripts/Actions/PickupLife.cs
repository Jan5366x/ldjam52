using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PickupLife : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log($"Collision detected with {col.gameObject.name} (tag:{col.tag}) by {gameObject.name}");

        if (col.tag?.Equals("player", System.StringComparison.OrdinalIgnoreCase) ?? false)
        {
            GlobalVariables.playerLives += 1;
            Debug.Log($"GlobalVariables.playerLives: {GlobalVariables.playerLives}");
        }
    }
}
