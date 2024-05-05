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
        return (obj.x >= mapX - (mapX / 2)) || (obj.z >= mapZ - (mapZ / 2)) || (obj.x <= -mapX - (mapX / 2)) || (obj.z <= -mapZ - (mapZ / 2)); 
        //|| (noiseMap[(int)obj.x, (int)obj.z] <= 0.35f);
    }
    public static float[,] noiseMap;
    public static int mapX, mapZ;
}
