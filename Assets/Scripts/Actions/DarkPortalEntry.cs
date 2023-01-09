using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Actions
{
    public class DarkPortalEntry : MonoBehaviour
    {
        public string nextSceneName;

        [AllowsNull]
        public GameObject afterEffectPrefab;

        private SpriteRenderer _render;

        private void Start()
        {
            _render = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            _render.color =
                GlobalVariables.CanLevelBeCompleted()
                ? new Color(1, 1, 1, 1)
                : new Color(1, 1, 1, 0.2f);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            //Debug.Log($"Collision detected with {col.gameObject.name} (tag:{col.tag}) by {gameObject.name}");

            if (!(col.tag?.Equals("player", StringComparison.OrdinalIgnoreCase) ?? false) ||
                string.IsNullOrWhiteSpace(nextSceneName) ||
                !GlobalVariables.CanLevelBeCompleted())
                return;

            if (afterEffectPrefab != null)
                Instantiate(afterEffectPrefab, transform.position, Quaternion.identity);

            GameObject.FindWithTag("Player").GetComponent<GameEventHandler>().OnVictory();
        }
    }
}
