using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[ExecuteInEditMode]
public class RotateToPlayer : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;

        var direction3 = player.transform.position - transform.position;
        var direction = new Vector2(direction3.x, direction3.y);
        transform.rotation = Quaternion.FromToRotation(Vector2.up, direction);
    }
}
