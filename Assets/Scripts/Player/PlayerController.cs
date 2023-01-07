using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{

    public const String ANIM_IDLE = "Idle";
    public const String ANIM_WALK = "Walk";
    public const String ANIM_JUMP = "Jump";
    public const String ANIM_FALLING = "Falling";
    public const String ANIM_LANDING = "Landing";
    
    public Transform legBack;
    public Transform legFront;

    public Vector2 accelleration;
    public Vector2 velocity;
    public float timeSinceLastMove;

    void Start()
    {
        legBack = transform.Find("LegBack");
        legFront = transform.Find("LegFront");
    }

    // Update is called once per frame
    void Update()
    {
        var oldOffset = (Vector2) transform.position - Vector2.Lerp(legBack.position, legFront.position, 0.5f);
        Debug.DrawLine(transform.position, transform.position - (Vector3) oldOffset);
        accelleration = Vector2.zero;
        timeSinceLastMove += Time.deltaTime;

        var animator = GetComponent<Animator>();

        bool idle = true;
        bool falling = false;

        var touchesGroundAtStart = touchesGround(legBack.position) || touchesGround(legFront.position);
        if (touchesGroundAtStart)
        {
            if (Input.GetAxis("Horizontal") > 0.01)
            {
                accelleration.x = 10;
                timeSinceLastMove = 0;
                idle = false;
            }
            else if (Input.GetAxis("Horizontal") < -0.01)
            {
                accelleration.x = -10;
                timeSinceLastMove = 0;
                idle = false;
            }
            else
            {
                accelleration.x = 0;
            }

            if (Input.GetButton("Jump"))
            {
                accelleration.y = 300;
                velocity.y = 0;
                idle = false;
                animator.SetTrigger(ANIM_JUMP);
            }
        }
        else
        {
            timeSinceLastMove = 0;
            falling = true;
            animator.SetTrigger(ANIM_FALLING);
        }

        if (idle)
        {
            animator.SetTrigger(ANIM_IDLE);
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

        velocity.y = velocity.y + accelleration.y * Time.deltaTime;


        var plannedMovement = velocity * Time.deltaTime;

        Debug.DrawLine(transform.position, transform.position + (Vector3) velocity, Color.red);

        Vector2 groundBack = GetGroundPosition((Vector2) legBack.position + plannedMovement);
        Vector2 groundFront = GetGroundPosition((Vector2) legFront.position + plannedMovement);

        Debug.DrawLine(transform.position, groundBack, Color.green);
        Debug.DrawLine(transform.position, groundFront, Color.blue);

        bool landing = false;
        var targetPositionBack = (Vector2) legBack.position + plannedMovement;
        if (targetPositionBack.y < groundBack.y)
        {
            targetPositionBack.y = groundBack.y;
            if (falling)
            {
                landing = true;
            }
        }

        var targetPositionFront = (Vector2) legFront.position + plannedMovement;
        if (targetPositionFront.y < groundFront.y)
        {
            targetPositionFront.y = groundFront.y;
            if (falling)
            {
                landing = true;
            }
        }

        if (landing)
        {
            animator.SetTrigger(ANIM_LANDING);
        }


        Debug.Log(plannedMovement.magnitude);

        Debug.DrawLine(targetPositionBack, groundBack, Color.green);
        Debug.DrawLine(targetPositionFront, groundFront, Color.blue);


        bool hasGroundBelow = groundBack.sqrMagnitude > 0.001f && groundFront.sqrMagnitude > 0.001f;

        if (hasGroundBelow)
        {
            Vector2 position = Vector2.Lerp(targetPositionFront, targetPositionBack, 0.5f);
            float angle = Mathf.Atan2(targetPositionFront.y - targetPositionBack.y,
                targetPositionFront.x - targetPositionBack.x);

            Debug.DrawLine(position, position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), Color.magenta);

            angle = Mathf.Rad2Deg * angle;

            if (angle < -45 || angle > 45)
            {
                Debug.LogError("Flipped the cat");
            }
            else
            {
                transform.SetPositionAndRotation((Vector2) position + oldOffset,
                    Quaternion.AngleAxis(angle, Vector3.forward));
            }
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
}