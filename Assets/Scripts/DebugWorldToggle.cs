using UnityEngine;

public class DebugWorldToggle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning("Debug World Toggle exist ... remove before release!!!!"); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GlobalVariables.world = GlobalVariables.world == GlobalVariables.World.NormalDimention
                ? GlobalVariables.World.OtherDimention
                : GlobalVariables.World.NormalDimention;
            
            Debug.LogWarning("Debug World Toggle triggered!!!!"); 
        }
    }
}
