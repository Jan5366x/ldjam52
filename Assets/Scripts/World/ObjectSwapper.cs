using System;
using UnityEngine;

namespace World
{
    [ExecuteInEditMode]
    public class ObjectSwapper : MonoBehaviour
    {
        public string normalObjectNameContains = "normal";

        public string otherObjectNameContains = "other";

        private GameObject _normalObject;

        private GameObject _otherObject;

        private GlobalVariables.World _worldState;

        // Start is called before the first frame update
        void Start()
        {
            _worldState = GlobalVariables.world;

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

        // Update is called once per frame
        void Update()
        {
            if (_worldState == GlobalVariables.world)
                return;

            UpdateSwitch();
        }

        private void UpdateSwitch()
        {
            var activateMainWorld = GlobalVariables.world switch
            {
                GlobalVariables.World.OtherDimention => false,
                _ => true,
            };

            if (_normalObject)
                _normalObject.SetActive(activateMainWorld);
            if (_otherObject)
                _otherObject.SetActive(!activateMainWorld);
        
            _worldState = GlobalVariables.world;
        }
    }
}
