using UnityEngine;

namespace World
{
    [ExecuteInEditMode]
    public class SpriteSwapper : MonoBehaviour
    {
        public Sprite normalSprite;
        public Sprite otherWorldSprite;

        private SpriteRenderer _rendered;

        // Start is called before the first frame update
        void Start()
        {
            _rendered = gameObject.GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_rendered == null)
                return;

            var sprite = GlobalVariables.world switch
            {
                GlobalVariables.World.OtherDimention => otherWorldSprite,
                _ => normalSprite,
            };

            if (_rendered.sprite != sprite)
                _rendered.sprite = sprite;
        }
    }
}
