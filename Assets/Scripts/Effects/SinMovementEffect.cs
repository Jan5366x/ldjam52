using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SinMovementEffect : MonoBehaviour
{
    public float amplifier;

    public float frequency;

    public MovementDirection direction;

    private Vector3 initPosition;

    void Start()
    {
        initPosition = transform.position;
    }

    void Update()
    {
        var delta = Mathf.Sin(Time.time * frequency) * amplifier;
        var yDelta = direction == MovementDirection.UpDown ? delta : 0f;
        var xDelta = direction == MovementDirection.LeftRight ? delta : 0f;

        transform.position = new Vector3(initPosition.x + xDelta, initPosition.y + yDelta, initPosition.z);
    }

    public enum MovementDirection
    {
        UpDown,
        LeftRight
    }
}

