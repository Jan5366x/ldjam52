using System;
using UnityEngine;

public class SinTransparencyEffect : MonoBehaviour
{
    [Range(0, 1)] public float minTransparency = 0.3f;
    [Range(0, 1)] public float maxTransparency = 1;
    public float frequency = 1;

    private void Update()
    {
        var color = GetComponent<SpriteRenderer>().color;
        color.a = Mathf.Lerp(minTransparency, maxTransparency,
            (1 + Mathf.Sin(Time.time * 2 * Mathf.PI * frequency)) / 2);
        GetComponent<SpriteRenderer>().color = color;
    }
}