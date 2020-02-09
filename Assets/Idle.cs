using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{



    private void Update()
    {
        if (character.tackle)
        {
            stateMachine.ChangeState(character.tacklingState);
        }
    }

    private void FixedUpdate()
    {
        CheckImpact();
        HandleInput();
        character.myRigidbody.velocity = Vector3.zero;
    }
    public override void HandleInput()
    {
        float h = character.h;
        float v = character.v;

        if(h!=0 || v != 0)
        {
            stateMachine.ChangeState(character.movingState);
        }
    }

    public Idle(PlayerController controller, StateMachine stateMachine, string stateName) : base (controller, stateMachine, stateName)
    {

    }

    public override void OnStateEnter()
    {
        character.myRigidbody.velocity = Vector3.zero;
    }
    
    public override void OnStateExit()
    {

    }
}
