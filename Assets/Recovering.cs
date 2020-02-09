using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recovering : State
{
    public override void HandleInput()
    {
        throw new System.NotImplementedException();
    }


    public Recovering(PlayerController controller, StateMachine stateMachine, string stateName) : base(controller, stateMachine, stateName)
    {

    }

    public override void OnStateEnter()
    {

    }

    public override void OnStateExit()
    {

    }
}
