using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFleeing : AiState
{
    private Vector3 fleePosition;
    public Transform predator;
    //private Creature creature;
    public AiStateId GetId()
    {
        return AiStateId.Fleeing;
    }
    public void Enter(AiAgent agent)
    {
        Debug.Log("Flee");
        fleePosition = GetFleePosition(agent);
        //creature = agent.creature;
    }

    public void Exit(AiAgent agent)
    {

    }
    public void Update(AiAgent agent)
    {
        float step = agent.creature.moveSpeed * Time.deltaTime;
        agent.creature.transform.position = Vector3.MoveTowards(agent.creature.transform.position, fleePosition, step);
        agent.creature.transform.LookAt(fleePosition, Vector3.up);
        if (Vector3.Distance(agent.creature.transform.position, fleePosition) < 1f) fleePosition = GetFleePosition(agent);
        if (Utils.IsOutOfBounds(fleePosition)) fleePosition = GetRoamingPosition(agent);
        agent.creature.roamPosition = fleePosition;

    }

    private Vector3 GetRoamingPosition(AiAgent agent)
    {
        return agent.transform.position + Utils.GetRandomDir() * agent.creature.senseRadius;
    }
    private Vector3 GetFleePosition(AiAgent agent)
    {
       // Debug.Log("GetFlee");
        Vector3 opposite = new Vector3(1, 0, 1).normalized;
        return agent.transform.position + opposite;// * agent.creature.senseRadius;
    }

    public void OnTriggerEnter(AiAgent agent, Collider other)
    {

    }
}


