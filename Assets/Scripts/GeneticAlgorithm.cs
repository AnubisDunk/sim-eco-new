using UnityEngine;

public enum Crossower{
        OnePointCrossover = 0,
        TwoPointCrossover = 1,
        UniformCrossover = 2,
    }
public class GeneticAlgorithm
{
    DNA mother, father, child;
    float mutationRate;
    int crossower;

    public GeneticAlgorithm(DNA mother, DNA father, Crossower crossower, float mutationRate)
    {
        this.mother = mother;
        this.father = father;
        this.crossower = (int)crossower;
        this.mutationRate = mutationRate;
    }

    public DNA Execute()
    {
        switch (crossower)
        {
            case 0:
                OnePointCrossover();
                Debug.Log("OnePoint");
                break;
            case 1:
                TwoPointCrossover();
                Debug.Log("TwoPoint");
                break;
            case 2:
                UniformCrossover();
                Debug.Log("Uniform");
                break;
            default:
                OnePointCrossover();
                break;
        }
        Mutate();
        return child;
    }
    void Mutate()
    {
        for (int i = 0; i < father.genes.Length; i++)
        {
            if (Random.value <= mutationRate)
            {
                float mgene = Random.Range(child.genome[i].minValue, child.genome[i].maxValue);
                child.genes[i] = Mathf.Round(mgene * 100f) * 0.01f;
            }
        }
    }
    void OnePointCrossover()
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
    void TwoPointCrossover()
    {
        child = mother;
        int cut = Random.Range(0, mother.genes.Length);
        int cut2 = Random.Range(cut, mother.genes.Length);
        for (int i = 0; i < cut; i++)
        {
            child.genes[i] = mother.genes[i];
        }
        for (int j = cut; j < cut2; j++)
        {
            child.genes[j] = father.genes[j];
        }
        for (int k = cut2; k < father.genes.Length; k++)
        {
            child.genes[k] = mother.genes[k];
        }
    }
    void UniformCrossover()
    {
        child = mother;
        for (int i = 0; i < father.genes.Length; i++)
        {
            child.genes[i] = Random.Range(0f, 1f) > 0.5f ? mother.genes[i] : father.genes[i];
        }
    }
}
