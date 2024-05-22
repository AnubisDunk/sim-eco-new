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
        if (agent.creature.creatureType == CreatureType.Herbivore) GlobalEventManager.SendHerbivoreKilled();
        if (agent.creature.creatureType == CreatureType.Carnivore) GlobalEventManager.SendCarnivoreKilled();

    }

    public void Exit(AiAgent agent)
    {

    }
    public void Update(AiAgent agent)
    {
    }

    public void OnTriggerEnter(AiAgent agent, Collider other)
    {

    }
}
