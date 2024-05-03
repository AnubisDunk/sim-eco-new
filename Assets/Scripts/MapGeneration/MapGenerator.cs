using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap, Mesh, FalloffMap };
    //[HideInInspector]
    public DrawMode drawMode;
    public int mapWidth;
    public int mapHeight;

    public float noiseScale;
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    float[,] falloffMap;

    public int seed;

    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;
    public bool isIsland;
    //public GameObject bush;
    public TerrainType[] regions;

    private void Awake()
    {
        falloffMap = FalloffGenerator.GenerateFalloffMap(mapWidth, mapHeight);
    }
    public void GenerateMap()
    {
        
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale, octaves, persistance, lacunarity, seed, offset);
        Color[] colorMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapHeight; x++)
            {
                if (isIsland)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {

                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        }
        else if (drawMode == DrawMode.FalloffMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapWidth, mapHeight)));

        }
        MapUtils.noiseMap = noiseMap;
        MapUtils.mapSize = mapWidth;
        // if (bushSpawn)
        // {
        //     BushController bushController = FindObjectOfType<BushController>();
        //     bushController.mapSize = mapHeight;
        //     bushController.GenerateBushes(noiseMap);
        // }


    }

    void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }
        if (mapHeight < 1)
        {
            mapWidth = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
        falloffMap = FalloffGenerator.GenerateFalloffMap(mapWidth, mapHeight);
    }
}
[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}