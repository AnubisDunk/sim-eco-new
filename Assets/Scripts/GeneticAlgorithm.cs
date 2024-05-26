using UnityEngine;

public class GeneticAlgorithm
{
    DNA mother, father, child;
    float mutationRate;

    public GeneticAlgorithm(DNA mother, DNA father, float mutationRate)
    {
        this.mother = mother;
        this.father = father;
        this.mutationRate = mutationRate;
    }
    public DNA Execute()
    {
        Crossover();
        Mutate();
        return child;
    }
    void Mutate()
    {
        for (int i = 0; i < father.genes.Length; i++)
        {
            if (Random.value <= mutationRate) {
                float mgene = Random.Range(child.genome[i].minValue, child.genome[i].maxValue);
                child.genes[i] = Mathf.Round(mgene * 100f) * 0.01f;
            Debug.Log("Mutate");
            }
        }
    }
    void Crossover()
    {

        child = mother;
        int cut = Random.Range(0, mother.genes.Length);
        for (int i = 0; i < cut; i++)
        {
            child.genes[i] = mother.genes[i];

        }
        for (int j = cut; j < father.genes.Length; j++)
        {
            child.genes[j] = father.genes[j];
        }
    }
}
