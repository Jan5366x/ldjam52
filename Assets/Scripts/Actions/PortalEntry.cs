using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private bool IsSameWorld()
        {
            return GlobalVariables.world == nextWorld;
        }

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
                    StartCoroutine(LoadNextSceneAsync());

                //Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }

        IEnumerator LoadNextSceneAsync()
        {
            //Debug.Log($"Loading next scene: {nextSceneName}");
            var loadOperation = SceneManager.LoadSceneAsync(nextSceneName);
            // Do we need loading screen?

            while (!loadOperation.isDone)
            {
                yield return null;
            }

            //Debug.Log($"Loaded next scene: {nextSceneName}");
        }
    }
}
