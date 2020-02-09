using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : State
{
    
    private void FixedUpdate()
    {
        CheckImpact();
        
        HandleInput(); 
    }

    private void Update()
    {
        if (character.tackle)
        {
            stateMachine.ChangeState(character.tacklingState);
        }
    }

    public override void HandleInput()
    {
        float h = character.h;
        float v = character.v;
        if(h!=0 || v != 0) { 
            float angleRad = Mathf.Atan2(h, v);
            character.myRigidbody.velocity = new Vector3(h, 0, v) * character.movementSpeed;
            float angle = Mathf.Rad2Deg * angleRad;
            angle = -angle;
            float rotationAngle = Mathf.Atan2(h, v) * Mathf.Rad2Deg + character.camera.transform.eulerAngles.y;
            rotationAngle = -rotationAngle;
            angle = -angle;
            Quaternion q = Quaternion.AngleAxis(rotationAngle, Vector3.up);
            character.cf.targetRotation = q;
            character.animator.SetFloat("Speed", rigidbody.velocity.magnitude);
        }
        else
        {
            stateMachine.ChangeState(character.idleState);
        }
    }

    public Moving(PlayerController controller, StateMachine stateMachine, string stateName) : base(controller, stateMachine, stateName)
    {

    }

    public override void OnStateEnter()
    {
        character.animator.SetBool("isWalking", true);
    }

    public override void OnStateExit()
    {
        character.animator.SetBool("isWalking", false);
    }

    
}
