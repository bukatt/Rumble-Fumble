using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tackling : State
{
    public MonoBehaviour monoBehaviour;

    private void FixedUpdate()
    {
        //CheckImpact();
        character.myRigidbody.velocity = character.transform.localRotation * Vector3.forward * character.tackleForce;
    }

    

    public override void HandleInput()
    {
        
    }

    public Tackling(PlayerController controller, StateMachine stateMachine, string stateName) : base(controller, stateMachine, stateName)
    {

    }

    public override void OnStateEnter()
    {
        character.animator.SetBool("isTackling", true);
        character.myCollider.enabled = false;
        StartCoroutine("TackleRoutine");
    }


    IEnumerator TackleRoutine()
    {
        yield return new WaitForSeconds(.3f);
        stateMachine.ChangeState(character.idleState);

    }

    public override void OnStateExit()
    {
        character.tackle = false;
        character.fallen = false;
        character.animator.SetBool("isTackling", false);
        character.myCollider.enabled = true;
    }
}
