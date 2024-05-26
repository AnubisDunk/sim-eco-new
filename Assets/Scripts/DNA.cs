using UnityEngine;
public class DNA
{
    public float[] genes;
    public Gene[] genome;
    // public DNA(float[] values)
    // {
    //     genes = values;
    // }
    public DNA(Gene[] values){ 
        genome = values; 
        genes = new float[values.Length];
        for(int i = 0; i< values.Length;i++){
            genes[i] = Mathf.Round(Random.Range(values[i].minValue, values[i].maxValue)*100f)/ 100f;
        }
        
    }
    public void ShowDNA() {
     string prompt = "";
        for(int i = 0; i< genes.Length;i++){
            prompt += $"{genome[i].name}:{genes[i]} | ";
        }
        Debug.Log(prompt);
    }
}
