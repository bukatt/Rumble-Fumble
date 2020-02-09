using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State CurrentState { get; private set; }

    public void Initialize(State startingState)
    {
        CurrentState = startingState;
        startingState.StateEnter();
    }

    public void ChangeState(State newState)
    {
        Debug.Log("changing state to " + newState.stateName);
        CurrentState.StateExit();

        CurrentState = newState;
        newState.StateEnter();
    }
}
