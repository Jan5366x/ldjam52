using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    public Transform legBack;
    public Transform legFront;

    public Vector2 accelleration;
    public Vector2 velocity;
    public float timeSinceLastMove;
    public float timeSinceLastJump;
    public float timeSinceLastLand;
    public bool touchesGroundAtStart;

    public const float maxSpeedX = 9;
    public const float maxSpeedY = 9;
    public const float maxStepSize = 0.5f;
    public const float closeToGround = 1f;

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
    void Update()
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

        float angle = Mathf.Atan2(legFrontGround.y - legBackGround.y, legFrontGround.x - legBackGround.x);
        if (Vector2.Distance(legBackGround, Vector2.zero) < 0.001 ||
            Vector2.Distance(legFrontGround, Vector2.zero) < 0.001)
        {
            angle = 0;
        }

        angle = Mathf.Clamp(angle, -45, 45);
        
        transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.forward);

        touchesGroundAtStart = touchesGround(legBackCenter) || touchesGround(legFrontCenter);

        var rigidBody = GetComponent<Rigidbody2D>();
        if (touchesGroundAtStart)
        {
            if (state == State.FALLING)
            {
                state = State.LANDING;
                timeSinceLastLand = 0;
            }
            if (Input.GetAxis("Horizontal") > 0.01)
            {
                accelleration.x = 10;
                timeSinceLastMove = 0;
                idle = false;
                if (timeSinceLastLand > 0.25 && timeSinceLastJump > 0.25)
                {
                    state = State.WALK;
                }

                GetComponent<SpriteRenderer>().flipX = false;
                if (Mathf.Abs(rigidBody.velocity.x) < maxSpeedX)
                {
                    rigidBody.AddForce(new Vector2(20 * Mathf.Cos(angle), 20 * Mathf.Sin(angle)));
                }
            }
            else if (Input.GetAxis("Horizontal") < -0.01)
            {
                accelleration.x = -10;
                timeSinceLastMove = 0;
                idle = false;
                if (timeSinceLastLand > 0.25)
                {
                    state = State.WALK;
                }

                GetComponent<SpriteRenderer>().flipX = true;
                if (Mathf.Abs(rigidBody.velocity.x) < maxSpeedX)
                {
                    rigidBody.AddForce(new Vector2(-20 * Mathf.Cos(angle), -20 * Mathf.Sin(angle)));
                }
            }
            else
            {
                accelleration.x = 0;
                velocity.y = -1;
            }

            if (Input.GetButton("Jump") && timeSinceLastJump > 0.25)
            {
                accelleration.y = 450;
                velocity.y = 0;
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

        if (idle && !falling && timeSinceLastMove > 0.25 && timeSinceLastJump > 0.25 && timeSinceLastLand > 0.25)
        {
            state = State.IDLE; 
        }
        
        var animator = GetComponent<Animator>();
        AnimationHelper.SetParameter(animator, "state", (int) state);


        /*var oldOffset = (Vector2) transform.position - Vector2.Lerp(legBackCenter, legFrontCenter, 0.5f);
        Debug.DrawLine(transform.position, transform.position - (Vector3) oldOffset, Color.yellow);
        accelleration = Vector2.zero;
        timeSinceLastMove += Time.deltaTime;
        timeSinceLastJump += Time.deltaTime;
        timeSinceLastLand += Time.deltaTime;

        var animator = GetComponent<Animator>();
        AnimationHelper.SetParameter(animator, AnimationHelper.ANIM_IDLE, false);
        AnimationHelper.SetParameter(animator, AnimationHelper.ANIM_WALK, false);

        bool idle = true;
        bool falling = false;

        touchesGroundAtStart = touchesGround(legBackCenter) || touchesGround(legFrontCenter);
        if (touchesGroundAtStart)
        {

        }
        else
        {
            timeSinceLastMove = 0;
            falling = true;
            idle = false;
            if (timeSinceLastJump > 0.25 && timeSinceLastLand > 0.25)
            {
                state = State.FALLING;
            }
        }

        accelleration.y -= 10;
        velocity.x = (touchesGroundAtStart || (state == State.FALLING && timeSinceLastJump < 5))
            ? velocity.x + accelleration.x * Time.deltaTime
            : velocity.x;
        if (timeSinceLastMove > 0.01 || (state == State.FALLING && timeSinceLastJump > 5))
        {
            velocity.x -= (velocity.x * Mathf.Min(1f, Mathf.Exp(timeSinceLastMove - 1)));
        }

        if (Mathf.Abs(velocity.x) < 0.1)
        {
            velocity.x = 0;
        }

        velocity.x = Mathf.Clamp(velocity.x, -maxSpeedX, maxSpeedX);
        velocity.y = Mathf.Clamp(velocity.y + accelleration.y * Time.deltaTime,
            (touchesGroundAtStart) ? -1 : -maxSpeedY, maxSpeedY);

        var plannedMovement = velocity * Time.deltaTime;

        Debug.DrawLine(transform.position, transform.position + (Vector3) plannedMovement, Color.red);

        Vector2 groundBack = GetGroundPosition(legBackCenter + plannedMovement);
        Vector2 groundFront = GetGroundPosition(legFrontCenter + plannedMovement);

        Debug.DrawLine(transform.position, groundBack, Color.green);
        Debug.DrawLine(transform.position, groundFront, Color.blue);

        var distanceGroundBack = Vector2.Distance(legBackCenter, groundBack);
        var distanceGroundFront = Vector2.Distance(legFrontCenter, groundFront);


        bool landing = false;
        var targetPositionBack = legBackCenter + plannedMovement;

        if (targetPositionBack.y < groundBack.y)
        {
            targetPositionBack.y = groundBack.y;
            if (falling)
            {
                landing = true;
            }
        }

        var targetPositionFront = legFrontCenter + plannedMovement;
        if (targetPositionFront.y < groundFront.y)
        {
            targetPositionFront.y = groundFront.y;
            if (falling)
            {
                landing = true;
            }
        }

        if ((landing && !touchesGroundAtStart) || (idle && state == State.FALLING))
        {
            state = State.LANDING;
            timeSinceLastLand = 0;
        }
        else if (idle)
        {
            if (timeSinceLastLand > 0.25 && timeSinceLastJump > 0.25)
            {
                state = State.IDLE;
            }
        }

        AnimationHelper.SetParameter(animator, "state", (int) state);

        Debug.DrawLine(targetPositionBack, groundBack, Color.green);
        Debug.DrawLine(targetPositionFront, groundFront, Color.blue);


        bool hasGroundBelow = groundBack.sqrMagnitude > 0.001f && groundFront.sqrMagnitude > 0.001f;

        if (hasGroundBelow)
        {
            Vector2 position = Vector2.Lerp(targetPositionFront, targetPositionBack, 0.5f);
            
            float angle = Mathf.Atan2(groundFront.y - groundBack.y, groundFront.x - groundBack.x);
            // Prevent glitching back up cliffs
            if (targetPositionFront.y > legFrontCenter.y + maxStepSize || targetPositionBack.y > legBackCenter.y + maxStepSize)
            {
                position.x = transform.position.x - oldOffset.x;
                position.y = Mathf.Min(targetPositionFront.y, targetPositionBack.y);
                angle = Mathf.Atan2(position.y - Mathf.Min(groundFront.y, groundBack.y), position.x - (groundFront.y < groundBack.y ? groundFront.x : groundBack.x));
            }

            if (Mathf.Abs(position.y - transform.position.y) > maxStepSize)
            {
                Debug.LogError("Prevented Teleporting Cat");
                return;
            }


            Debug.DrawLine(position, position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), Color.magenta);

            if (distanceGroundBack < closeToGround && distanceGroundFront < closeToGround)
            {
                angle = Mathf.Rad2Deg * angle;
            }

            if (angle < -70 || angle > 70)
            {
                Debug.LogError("Flipped the cat");
                return;
            }

            transform.SetPositionAndRotation((Vector2) position + oldOffset,
                Quaternion.AngleAxis(angle, Vector3.forward));
        }
        else
        {
            transform.SetPositionAndRotation((Vector2) transform.position + plannedMovement, Quaternion.identity);
        }
        */
    }


    bool touchesGround(Vector2 pos)
    {
        foreach (var raycastHit2D in Physics2D.RaycastAll(pos, Vector2.down, 1))
        {
            if (raycastHit2D.transform.CompareTag("Scene"))
            {
                return raycastHit2D.distance < maxStepSize;
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