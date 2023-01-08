using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectSwapper : MonoBehaviour
{
    public string normalObjectNameContains = "normal";

    public string otherObjectNameContains = "other";

    private GameObject normalObject;

    private GameObject otherObject;

    private GlobalVariables.World worldState;

    // Start is called before the first frame update
    void Start()
    {
        worldState = GlobalVariables.world;

        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (NameContains(child, normalObjectNameContains))
                normalObject = child.gameObject;
            else if (NameContains(child, otherObjectNameContains))
                otherObject = child.gameObject;
        }

        bool NameContains(Transform child, string contains)
        {
            return child.name.Contains(contains, StringComparison.OrdinalIgnoreCase);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (worldState == GlobalVariables.world ||
            normalObject == null ||
            otherObject == null)
            return;

        var activateMainWorld = GlobalVariables.world switch
        {
            GlobalVariables.World.OtherDimention => false,
            _ => true,
        };

        normalObject.SetActive(activateMainWorld);
        otherObject.SetActive(!activateMainWorld);
        worldState = GlobalVariables.world;
    }
}
