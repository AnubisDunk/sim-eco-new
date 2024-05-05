
using UnityEngine;
using UnityEditor;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
using System.Collections.Generic;


public class Creature : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 roamPosition;
    public CreatureType creatureType;

    [SerializeField]
    public Creature father, mother;
    public AiAgent agent;
    public Gene[] genome;
    //public bool isSelected = false;
    public bool isSelectedByMale = false;
    public DNA creatureDna;
    public bool isFemale = false;
    public Creature desiredCreature;
    public Renderer render;
    public float size, rest, hunger, hungerCeil, hungerSpeed, growSpeed, restSpeed;
    public float senseRadius;
    public bool isReadyToMate = false;
    public TMP_Text uiTextState;
    public RectTransform uiHunger;
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
        render.material.color = isFemale ? new Color(1f, 0.25f, 0.96f, 1f) : new Color(0.19f, 0.23f, 0.92f, 1f);
        canvas = GetComponentInChildren<Canvas>();
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
        creatureDna.ShowDNA();
    }
    void Update()
    {
        //SelectionCheck();
        uiTextState.text = agent.stateMachine.currentState.ToString();

        canvas.transform.LookAt(Camera.main.transform);
        canvas.transform.localRotation *= Quaternion.Euler(0, 180, 0);
        //uiHunger.transform.LookAt(Camera.main.transform);
        Living();
        States();
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
        if (desiredCreature == null) Debug.Log("YOOOOOOO");
        GeneticAlgorithm genetic = new(creatureDna, desiredCreature.creatureDna, 0.25f);
        DNA dna = genetic.Execute();
        instance.creatureDna = dna;
        instance.father = desiredCreature;
        instance.desiredCreature = null;
        instance.mother = this;
        GlobalEventManager.SendCreatureBorn();

    }
    void States()
    {
        if (hunger >= hungerCeil / 4f && agent.stateMachine.currentState != AiStateId.LookingForFood) agent.stateMachine.ChangeState(AiStateId.LookingForFood);
        if (hunger <= hungerCeil / 4f && agent.creature.isReadyToMate && agent.stateMachine.currentState != AiStateId.LookingForMate) agent.stateMachine.ChangeState(AiStateId.LookingForMate);
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
        if (hunger >= hungerCeil && agent.stateMachine.currentState != AiStateId.Dead) Kill();
        isReadyToMate = size >= 100 && rest >= 100;

    }
    // void SelectionCheck()
    // {
    //     if (isSelected)
    //     {
    //         // senseRadiusDrawer.transform.position = transform.position;
    //         // sr.DrawCircle(100, senseRadius);
    //     }
    // }
    // // void DestroyController()
    // // {
    // //     if (Input.GetKeyDown(KeyCode.D)) Kill();
    // //     if ((transform.position.y <= -10) || IsOutOfBounds(transform.position))
    // //     {
    // //         Kill();
    // //     }
    // // }
    public void Kill()
    {
        agent.stateMachine.ChangeState(AiStateId.Dead);
    }
    private bool IsOutOfBounds(Vector3 obj)
    {
        return (obj.x >= Spawner.instance.planeX * 5) || (obj.z >= Spawner.instance.planeZ * 5) || (obj.x <= -Spawner.instance.planeX * 5) || (obj.z <= -Spawner.instance.planeZ * 5);
    }
    // private void OnDestroy()
    // {
    //     if (transform.childCount > 0)
    //     {
    //         transform.DetachChildren();
    //     }
    //     GlobalEventManager.SendCreatureKilled();
    // }
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
        Handles.DrawWireDisc(roamPosition, new Vector3(0, 1, 0), 0.1f);
        Handles.DrawLine(transform.position, roamPosition);
#endif

    }
    // public void OnTriggerExit(Collider other){
    //     if (other.gameObject.CompareTag("Bush") && other.gameObject.GetComponent<Food>().isReadyToEat)
    //     {
    //         agent.creature.bushes.Remove(other.gameObject);
    //     } 
    // }
}
