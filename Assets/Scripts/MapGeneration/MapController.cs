using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private MapGenerator mapGenerator;
    //public PopulationManager popManager;
    //public BushController bushController;
    private void Awake()
    {
        mapGenerator = GetComponent<MapGenerator>();
        mapGenerator.GenerateMap();
    }
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) isScrolling = !isScrolling;
        if (Input.GetKeyDown(KeyCode.R)) RegenerateMap();

    }
    void RegenerateMap()
    {
        //bushController.DeleteBushes();
        mapGenerator.seed = Random.Range(0, 100);
        mapGenerator.GenerateMap();
        //bushController.GenerateBushes(Utils.noiseMap);
       // popManager.Generate();
        //StatsUi.Reset();
        
    }
}
