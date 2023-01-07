using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static int playerMaxLives = 9;

    public static int playerLives = 1;

    public static World world = World.Main;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public enum World
    {
        Main,
        UpsideDown
    }
}
