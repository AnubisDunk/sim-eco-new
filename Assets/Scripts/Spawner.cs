
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance { get; private set; }
    public GameObject cube;
    public GameObject sphere;
    public GameObject foodPref;
    public GameObject waterPref;

    public int carnivore, herbivore = 0;
    public int food = 20;
    public int seed = 10;

    //private float posX, posZ;

    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        Random.InitState(seed);
        SpawnCreature(herbivore, carnivore);
        SpawnFood(food);
        SpawnWater();

    }

    // Update is called once per frame
    void Update()
    {
        Controls();

    }
    private void SpawnCreature(int herbivore, int carnivore)
    {

        SpawnOnNoise(sphere, 0.65f, herbivore, GlobalEventManager.SendCreatureBorn);
        SpawnOnNoise(cube, 0.65f, carnivore, GlobalEventManager.SendCreatureBorn);
        // instance.name = $"H{i}";
    }
    private void SpawnWater()
    {
        int mapZ = Utils.mapZ;
        int mapX = Utils.mapX;
        float[,] noiseMap = Utils.noiseMap;
        for (int y = 0; y < mapZ; y+=2)
        {
            for (int x = 0; x < mapX; x+=2)
            {
                if (noiseMap[x, y] <= 0.35f && noiseMap[x, y] >= 0.3f)
                {
                    Instantiate(waterPref, new Vector3(x - (mapX / 2), 1, (y * -1) + (mapZ / 2)), Quaternion.identity);
                }
            }
        }
        // instance.name = $"H{i}";
    }
    void SpawnOnNoise(GameObject instance, float limit, int count, System.Action sendSmth)
    {
        int tempCount = 0;
        float[,] noiseMap = Utils.noiseMap;
        int posX = Utils.mapX;
        int posZ = Utils.mapZ;
        while (tempCount < count)
        {
            int spawnx = Random.Range(0, posX);
            int spawny = Random.Range(0, posZ);
            if (noiseMap[spawnx, spawny] > limit)
            {
                Instantiate(instance, new Vector3((spawnx * 1) - (posX / 2), 1, (spawny * -1) + (posZ / 2)), Quaternion.identity);
                tempCount++;
                sendSmth.Invoke();
            }

        }
    }
    private void SpawnFood(int foodCount)
    {
        SpawnOnNoise(foodPref, 0.65f, foodCount, GlobalEventManager.SendFood);
    }

    void Controls()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GlobalEventManager.SendPause();
        }
    }
}
