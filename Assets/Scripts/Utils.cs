//using System;
using System;
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
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;
    }
    public static bool IsOutOfBounds(Vector3 obj)
    {
        try{
           if (boundMap[Mathf.RoundToInt(obj.x + (mapX / 2)), Mathf.RoundToInt(obj.z * -1 + (mapZ / 2))] )
        {
            //Debug.Log("Is out of bounds");
            return true;
        }
        else return false;
        }
        catch (IndexOutOfRangeException)
        {
            return false;
        }
       
    }
    
    public static int herbivoreCount;

    public static bool hideUi = false;
    public static int carnivoreCount;

    public static Crossower genCrossower;
    public static float genMutation;
    public static float[,] noiseMap;
    public static bool[,] boundMap;
    public static int mapX, mapZ;
}
