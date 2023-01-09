using UnityEngine;

namespace Effects
{
    public class SinMovementEffect : MonoBehaviour
    {
        public float amplifier;

        public float frequency;

        public MovementDirection direction;

        private Vector3 _initPosition;

        void Start()
        {
            _initPosition = transform.position;
        }

        void Update()
        {
            var delta = Mathf.Sin(Time.time * frequency) * amplifier;
            var yDelta = direction == MovementDirection.UpDown ? delta : 0f;
            var xDelta = direction == MovementDirection.LeftRight ? delta : 0f;

            transform.position = new Vector3(_initPosition.x + xDelta, _initPosition.y + yDelta, _initPosition.z);
        }

        public enum MovementDirection
        {
            UpDown,
            LeftRight
        }
    }
}

