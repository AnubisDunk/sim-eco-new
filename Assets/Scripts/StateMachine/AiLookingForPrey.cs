using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiLookingForPrey : AiState
{
    private Vector3 roamPosition;
    public Transform creature;

    public bool isFoundFood = false;
    private Vector3 preyPosition;
    private Creature prey;
    public AiStateId GetId()
    {
        return AiStateId.LookingForPrey;
    }
    public void Enter(AiAgent agent)
    {
        roamPosition = GetRoamingPosition(agent);
        isFoundFood = false;
    }

    public void Exit(AiAgent agent)
    {

    }

    public void Update(AiAgent agent)
    {
        float step = agent.creature.moveSpeed * Time.deltaTime;
        if (Utils.IsOutOfBounds(roamPosition)) roamPosition = GetRoamingPosition(agent);
        agent.creature.transform.position = Vector3.MoveTowards(agent.creature.transform.position, roamPosition, step);
        agent.creature.transform.LookAt(roamPosition, Vector3.up);
        if (Vector3.Distance(agent.creature.transform.position, roamPosition) < 1f) roamPosition = GetRoamingPosition(agent);
        if (isFoundFood && prey != null) preyPosition = prey.transform.position;
        if (Vector3.Distance(agent.creature.transform.position, preyPosition) < 1f && isFoundFood && prey != null)
        {
            //Debug.Log("Chased");
            prey.Kill();
            isFoundFood = false;
            agent.creature.hunger = 0;
            agent.creature.isHungry = false;
            //foodPosition = GetRoamingPosition(agent);
            agent.stateMachine.ChangeState(AiStateId.Wander);
        }
        //if(isFoundFood && !food.isReadyToEat) isFoundFood = false;

        agent.creature.roamPosition = roamPosition;
    }
    private Vector3 GetRoamingPosition(AiAgent agent)
    {
        if (isFoundFood && prey != null) return prey.transform.position;
        //else 
        return agent.transform.position + Utils.GetRandomDir() * agent.creature.senseRadius;
    }
    public void OnTriggerEnter(AiAgent agent, Collider other)
    {
        if (other.gameObject.CompareTag("Creature") && !isFoundFood && other.GetComponent<Creature>().creatureType == CreatureType.Herbivore)
        {
            //Debug.Log("Found");
            prey = other.GetComponent<Creature>();
            prey.predatorCreature = agent.creature;
            
            isFoundFood = true;
        }
    }
}
