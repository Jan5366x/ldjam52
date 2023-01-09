using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public Transform legBack;
        public Transform legFront;

        public float timeSinceLastMove;
        public float timeSinceLastJump;
        public float timeSinceLastLand;
        public bool touchesGroundAtStart;
        public float prevAngle;

        public const float AccelGround = 20;
        public const float AccelAir = 10;
        public const float MaxSpeedXGround = 10;
        public const float MaxSpeedXAir = 6;
        public const float MaxStepSize = 0.5f;

        public enum State
        {
            IDLE,
            WALK,
            JUMP,
            FALLING,
            LANDING
        }

        public State state;

        void Start()
        {
            legBack = transform.Find("LegBack");
            legFront = transform.Find("LegFront");
            state = State.IDLE;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            timeSinceLastMove += Time.deltaTime;
            timeSinceLastJump += Time.deltaTime;
            timeSinceLastLand += Time.deltaTime;

            bool idle = true;
            bool falling = false;

            var legBackCenter = GetColliderCenter(legBack);
            var legFrontCenter = GetColliderCenter(legFront);

            var legBackGround = GetGroundPosition(legBackCenter);
            var legFrontGround = GetGroundPosition(legFrontCenter);

            Debug.DrawLine(legBackGround, legFrontGround, Color.green);

            float angle = Mathf.Atan2(legFrontGround.y - legBackGround.y, legFrontGround.x - legBackGround.x);
            if (Vector2.Distance(legBackGround, Vector2.zero) < 0.001 ||
                Vector2.Distance(legFrontGround, Vector2.zero) < 0.001)
            {
                angle = 0;
            }

            angle *= Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, -45, 45);
            
            Debug.DrawLine(transform.position,
                transform.position +
                new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0),
                Color.magenta);

            if (Mathf.Abs(angle - prevAngle) > 10)
            {
                angle = Mathf.LerpAngle(prevAngle, angle, 0.01f);
            }
            else
            {
             prevAngle = angle;
            }


            Debug.DrawLine(transform.position,
                transform.position +
                new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0),
                Color.blue);

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            touchesGroundAtStart = touchesGround(legBackCenter) || touchesGround(legFrontCenter);

            var rigidBody = GetComponent<Rigidbody2D>();
            if (touchesGroundAtStart)
            {
                if (state == State.FALLING)
                {
                    state = State.LANDING;
                    timeSinceLastLand = 0;
                }

                if (Input.GetButton("Jump") && timeSinceLastJump > 0.25)
                {
                    idle = false;
                    state = State.JUMP;
                    timeSinceLastJump = 0;
                    rigidBody.AddForce(Vector2.up * 400);
                }

                rigidBody.drag = 0.95f;
            }
            else
            {
                rigidBody.drag = 0f;
                timeSinceLastMove = 0;
                falling = true;
                idle = false;
                if (timeSinceLastJump > 0.25 && timeSinceLastLand > 0.25)
                {
                    state = State.FALLING;
                }
            }

            float maxSpeed = touchesGroundAtStart ? MaxSpeedXGround : MaxSpeedXAir;
            float accel = touchesGroundAtStart ? AccelGround : AccelAir;


            if (Input.GetAxis("Horizontal") > 0.1)
            {
                timeSinceLastMove = 0;
                idle = false;
                if (state == State.IDLE || (state == State.LANDING && timeSinceLastLand > 0.25))
                {
                    state = State.WALK;
                }

                GetComponent<SpriteRenderer>().flipX = false;
                if (rigidBody.velocity.x < maxSpeed)
                {
                    rigidBody.AddForce(new Vector2(accel * Mathf.Cos(Mathf.Deg2Rad * angle),
                        accel * Mathf.Sin(Mathf.Deg2Rad * angle)));
                }
            }
            else if (Input.GetAxis("Horizontal") < -0.1)
            {
                timeSinceLastMove = 0;
                idle = false;
                if (state == State.IDLE || (state == State.LANDING && timeSinceLastLand > 0.25))
                {
                    state = State.WALK;
                }

                GetComponent<SpriteRenderer>().flipX = true;
                if (rigidBody.velocity.x > -maxSpeed)
                {
                    rigidBody.AddForce(new Vector2(-accel * Mathf.Cos(Mathf.Deg2Rad * angle),
                        -accel * Mathf.Sin(Mathf.Deg2Rad * angle)));
                }
            }

            if (idle && !falling && timeSinceLastMove > 0.25 && timeSinceLastJump > 0.25 && timeSinceLastLand > 0.25)
            {
                state = State.IDLE;
            }

            var animator = GetComponent<Animator>();
            AnimationHelper.SetParameter(animator, "state", (int) state);

            for (int i = 0; i < (int) state; i++)
            {
                Debug.DrawLine(transform.position, transform.position + Vector3.right * i, Color.blue);
            }
        }


        bool touchesGround(Vector2 pos)
        {
            foreach (var raycastHit2D in Physics2D.RaycastAll(pos, Vector2.down, 1))
            {
                if (raycastHit2D.transform.CompareTag("Scene"))
                {
                    return raycastHit2D.distance < MaxStepSize;
                }
            }

            return false;
        }

        Vector2 GetGroundPosition(Vector2 pos)
        {
            Vector2 tryDown = GetRayCastDownHighestHit(new Vector2(pos.x, pos.y + 1));
            if (Vector2.Distance(Vector2.zero, tryDown) <
                0.01) // Nothing below. Try checking from the top and teleport there
            {
                return GetRayCastDownHighestHit(new Vector2(pos.x, 999));
            }

            return tryDown;
        }

        Vector2 GetRayCastDownHighestHit(Vector2 pos)
        {
            Debug.DrawRay(pos, Vector2.down, Color.black);
            float maxHeight = -99999;
            Vector2? maxObject = null;
            foreach (var raycastHit2D in Physics2D.RaycastAll(pos, Vector2.down))
            {
                if (raycastHit2D.transform.CompareTag("Scene"))
                {
                    float height = raycastHit2D.point.y;
                    if (height > maxHeight)
                    {
                        maxHeight = height;
                        maxObject = raycastHit2D.point;
                    }
                }
            }

            return (maxObject == null) ? Vector2.zero : maxObject.Value;
        }

        Vector2 GetColliderCenter(Transform transform)
        {
            return (Vector2) transform.GetComponent<Collider2D>().bounds.center;
        }
    }
}