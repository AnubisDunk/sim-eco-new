using System;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;

    public float noiseScale;
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    public int seed;
    public Vector2 offset;
    public bool isIsland;

    [Range(0, 10)]
    public float falloffStart, falloffEnd;

    public bool autoUpdate;
    private float[,] falloffMap;
    public TerrainType[] regions;
    private Color[] colorMap;

    private Renderer texRender;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    //private Color[] pix;
    void Awake()
    {
        GenerateMap();
    }
    public void GenerateMap()
    {
        texRender = GetComponent<Renderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale, octaves, persistance, lacunarity, seed, offset);
        falloffMap = GenerateFalloffMap(mapWidth, mapHeight, falloffStart, falloffEnd);
        colorMap = new Color[mapWidth * mapHeight];
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
        DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, 1), TextureFromColorMap(colorMap, mapWidth, mapHeight));
        Utils.noiseMap = noiseMap;
        Utils.mapX = mapWidth;
        Utils.mapZ = mapHeight;
    
    }
    public Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }
        return TextureFromColorMap(colorMap, width, height);
    }
    public Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
    {
        Texture2D texture = new(width, height)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }
    public float[,] GenerateFalloffMap(int width, int height, float falloffStart, float falloffEnd)
    {
        float[,] map = new float[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 position = new(
                (float)x / width * 2 - 1,
                (float)y / height * 2 - 1
                );

                float value = Mathf.Max(Mathf.Abs(position.x), Mathf.Abs(position.y));
                //map[x, y] = Evaluate(value);
                // if (value < falloffStart)
                // {
                //     map[x, y] = 1;
                // }
                // else if (value > falloffEnd)
                // {
                //     map[x, y] = 0;
                // }
                // else
                // {
                    map[x, y] = Evaluate(value,falloffStart,falloffEnd);
                    //map[x, y] = Mathf.SmoothStep(1, 0, Mathf.InverseLerp(falloffStart, falloffEnd, value));
                
            }
        }
        return map;
    }
    float Evaluate(float value,float a, float b)
    {
        // float a = 4; //3
        // float b = 3; //2.2

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
        meshCollider.sharedMesh = meshData.CreateMesh();
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
        //falloffMap = FalloffGenerator.GenerateFalloffMap(mapWidth, mapHeight);
    }
    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }
}
