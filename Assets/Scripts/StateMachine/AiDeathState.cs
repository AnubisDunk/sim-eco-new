using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDeathState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Dead;
    }
    public void Enter(AiAgent agent)
    {
        agent.creature.DestroyCreature();  
        GlobalEventManager.SendCreatureKilled(); 
    }

    public void Exit(AiAgent agent)
    {

    }
    public void Update(AiAgent agent)
    {
    }

    public void OnTriggerEnter(AiAgent agent,Collider other)
    {

    }
}
