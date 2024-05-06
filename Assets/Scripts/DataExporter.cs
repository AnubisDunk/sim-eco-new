using System.IO;
using UnityEngine;

public class DataExporter : MonoBehaviour
{

    private float nextUpdate = 1f;
    private string filename = "";
    void Start()
    {
        filename = Application.dataPath + "/data.csv";
        TextWriter tw = new StreamWriter(filename, false);
        tw.WriteLine("Time,Population");
        tw.Close();
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
        TextWriter tw = new StreamWriter(filename, true);
        tw.WriteLine($"{Mathf.FloorToInt(Time.time)},{Utils.creatureCount}");
        tw.Close();
    }
}
