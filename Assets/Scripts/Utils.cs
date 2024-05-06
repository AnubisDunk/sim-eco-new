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
        int x = Mathf.RoundToInt(obj.x) + 50;
        int z = Mathf.RoundToInt(obj.z) + 50;
        Debug.Log($"{x}/{z} / {noiseMap.GetLength(0)}");
        return obj.x >= mapX / 2 || (obj.z >= mapZ / 2) || (obj.x <= -(mapX / 2)) || (obj.z <= -(mapZ/2)); 
        // if (noiseMap[x + 50, z + 50] <= 0.35f)
        // {
        //     return true;
        // }
        // else
        //     return false;
        //Instantiate(waterPref, new Vector3(x - (mapX / 2), 1, (y * -1) + (mapZ / 2)), Quaternion.identity, waterHolder.transform);
        //|| (noiseMap[(int)obj.x, (int)obj.z] <= 0.35f);
    }
    public static float[,] noiseMap;
    public static int mapX, mapZ;
}
