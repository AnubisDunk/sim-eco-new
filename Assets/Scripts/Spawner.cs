using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance { get; private set; }
    public GameObject cube;
    public GameObject sphere;
    public GameObject foodPref;

    public int carnivore, herbivore = 0;
    public int food = 20;
    public GameObject plane;
    public float planeX, planeZ = 1f;
    public float spawnX, spawnZ = 1f;
    public GameObject spawnZone;
    public int seed = 10;

    private float posX, posZ;
   
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
}    void Start()
    {
        Random.InitState(seed);
        posX = spawnX * 4.5f;
        posZ = spawnZ * 4.5f;
        //plane.transform.localScale = new Vector3(planeX, 1, planeZ);
        spawnZone.transform.localScale = new Vector3(spawnX, 1, spawnZ);
        SpawnCreature(herbivore,carnivore);
        SpawnFood(food);

    }

    // Update is called once per frame
    void Update()
    {
        Controls();
        
    }
    private void SpawnCreature(int herbivore,int carnivore)
    {
        for (int i = 0; i < herbivore; i++)
        {
            Vector3 randomPos = new(Random.Range(-posX, posX), 0, Random.Range(-posZ, posZ));
            var instance = Instantiate(sphere, randomPos, Quaternion.identity);
            instance.name = $"H{i}";
            GlobalEventManager.SendCreatureBorn();
            
        }
         for (int i = 0; i < carnivore; i++)
        {
            Vector3 randomPos = new(Random.Range(-posX, posX), 0, Random.Range(-posZ, posZ));
            var instance = Instantiate(cube, randomPos, Quaternion.identity);
            instance.name = $"C{i}";
            GlobalEventManager.SendCreatureBorn();
        }
    }
    private void SpawnFood(int foodCount) {
         for (int i = 0; i < foodCount; i++)
        {
            Vector3 randomPos = new(Random.Range(-posX, posX), 0, Random.Range(-posZ, posZ));
            var foodInstance = Instantiate(foodPref, randomPos, Quaternion.identity);
            GlobalEventManager.SendFood();
            
        }
    }
    void Controls(){
        if (Input.GetKeyDown(KeyCode.Space)) {
            GlobalEventManager.SendPause();
        } 
       // if(Input.GetKeyDown(KeyCode.S)) SpawnCreature(1,1);
    }  
}
