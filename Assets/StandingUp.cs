using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingUp : State
{
    Coroutine c;
    JointDrive jd;
    float timer = 0f;
    public override void HandleInput()
    {
        
    }

    private void FixedUpdate()
    {
        CheckImpact();
        timer += Time.deltaTime;
        jd.positionDamper = character.jd.positionDamper*Mathf.Exp(timer);
        jd.positionSpring = character.jd.positionSpring*Mathf.Exp(timer);
        jd.maximumForce = character.jd.maximumForce*Mathf.Exp(timer);
    }

    public StandingUp(PlayerController controller, StateMachine stateMachine, string stateName) : base(controller, stateMachine, stateName)
    {

    }

    public override void OnStateEnter()
    {
        jd.positionDamper = 0;
        jd.positionSpring = 0;
        jd.maximumForce = 0;
        character.jtd.EnableJoints();
        character.cf.slerpDrive = character.jdGrad;
        c = StartCoroutine("StandUp");
    }

    public override void OnStateExit()
    {
        StopCoroutine(c);
    }
    IEnumerator StandUp()
    {
        yield return new WaitForSeconds(2f);
        character.cf.angularXMotion = ConfigurableJointMotion.Locked;
        character.cf.angularZMotion = ConfigurableJointMotion.Locked;
        character.cf.slerpDrive = character.jd;
        stateMachine.ChangeState(character.idleState);

    }
}
