
using UnityEngine;
using UnityEditor;
using TMPro;
using System;
using System.IO;


public class Creature : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 roamPosition, lastPosition;
    public CreatureType creatureType;

    [SerializeField]
    public Creature father, mother;
    public AiAgent agent;
    public Gene[] genome;
    //public bool isSelected = false;
    public bool isSelectedByMale = false;
    public DNA creatureDna;

    [Range(0, 1)]
    public float dLevel = 0.3f;
    public bool isFemale = false;
    public Creature desiredCreature;
    public Creature predatorCreature;
    public Renderer render;
    public float size, rest, hunger, hungerCeil, hungerSpeed, thirst, thirstCeil, thirstSpeed, growSpeed, restSpeed;
    public float senseRadius;
    public bool isReadyToMate = false;
    public TMP_Text uiTextState;
    public RectTransform uiHunger;
    public RectTransform uiThirst;
    public bool isHungry, isThirsty;
    private SphereCollider scollider;
    private Canvas canvas;

    public void Init()
    {
        agent = GetComponent<AiAgent>();
        render = GetComponent<Renderer>();
        InitGenes(creatureDna);
        scollider = GetComponent<SphereCollider>();
        scollider.radius = senseRadius;
        agent.stateMachine.ChangeState(AiStateId.Wander);
        render.materials[1].color = isFemale ? new Color(1f, 0.25f, 0.96f, 1f) : new Color(0.19f, 0.23f, 0.92f, 1f);
        canvas = GetComponentInChildren<Canvas>();
        if (isFemale) name = NamesReader.namesFemale[UnityEngine.Random.Range(0, NamesReader.namesFemale.Length - 1)];
        if (!isFemale) name = NamesReader.namesMale[UnityEngine.Random.Range(0, NamesReader.namesMale.Length - 1)];
        WriteGene(creatureDna);
    }
    void WriteGene(DNA dna)
    {

        StreamWriter tw = new StreamWriter(Application.dataPath + "/genes.csv", true);
        string sex = isFemale ? "Female" : "Male";
        string fatherString = father != null ? father.name : "-";
        string motherString = mother != null ? mother.name : "-";
        tw.Write($"{name},{creatureType},{sex},");
        for (int i = 0; i < dna.genes.Length; i++)
        {
            tw.Write($"{dna.genes[i]},");
        }
        tw.Write($"{fatherString},{motherString},");
        tw.WriteLine("");
        tw.Close();

    }
    void InitGenes(DNA dna)
    {
        if (dna == null)
        {
            creatureDna = new DNA(genome);
        }
        else
        {
            creatureDna = dna;
        }
        isFemale = creatureDna.genes[0] > 0.5f;
        moveSpeed = creatureDna.genes[1];
        senseRadius = creatureDna.genes[2];
        //creatureDna.ShowDNA();
    }
    public void CreatureUpdate()
    {
        uiTextState.text = agent.stateMachine.currentState.ToString();
        canvas.transform.LookAt(Camera.main.transform);
        canvas.transform.localRotation *= Quaternion.Euler(0, 180, 0);
        Living();
        States();
        Boundries();
    }
    public void Boundries()
    {
        if (transform.position.x >= Utils.mapX / 2 || (transform.position.z >= Utils.mapZ / 2) || (transform.position.x <= -(Utils.mapX / 2)) || (transform.position.z <= -(Utils.mapZ / 2)))
        {
            Debug.Log("Out");
            Kill();
        }
    }
    public void Mating()
    {

        //Debug.Log($"I am({name}) mating with {desiredCreature.name}");
        rest = 0;
        isReadyToMate = false;
        isSelectedByMale = false;
        if (desiredCreature != null) Birth();

        desiredCreature = null;

    }
    void Birth()
    {
        var instance = Instantiate(this, transform.position, Quaternion.identity);
        instance.size = 0;
        instance.name = $"{name}+";
        instance.hunger = 0;
        //instance.creatureDna = creatureDna;
        //Debug.Log($"{creatureDna}/ {desiredCreature.creatureDna}");
        // if (desiredCreature == null) Debug.Log("YOOOOOOO");
        GeneticAlgorithm genetic = new(creatureDna, desiredCreature.creatureDna, 0.25f);
        DNA dna = genetic.Execute();
        instance.creatureDna = dna;
        instance.father = desiredCreature;
        instance.desiredCreature = null;
        instance.mother = this;
        if (instance.creatureType == CreatureType.Herbivore) GlobalEventManager.SendHerbivoreBorn();
        if (instance.creatureType == CreatureType.Carnivore) GlobalEventManager.SendCarnivoreBorn();
    }
    void States()
    {
        //if(predatorCreature != null && agent.stateMachine.currentState != AiStateId.Fleeing) agent.stateMachine.ChangeState(AiStateId.Fleeing); 
        if ((hunger >= hungerCeil * dLevel) && !isHungry)
        {
            if (creatureType == CreatureType.Herbivore)
            {
                if (agent.stateMachine.currentState != AiStateId.LookingForFood)
                {
                    isHungry = true;
                    agent.stateMachine.ChangeState(AiStateId.LookingForFood);
                }
            }
            if (creatureType == CreatureType.Carnivore)
            {
                if (agent.stateMachine.currentState != AiStateId.LookingForPrey)
                {
                    isHungry = true;
                    agent.stateMachine.ChangeState(AiStateId.LookingForPrey);
                }
            }

        }

        if (thirst >= thirstCeil * dLevel && !isHungry)
        {
            if (agent.stateMachine.currentState != AiStateId.LookingForWater)
                agent.stateMachine.ChangeState(AiStateId.LookingForWater);
        }

        if (hunger <= hungerCeil * dLevel && thirst <= thirstCeil * dLevel && !isHungry && agent.creature.isReadyToMate && agent.stateMachine.currentState != AiStateId.LookingForMate)
            agent.stateMachine.ChangeState(AiStateId.LookingForMate);
    }

    void Living()
    {
        if (rest <= 100) rest += Time.deltaTime * restSpeed;
        if (size <= 100) size += Time.deltaTime * growSpeed;
        if (hunger <= hungerCeil)
        {
            hunger += Time.deltaTime * hungerSpeed;
            uiHunger.localScale = new Vector3(hunger / 100f, 1, 1);
        }
        if (thirst <= thirstCeil)
        {
            thirst += Time.deltaTime * thirstSpeed;
            uiThirst.localScale = new Vector3(thirst / 100f, 1, 1);
        }
        if ((hunger >= hungerCeil || thirst >= thirstCeil) && agent.stateMachine.currentState != AiStateId.Dead) Kill();
        isReadyToMate = size >= 100 && rest >= 100;

    }
    // public void OnTriggerEnter(Collider other)
    // {
    //     if(creatureType == CreatureType.Herbivore) {

    //     }

    //     if (other.gameObject.CompareTag("Creature") && other.GetComponent<Creature>().creatureType == CreatureType.Carnivore)
    //     {
    //         Debug.Log("Predator");
    //     }
    // }

    public void Kill()
    {
        agent.stateMachine.ChangeState(AiStateId.Dead);
    }

    public void DestroyCreature()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
#if UNITY_EDITOR
        Handles.color = new Color(1, 1, 1, 0.4f);
        Handles.DrawWireDisc(origin, new Vector3(0, 1, 0), senseRadius);
        Handles.DrawWireDisc(roamPosition, new Vector3(0, 1, 0), 0.5f);
        Handles.DrawLine(transform.position, roamPosition);
#endif
    }
}
