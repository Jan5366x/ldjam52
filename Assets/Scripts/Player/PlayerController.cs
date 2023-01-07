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
        var legBackCenter = GetColliderCenter(legBack);
        var legFrontCenter = GetColliderCenter(legFront);
        
        var oldOffset = (Vector2) transform.position - Vector2.Lerp(legBackCenter, legFrontCenter, 0.5f);
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

        var touchesGroundAtStart = touchesGround(legBackCenter) || touchesGround(legFrontCenter);
        if (touchesGroundAtStart)
        {
            if (Input.GetAxis("Horizontal") > 0.01)
            {
                accelleration.x = 10;
                timeSinceLastMove = 0;
                idle = false;
                if (timeSinceLastLand > 0.25 && timeSinceLastJump > 0.25)
                {
                    state = State.WALK;
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
            }
            else
            {
                accelleration.x = 0;
            }

            if (Input.GetButton("Jump") && timeSinceLastJump > 0.25)
            {
                accelleration.y = 300;
                velocity.y = 0;
                idle = false;
                state = State.JUMP;
                timeSinceLastJump = 0;
            }
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
        velocity.x = touchesGroundAtStart
            ? velocity.x + accelleration.x * Time.deltaTime
            : velocity.x;
        if (timeSinceLastMove > 0.01)
        {
            velocity.x -= (velocity.x * Mathf.Min(1f, Mathf.Exp(timeSinceLastMove - 1)));
        }

        if (Mathf.Abs(velocity.x) < 0.1)
        {
            velocity.x = 0;
        }
        velocity.y = Mathf.Clamp(velocity.y + accelleration.y * Time.deltaTime, -5, 5);
       


        var plannedMovement = velocity * Time.deltaTime;

        Debug.DrawLine(transform.position, transform.position + (Vector3) velocity, Color.red);

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
            float angle = Mathf.Atan2(groundFront.y - groundBack.y,
                groundFront.x - groundBack.x);

            Debug.DrawLine(position, position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), Color.magenta);

            if (distanceGroundBack < 1 && distanceGroundFront < 1)
            {
                angle = Mathf.Rad2Deg * angle;
            }

            Debug.Log(angle);

            if (angle < -70 || angle > 70)
            {
                Debug.LogError("Flipped the cat");
            }
                transform.SetPositionAndRotation((Vector2) position + oldOffset,
                    Quaternion.AngleAxis(angle, Vector3.forward));
        }
        else
        {
            transform.SetPositionAndRotation((Vector2) transform.position + plannedMovement, Quaternion.identity);
        }
    }


    bool touchesGround(Vector2 pos)
    {
        foreach (var raycastHit2D in Physics2D.RaycastAll(pos, Vector2.down, 1))
        {
            if (raycastHit2D.transform.CompareTag("Scene"))
            {
                return raycastHit2D.distance < 0.1;
            }
        }

        return false;
    }

    Vector2 GetGroundPosition(Vector2 pos)
    {
        float minDistance = 99999;
        Vector2? minObject = null;
        foreach (var raycastHit2D in Physics2D.RaycastAll(new Vector2(pos.x, 99), Vector2.down))
        {
            if (raycastHit2D.transform.CompareTag("Scene"))
            {
                float distance = Vector2.Distance(pos, raycastHit2D.point);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    minObject = raycastHit2D.point;
                }
            }
        }

        return (minObject == null) ? Vector2.zero : minObject.Value;
    }

    Vector2 GetColliderCenter(Transform transform)
    {
        return (Vector2) transform.GetComponent<Collider2D>().bounds.center;
    }
}