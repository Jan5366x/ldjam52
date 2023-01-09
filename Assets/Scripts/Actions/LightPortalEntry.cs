using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Actions
{
    public class LightPortalEntry : MonoBehaviour
    {
        private GlobalVariables.World nextWorld = GlobalVariables.World.OtherDimention;

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
                GlobalVariables.world = nextWorld;
                //Debug.Log($"GlobalVariables.world: {GlobalVariables.world}");

                if (afterEffectPrefab != null)
                    Instantiate(afterEffectPrefab, transform.position, Quaternion.identity);
                
                RandomizedSound.Play(col.transform, RandomizedSound.PORTAL);

                gameObject.SetActive(false);
            }
        }
    }
}
