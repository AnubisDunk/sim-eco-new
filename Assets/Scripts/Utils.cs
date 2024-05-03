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
        return (obj.x >= Spawner.instance.planeX * 5) || (obj.z >= Spawner.instance.planeZ * 5) || (obj.x <= -Spawner.instance.planeX * 5) || (obj.z <= -Spawner.instance.planeZ * 5);
    }
}
