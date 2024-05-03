using UnityEngine;

public class MapUtils : MonoBehaviour
{
    public static MapUtils ut = null;
    void Awake(){
        ut = this;
    }
    public static float[,] noiseMap;
    public static int mapSize;
}
