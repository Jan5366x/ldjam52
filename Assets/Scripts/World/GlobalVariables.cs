using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static string lastLevel;

    public static int soulsPerLevel = 9;

    public static int soulsCollected = 0;

    public static int totalSoulsCollected = 0;

    public static int levelsCompleted = 0;

    public static World world = World.NormalDimention;

    public static DateTime startTime = DateTime.Now;

    public static void ResetLevel()
    {
        world = World.NormalDimention;
        soulsCollected = 0;
    }

    public static void ResetGame()
    {
        startTime = DateTime.Now;
        world = World.NormalDimention;
        soulsCollected = 0;
        totalSoulsCollected = 0;
        levelsCompleted = 0;
    }

    public static bool IsLevelCompletedNextLevel() =>
        soulsCollected >= totalSoulsCollected;

    public static void NextLevel()
    {
        ResetLevel();
        levelsCompleted++;
    }

    public static void SoulPicked()
    {
        if (soulsCollected + 1 <= soulsPerLevel)
            soulsCollected++;
    }

    public enum World
    {
        NormalDimention,
        OtherDimention
    }
}
