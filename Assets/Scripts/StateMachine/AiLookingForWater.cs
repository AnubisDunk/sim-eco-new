using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiLookingForWater : AiState
{
    private Vector3 roamPosition;
    public Transform creature;

    public bool isFoundWater = false;
    private Vector3 waterPosition;

    public AiStateId GetId()
    {
        return AiStateId.LookingForWater;
    }
    public void Enter(AiAgent agent)
    {
        roamPosition = GetRoamingPosition(agent);
        isFoundWater = false;
    }
    private Vector3 GetRoamingPosition(AiAgent agent)
    {
        if(isFoundWater) return waterPosition;
        return agent.transform.position + Utils.GetRandomDir() * agent.creature.senseRadius;
    }

    public void Exit(AiAgent agent)
    {
        
    }


    public void OnTriggerEnter(AiAgent agent,Collider other)
    {
        if (other.gameObject.CompareTag("Water") && !isFoundWater)
        {
            waterPosition = other.transform.position;
            isFoundWater = true;
        } 
    }

    public void Update(AiAgent agent)
    {
        float step = agent.creature.moveSpeed * Time.deltaTime;
        agent.creature.transform.position = Vector3.MoveTowards(agent.creature.transform.position, roamPosition, step);
        agent.creature.transform.LookAt(roamPosition, Vector3.up);
        if (Vector3.Distance(agent.creature.transform.position, roamPosition) < 1f) roamPosition = GetRoamingPosition(agent);
        if (Vector3.Distance(agent.creature.transform.position, waterPosition) < 1f && isFoundWater)
        {
            agent.creature.thirst = 0;
            isFoundWater = false;
            waterPosition = GetRoamingPosition(agent);
            agent.stateMachine.ChangeState(AiStateId.Wander);
        }

        if (Utils.IsOutOfBounds(roamPosition)) roamPosition = GetRoamingPosition(agent);
        agent.creature.roamPosition = roamPosition;
    }
    
}
