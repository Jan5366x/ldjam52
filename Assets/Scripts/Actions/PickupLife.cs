using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class PickupLife : MonoBehaviour
{
    public GameObject afterEffectPrefab;

    public float afterEffectDuration = 1;

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
            IncreaseLives();
            //Debug.Log($"GlobalVariables.playerLives: {GlobalVariables.playerLives}");

            var afterEffect = Instantiate(afterEffectPrefab, transform.position, Quaternion.identity);
            Destroy(afterEffect, afterEffectDuration);

            Destroy(gameObject);
        }
    }

    private static void IncreaseLives()
    {
        if (GlobalVariables.playerLives + 1 <= GlobalVariables.playerMaxLives)
            GlobalVariables.playerLives++;
    }

    private void OnDestroy()
    {
    }
}
