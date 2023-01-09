using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static string lastLevel = "World1";

    public static int soulsPerLevel = 9;

    public static int soulsCollected = 0;

    public static int totalSoulsCollected = 0;

    public static int levelsCompleted = 0;

    public static int totalHearts = 9;

    public static int hearts = 9;

    public static bool fillingHearts = false;

    public static World world = World.OtherDimention;

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
        hearts = 9;
    }

    public static bool CanLevelBeCompleted() =>
        soulsCollected >= soulsPerLevel;

    public static void LevelFinishedLevel()
    {
        totalSoulsCollected += soulsCollected;
        ResetLevel();
        levelsCompleted++;
    }

    public static void SoulPicked()
    {
        if (soulsCollected <= soulsPerLevel)
            soulsCollected++;
    }

    public static TimeSpan TimeSpent() =>
        DateTime.Now - startTime;

    public enum World
    {
        NormalDimention,
        OtherDimention
    }

    public int getTotalSoulsCollectedWhileInScene()
    {
        return totalSoulsCollected + soulsCollected;
    }
}