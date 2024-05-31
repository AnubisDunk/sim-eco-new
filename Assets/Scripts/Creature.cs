
using UnityEngine;
using UnityEditor;
using TMPro;
using System;
using System.IO;
using UnityEngine.SocialPlatforms;


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
    //public float dLevel = 0.3f;
    public bool isFemale = false;
    public Creature desiredCreature;
    public Creature predatorCreature;
    public Renderer render;
    public float size, rest, hunger, hungerLevel, hungerCeil, thirstCeil, hungerSpeed, thirst, thirstLevel, thirstSpeed, growSpeed, restSpeed;
    public float senseRadius;
    public bool isReadyToMate = false;
    public TMP_Text uiTextState;
    public TMP_Text uiAge;
    public RectTransform uiHunger;
    public RectTransform uiThirst;
    public bool isHungry, isThirsty;
    private SphereCollider scollider;
    private Canvas canvas;

    private float lifetime = 0f;
    private float birthtime = 0f;
    private int intLifetime = 0;
    private int intBirthtime = 0;
    private string fatherString, motherString;

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
        birthtime = Time.time;
        intBirthtime = (int)(birthtime % 60);
        //WriteGene(creatureDna);
        fatherString = father != null ? father.name : "-";
        motherString = mother != null ? mother.name : "-";
    }
    void WriteGene(DNA dna)
    {

        StreamWriter tw = new StreamWriter(Application.dataPath + "/genes.csv", true);
        string sex = isFemale ? "Female" : "Male";
        // string fatherString = father != null ? father.name : "-";
        // string motherString = mother != null ? mother.name : "-";
        tw.Write($"{name},{creatureType},{sex},");
        for (int i = 0; i < dna.genes.Length; i++)
        {
            tw.Write($"{dna.genes[i]},");
        }
        tw.Write($"{fatherString},{motherString},{intBirthtime},{intLifetime}");
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
        //isFemale = UnityEngine.Random.Range(0, 1f) > 0.5f;
        moveSpeed = creatureDna.genes[1];
        senseRadius = creatureDna.genes[2];
        hungerSpeed = creatureDna.genes[3];
        thirstSpeed = creatureDna.genes[4];
        hungerLevel = creatureDna.genes[5];
        thirstLevel = creatureDna.genes[6];
        growSpeed = creatureDna.genes[7];
        restSpeed = creatureDna.genes[8];
        //creatureDna.ShowDNA();
    }
    public void CreatureUpdate()
    {
        uiTextState.text = agent.stateMachine.currentState.ToString();
        if (!Utils.hideUi)
        {
            canvas.transform.LookAt(Camera.main.transform);
            canvas.transform.localRotation *= Quaternion.Euler(0, 180, 0);
        }

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
        var instance = Instantiate(this, transform.position, Quaternion.identity, transform.parent);
        instance.size = 0;
        instance.name = $"{name}+";
        instance.hunger = 0;
        //instance.creatureDna = creatureDna;
        //Debug.Log($"{creatureDna}/ {desiredCreature.creatureDna}");
        // if (desiredCreature == null) Debug.Log("YOOOOOOO");
        GeneticAlgorithm genetic = new(creatureDna, desiredCreature.creatureDna, Utils.genCrossower, Utils.genMutation);
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
        if ((hunger >= hungerCeil * hungerLevel) && !isHungry)
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

        if (thirst >= thirstCeil * thirstLevel && !isHungry)
        {
            if (agent.stateMachine.currentState != AiStateId.LookingForWater)
                agent.stateMachine.ChangeState(AiStateId.LookingForWater);
        }

        if (hunger <= hungerCeil * hungerLevel && thirst <= thirstCeil * thirstLevel && !isHungry && agent.creature.isReadyToMate && agent.stateMachine.currentState != AiStateId.LookingForMate)
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

        transform.localScale = 0.01f * size * Vector3.one;

        if (Utils.hideUi)
        {
            canvas.enabled = false;
        }
        else
        {
            canvas.enabled = true;
        }
        lifetime += Time.deltaTime;
        intLifetime = (int)(lifetime % 60);
        uiAge.text = intLifetime.ToString();
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
    void OnApplicationQuit(){
        Kill();
    }
    public void Kill()
    {
        agent.stateMachine.ChangeState(AiStateId.Dead);
    }

    public void DestroyCreature()
    {
        WriteGene(creatureDna);
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
