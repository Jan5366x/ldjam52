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

        private Collider2D _collider;

        private void Start()
        {
            _render = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            _render.enabled = IsInOtherWorld();
            _collider.enabled = IsInOtherWorld();

            if (IsInOtherWorld())
            {
                _render.color = new Color(1, 1, 1, PortalVisibility());
            }
        }

        private static float PortalVisibility() =>
            Mathf.Min(1.0f, 0.1f + (0.1f * GlobalVariables.soulsCollected));

        private static bool IsInOtherWorld()
        {
            return GlobalVariables.world == GlobalVariables.World.OtherDimention;
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
