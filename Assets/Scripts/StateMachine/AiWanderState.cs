using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiWanderState : AiState
{
    private Vector3 roamPosition;
    public Transform creature;
    public AiStateId GetId()
    {
        return AiStateId.Wander;
    }
    public void Enter(AiAgent agent)
    {
        roamPosition = GetRoamingPosition(agent);
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

        agent.creature.roamPosition = roamPosition;
    }

    private Vector3 GetRoamingPosition(AiAgent agent)
    {
        return agent.transform.position + Utils.GetRandomDir() * agent.creature.senseRadius;
    }

    public void OnTriggerEnter(AiAgent agent, Collider other)
    {
    }
}


