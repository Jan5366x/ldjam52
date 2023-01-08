using UnityEngine;

public class PickupSoul : MonoBehaviour
{
    public GameObject afterEffectPrefab;

    public float afterEffectDuration = 1;

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log($"Collision detected with {col.gameObject.name} (tag:{col.tag}) by {gameObject.name}");

        if (col.tag?.Equals("player", System.StringComparison.OrdinalIgnoreCase) ?? false)
        {
            GlobalVariables.SoulPicked();
            //Debug.Log($"GlobalVariables.playerLives: {GlobalVariables.playerLives}");

            var afterEffect = Instantiate(afterEffectPrefab, transform.position, Quaternion.identity);
            Destroy(afterEffect, afterEffectDuration);

            Destroy(gameObject);
        }
    }
}
