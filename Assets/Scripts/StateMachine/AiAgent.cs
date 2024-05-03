using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public Creature creature;
    public AiStateId currentState;


    void Start()
    {
        creature = GetComponent<Creature>();
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiWanderState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiHungryState());
        stateMachine.RegisterState(new AiLookingForMate());
        stateMachine.ChangeState(initialState);
        creature.Init();

    }

    // Update is called once per frame
    void Update()
    {
        currentState = stateMachine.currentState;
        stateMachine.Update();
    }
    void OnTriggerEnter(Collider other)
    {
        stateMachine?.OnTriggerEnter(other);
    }
}
