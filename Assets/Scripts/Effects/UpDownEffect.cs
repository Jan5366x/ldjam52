using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class UpDownEffect : MonoBehaviour
{
    public float amplifier;

    public float frequency;

    private Vector3 initPosition;

    void Start()
    {
        initPosition = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(initPosition.x, initPosition.y + (Mathf.Sin(Time.time * frequency) * amplifier), initPosition.z);
    }
}
