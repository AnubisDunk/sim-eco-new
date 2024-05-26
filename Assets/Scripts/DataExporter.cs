using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class DataExporter : MonoBehaviour
{

    private float nextUpdate = 1f;
    private string filenamePopulation = "";
    private string filenameGenes = "";
    //private TextWriter twPopulation,twGenes;
    void Start()
    {
        filenamePopulation = Application.dataPath + "/population.csv";
        filenameGenes = Application.dataPath + "/genes.csv";
        TextWriter twPopulation = new StreamWriter(filenamePopulation, false);
        twPopulation.WriteLine("Time,Rabbit,Fox");
        TextWriter twGenes = new StreamWriter(filenameGenes, false);
        twGenes.WriteLine("Name,Type,Sex,SGene,Speed,Sense,Hunger Speed,Thirst Speed,Hunger Level,Thirst Level,Grow Speed, Rest Speed,Father,Mother");
        twGenes.Close();
        twPopulation.Close();
    }

    void Update()
    {
        // If the next update is reached
        if (Time.time >= nextUpdate)
        {
            //Debug.Log(Time.time + ">=" + nextUpdate);
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            // Call your fonction
            UpdateEverySecond();
        }
    }
   

    void UpdateEverySecond()
    {
        GlobalEventManager.SendTime();
        TextWriter twPopulation = new StreamWriter(filenamePopulation, true);
        twPopulation.WriteLine($"{Mathf.FloorToInt(Time.time)},{Utils.herbivoreCount},{Utils.carnivoreCount}");
        twPopulation.Close();
    }
}
