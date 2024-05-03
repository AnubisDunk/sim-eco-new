using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateId
{
    Idle,
    Wander,
    Dead,
    LookingForFood,
    LookingForWater,
    LookingForMate,
    LookingForPrey,
    Hunting,
    Fleeing
}
public interface AiState{
    AiStateId GetId();
    void Enter(AiAgent agent);
    void Update(AiAgent agent);
    void Exit(AiAgent agent);
    void OnTriggerEnter(AiAgent agent,Collider other);
}

    
