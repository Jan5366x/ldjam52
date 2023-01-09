using Unity.Collections;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;

    public Vector2 centerNormalized = new Vector2(0.5f, 0.25f);
    public Rect nothingEdge;
    public Rect softEdge;
    public Rect hardEdge;

    public float speedSoft = 5;
    public float speedHard = 12;

    [ReadOnly] public float speed;
    [ReadOnly] public Vector2 dir;
    [ReadOnly] public Vector2 screenRatio;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    void FixedUpdate()
    {
        if (player)
        {
            speed = 0;
            Camera camera = GetComponentInChildren<Camera>();
            screenRatio = camera.WorldToScreenPoint(player.transform.position) /
                          new Vector2(Screen.width, Screen.height);
            Debug.DrawLine(camera.ViewportToWorldPoint(centerNormalized), player.transform.position, Color.red);
            DrawRect(camera, nothingEdge);
            DrawRect(camera, softEdge);
            DrawRect(camera, hardEdge);
            if (nothingEdge.Contains(screenRatio))
            {
                speed = 0;
            }
            else if (softEdge.Contains(screenRatio))
            {
                speed = Mathf.Lerp(0, speedSoft, getMaxRectRatio(nothingEdge, softEdge, screenRatio));
            }
            else if (hardEdge.Contains(screenRatio))
            {
                speed = Mathf.Lerp(speedSoft, speedHard, getMaxRectRatio(softEdge, hardEdge, screenRatio));
            }
            else
            {
                speed = speedHard;
            }

            Vector3 delta = player.transform.position - camera.ViewportToWorldPoint(centerNormalized);
            Debug.DrawLine(player.transform.position, transform.position + delta, Color.red);

            dir = delta / delta.magnitude * (speed * Time.fixedDeltaTime);
            transform.Translate(dir.x, dir.y, 0);
        }
    }

    private float getMaxRectRatio(Rect innerRect, Rect outerRect, Vector2 point)
    {
        Vector2 deltaPoint = (point - centerNormalized);
        float deltaX = Mathf.Abs(deltaPoint.x);
        float deltaY = Mathf.Abs(deltaPoint.y);

        float innerWidth = innerRect.width / 2;
        float innerHeight = innerRect.height / 2;
        float outerWidth = outerRect.width / 2;
        float outerHeight = outerRect.height / 2;

        float tX = Mathf.InverseLerp(innerWidth, outerWidth, deltaX);
        float tY = Mathf.InverseLerp(innerHeight, outerHeight, deltaY);

        return Mathf.Max(tX, tY);
    }

    private static void DrawRect(Camera camera, Rect rect)
    {
        var bl = camera.ViewportToWorldPoint(new Vector3(rect.xMin, rect.yMin));
        var br = camera.ViewportToWorldPoint(new Vector3(rect.xMax, rect.yMin));
        var tl = camera.ViewportToWorldPoint(new Vector3(rect.xMin, rect.yMax));
        var tr = camera.ViewportToWorldPoint(new Vector3(rect.xMax, rect.yMax));
        bl.z = 0;
        br.z = 0;
        tl.z = 0;
        tr.z = 0;

        Debug.DrawLine(bl, br, Color.red);
        Debug.DrawLine(bl, tl, Color.red);
        Debug.DrawLine(tl, tr, Color.red);
        Debug.DrawLine(tr, br, Color.red);
    }
}