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
        //Debug.Log($"Collision detected with {col.gameObject.name} (tag:{col.tag}) by {gameObject.name}");

        if (col.tag?.Equals("player", System.StringComparison.OrdinalIgnoreCase) ?? false)
        {
            SwitchWorlds();
            //Debug.Log($"GlobalVariables.world: {GlobalVariables.world}");

            if (afterEffectPrefab != null)
                Instantiate(afterEffectPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    private void SwitchWorlds() =>
        GlobalVariables.world = GlobalVariables.world switch
        {
            GlobalVariables.World.NormalDimention => GlobalVariables.World.OtherDimention,
            _ => GlobalVariables.World.NormalDimention
        };
}
