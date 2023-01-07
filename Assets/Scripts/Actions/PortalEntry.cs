using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalEntry : MonoBehaviour
{
    [AllowsNull]
    public GameObject afterEffectPrefab;

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
            SwitchWorld();
            Debug.Log($"GlobalVariables.world: {GlobalVariables.world}");

            if (afterEffectPrefab != null)
                Instantiate(afterEffectPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    private void SwitchWorld()
    {
        GlobalVariables.world = 
            GlobalVariables.world == GlobalVariables.World.Main 
            ? GlobalVariables.World.UpsideDown 
            : GlobalVariables.World.Main;
    }
}
