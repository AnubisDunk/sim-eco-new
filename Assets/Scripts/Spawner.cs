
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
        // Instantiate(foodPref,new Vector3(0, 5, 0),Quaternion.identity);
        // Debug.Log($"{Utils.noiseMap[0,0]}");
        //Instantiate(foodPref,new Vector3(Utils.noiseMap[30,30], 5, 30),Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        Controls();

    }
    private void SpawnCreature(int herbivore, int carnivore)
    {
        GameObject creatureHolder = new("CreatureHolder");
        creatureHolder.transform.parent = transform;
        SpawnOnNoise(sphere, 0.65f, 0.5f, herbivore, GlobalEventManager.SendCreatureBorn, creatureHolder);
        SpawnOnNoise(cube, 0.65f, 0.5f, carnivore, GlobalEventManager.SendCreatureBorn, creatureHolder);
        // instance.name = $"H{i}";
    }
    private void SpawnWater()
    {
        GameObject waterHolder = new("WaterHolder");
        waterHolder.transform.parent = transform;
        int mapZ = Utils.mapZ;
        int mapX = Utils.mapX;
        float[,] noiseMap = Utils.noiseMap;
        for (int y = 0; y < mapZ; y++)
        {
            for (int x = 0; x < mapX; x++)
            {
                if (noiseMap[x, y] <= 0.35f && noiseMap[x, y] >= 0.25f)
                {
                    Instantiate(waterPref, new Vector3(x - (mapX / 2), 1, (y * -1) + (mapZ / 2)), Quaternion.identity, waterHolder.transform);
                }
            }
        }
        // instance.name = $"H{i}";
    }
    void SpawnOnNoise(GameObject instance, float limit, float elevation, int count, System.Action sendSmth, GameObject holder)
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
                Instantiate(instance, new Vector3((spawnx * 1) - (posX / 2), elevation, (spawny * -1) + (posZ / 2)), Quaternion.identity, holder.transform);
                tempCount++;
                sendSmth.Invoke();
            }

        }
    }
    private void SpawnFood(int foodCount)
    {
        GameObject foodHolder = new("FoodHolder");
        foodHolder.transform.parent = transform;
        SpawnOnNoise(foodPref, 0.4f, 0.4f, foodCount, GlobalEventManager.SendFood, foodHolder);
    }

    void Controls()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GlobalEventManager.SendPause();
        }
    }
}
