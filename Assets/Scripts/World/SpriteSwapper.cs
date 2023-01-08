using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteSwapper : MonoBehaviour
{
    public Sprite normalSprite;

    public Sprite otherWorldSprite;

    private SpriteRenderer rendered;

    // Start is called before the first frame update
    void Start()
    {
        rendered = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rendered == null)
            return;

        var sprite = GlobalVariables.world switch
        {
            GlobalVariables.World.OtherDimention => otherWorldSprite,
            _ => normalSprite,
        };

        if (rendered.sprite != sprite)
            rendered.sprite = sprite;
    }
}
