using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Actions
{
    public class PortalEntry : MonoBehaviour
    {
        public GlobalVariables.World nextWorld = GlobalVariables.World.OtherDimention;

        [AllowsNull]
        public string nextSceneName;

        [AllowsNull]
        public GameObject afterEffectPrefab;

        private SpriteRenderer _render;
        private Collider2D _collider;

        void Start()
        {
            _render = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
        }

        void Update()
        {
            _render.enabled = !IsSameWorld();
            _collider.enabled = !IsSameWorld();
        }

        private bool IsSameWorld() => GlobalVariables.world == nextWorld;

        void OnTriggerEnter2D(Collider2D col)
        {
            //Debug.Log($"Collision detected with {col.gameObject.name} (tag:{col.tag}) by {gameObject.name}");

            if ((col.tag?.Equals("player", StringComparison.OrdinalIgnoreCase) ?? false))
            {
                if (GlobalVariables.IsLevelCompletedNextLevel() && !string.IsNullOrWhiteSpace(nextSceneName))
                {
                    Debug.Log($"Level is not completed.");
                }

                GlobalVariables.world = nextWorld;
                //Debug.Log($"GlobalVariables.world: {GlobalVariables.world}");

                if (afterEffectPrefab != null)
                    Instantiate(afterEffectPrefab, transform.position, Quaternion.identity);
                
                if (!string.IsNullOrWhiteSpace(nextSceneName))
                {
                    GameObject.FindWithTag("Player").GetComponent<GameEventHandler>().OnVictory();
                }

                //Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }
    }
}
