using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State: MonoBehaviour
{
    protected PlayerController character;
    protected StateMachine stateMachine;
    public string stateName;
    protected Rigidbody rigidbody;
   
    public abstract void HandleInput();
    public void Initialize(PlayerController character, StateMachine stateMachine, string stateName)
    {
        this.character = character;
        this.stateMachine = stateMachine;
        this.stateName = stateName;
        this.rigidbody = character.myRigidbody;
        enabled = false;
    }
    public void StateExit()
    {
        enabled = false;
        OnStateExit();
    }

    public void StateEnter()
    {
        enabled = true;
        OnStateEnter();
    }
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    public State(PlayerController character=null, StateMachine stateMachine=null, string stateName=null)
    {
        this.character = character;
        this.stateMachine = stateMachine;
        this.stateName = stateName;
    }

    public void CheckImpact()
    {
        if (character.fallen)
        {
            stateMachine.ChangeState(character.fallenState);
        }
    }
}
