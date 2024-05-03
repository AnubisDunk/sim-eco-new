using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiHungryState : AiState
{
    private Vector3 roamPosition;
    public Transform creature;

    public bool isFoundFood = false;
    private Vector3 foodPosition;
    private Food food;
    public AiStateId GetId()
    {
        return AiStateId.LookingForFood;
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
        agent.creature.transform.position = Vector3.MoveTowards(agent.creature.transform.position, roamPosition, step);
        agent.creature.transform.LookAt(roamPosition, Vector3.up);
        if (Vector3.Distance(agent.creature.transform.position, roamPosition) < 1f) roamPosition = GetRoamingPosition(agent);
        if(isFoundFood && food.isReadyToEat) roamPosition = foodPosition;
        if (Vector3.Distance(agent.creature.transform.position, foodPosition) < 1f && food.isReadyToEat && isFoundFood)
        {
            food.Eat();
            agent.creature.hunger = 0;
            isFoundFood = false;
            foodPosition = GetRoamingPosition(agent);
            agent.stateMachine.ChangeState(AiStateId.Wander);
        }
        //if(isFoundFood && !food.isReadyToEat) isFoundFood = false;

        if (Utils.IsOutOfBounds(roamPosition)) roamPosition = GetRoamingPosition(agent);
        agent.creature.roamPosition = roamPosition;
    }
    private Vector3 GetRoamingPosition(AiAgent agent)
    {
        //if (isFoundFood) return foodPosition;
        //else 
        return agent.transform.position + Utils.GetRandomDir() * agent.creature.senseRadius;
    }
    public void OnTriggerEnter(AiAgent agent,Collider other)
    {
        if (other.gameObject.CompareTag("Bush") && !isFoundFood && other.gameObject.GetComponent<Food>().isReadyToEat)
        {
            //agent.creature.bushes.Add(other.gameObject);
            foodPosition = other.transform.position;
            food = other.gameObject.GetComponent<Food>();
            isFoundFood = true;
        } 
    }
}
