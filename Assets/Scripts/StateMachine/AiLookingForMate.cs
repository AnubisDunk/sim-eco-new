using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiLookingForMate : AiState
{
    private Vector3 roamPosition, matePosition;
    private Creature otherCreature;
    public bool isFoundMate = false;
    public AiStateId GetId()
    {
        return AiStateId.LookingForMate;
    }
    public void Enter(AiAgent agent)
    {
        roamPosition = GetRoamingPosition(agent);
        isFoundMate = false;
        agent.creature.desiredCreature = null;
    }

    public void Exit(AiAgent agent)
    {

    }
    public void Update(AiAgent agent)
    {
        float step = agent.creature.moveSpeed * Time.deltaTime;
        agent.creature.transform.position = Vector3.MoveTowards(agent.creature.transform.position, roamPosition, step);
        agent.creature.transform.LookAt(roamPosition, Vector3.up);
        if (Vector3.Distance(agent.creature.transform.position, roamPosition) < 1f) roamPosition = GetRoamingPosition(agent);
        if (agent.creature.desiredCreature != null)
        if (Vector3.Distance(agent.creature.transform.position, agent.creature.desiredCreature.transform.position) < 1f && isFoundMate)
        {
            //Debug.Log("Mating with female");
            agent.creature.desiredCreature.Mating();
            agent.creature.rest = 0;
            isFoundMate = false;
            agent.creature.desiredCreature = null;
            agent.stateMachine.ChangeState(AiStateId.Wander);
        }
        if (Utils.IsOutOfBounds(roamPosition)) roamPosition = GetRoamingPosition(agent);
        Debug.Log(Utils.IsOutOfBounds(roamPosition));
        agent.creature.roamPosition = roamPosition;
    }
    private Vector3 GetRoamingPosition(AiAgent agent)
    {
        if (agent.creature.isSelectedByMale) return agent.creature.transform.position;
        if (isFoundMate) return otherCreature.transform.position;
        else return agent.transform.position + Utils.GetRandomDir() * agent.creature.senseRadius;
    }

    public void OnTriggerEnter(AiAgent agent, Collider other)
    {
        if (other.gameObject.CompareTag("Creature"))
        {
            if (!agent.creature.isFemale && agent.creature.creatureType == other.GetComponent<Creature>().creatureType)
            {
                if (other.GetComponent<Creature>().agent.currentState == AiStateId.LookingForMate && !isFoundMate && agent.creature.isReadyToMate &&
                 other.GetComponent<Creature>().isFemale)
                {
                   // Debug.Log("Found female");
                    isFoundMate = true;
                    agent.creature.desiredCreature = other.GetComponent<Creature>();
                    other.GetComponent<Creature>().desiredCreature = agent.creature;
                    other.GetComponent<Creature>().isSelectedByMale = true;
                    otherCreature = other.GetComponent<Creature>();
                }
            }
        }
    }
}
