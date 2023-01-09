using System;
using UnityEngine;

namespace World
{
    [ExecuteInEditMode]
    public class UIObjectSwapper : MonoBehaviour
    {
        public string normalObjectNameContains = "normal";

        public string otherObjectNameContains = "other";

        private GameObject _normalObject;

        private GameObject _otherObject;

        private GlobalVariables.World _worldState;

        private bool _fillingHeart;

        // Start is called before the first frame update
        void Start()
        {
            _worldState = GlobalVariables.world;
            _fillingHeart = GlobalVariables.fillingHearts;

            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (NameContains(child, normalObjectNameContains))
                    _normalObject = child.gameObject;
                else if (NameContains(child, otherObjectNameContains))
                    _otherObject = child.gameObject;
            }

            bool NameContains(Transform child, string contains)
            {
                return child.name.Contains(contains, StringComparison.OrdinalIgnoreCase);
            }
            UpdateSwitch();
        }

        void Update()
        {
            if (_worldState == GlobalVariables.world &&
                _fillingHeart == GlobalVariables.fillingHearts)
                return;

            UpdateSwitch();
        }

        private void UpdateSwitch()
        {
            var showHearts = GlobalVariables.world switch
            {
                _ when GlobalVariables.fillingHearts => true,
                GlobalVariables.World.OtherDimention => false,
                _ => true,
            };

            if (_normalObject)
                _normalObject.SetActive(showHearts);
            if (_otherObject)
                _otherObject.SetActive(!showHearts);
        
            _worldState = GlobalVariables.world;
            _fillingHeart = GlobalVariables.fillingHearts;
        }
    }
}
