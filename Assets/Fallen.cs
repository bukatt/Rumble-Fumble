using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallen : State
{
    private float timer = 0f;
    public override void HandleInput()
    {
       
    }

    private void FixedUpdate()
    {
        CheckImpact();
        Vector3 dir = character.collisionDirection.normalized;
        dir.y += 1;
        rigidbody.AddForce(Mathf.Exp(-.5f*timer)*-1000f*dir);
        timer += Time.deltaTime;
    }

    public Fallen(PlayerController controller, StateMachine stateMachine, string stateName) : base(controller, stateMachine, stateName)
    {

    }

    public override void OnStateEnter()
    {
        character.myCollider.enabled = false;
        character.jtd.DisableJoints();
        character.cf.slerpDrive = character.jd0;
        character.cf.angularXMotion = ConfigurableJointMotion.Free;
        character.cf.angularZMotion = ConfigurableJointMotion.Free;
        StartCoroutine("StandUp");
        
    }

    public override void OnStateExit()
    {
        character.myCollider.enabled = true;
        
        character.fallen = false;
    }

    IEnumerator StandUp()
    {
        yield return new WaitForSeconds(3f);
        stateMachine.ChangeState(character.standingUpState);

    }
}
