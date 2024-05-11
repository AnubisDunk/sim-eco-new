//using System;
using UnityEngine;


public enum CreatureType
{
    Herbivore,
    Carnivore,
    Omnivore
}
public static class Utils
{
    public static Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }
    public static bool IsOutOfBounds(Vector3 obj)
    {

        if (boundMap[Mathf.RoundToInt(obj.x + 32), Mathf.RoundToInt(obj.z * -1 + 32)] )
        {
            return true;
        }
        else return false;
    }
    public static int creatureCount;
    public static float[,] noiseMap;
    public static bool[,] boundMap;
    public static int mapX, mapZ;
}
