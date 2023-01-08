using UnityEngine;

public class SpriteToCameraSize : MonoBehaviour
{
    void Update()
    {
        var cbl = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        var cbr = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        var ctl = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        var ctr = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        var cw = cbr.x - cbl.x;
        var ch = ctl.y - cbl.y;


        var pw = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        var ph = GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        transform.localScale = new Vector3(cw / pw, ch / ph, 1);
    }
}