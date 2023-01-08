using UnityEngine;

public class CameraColorSwapper : MonoBehaviour
{
    public Color normalColor;
    public Color otherColor;

    private GlobalVariables.World _worldState;

    // Start is called before the first frame update
    void Start()
    {
        UpdateSwitch();
    }

    // Update is called once per frame
    void Update()
    {
        if (_worldState == GlobalVariables.world)
            return;

        UpdateSwitch();
    }

    private void UpdateSwitch()
    {
        var activateMainWorld = GlobalVariables.world switch
        {
            GlobalVariables.World.OtherDimention => false,
            _ => true,
        };

        Camera.main.backgroundColor = activateMainWorld ? normalColor : otherColor;
        _worldState = GlobalVariables.world;
    }
}