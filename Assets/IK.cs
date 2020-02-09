using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public ConfigurableJoint thisJoint;
    public Transform target;


    private void FixedUpdate()
    {
        ConfigurableJoint tempJoint;
        tempJoint = thisJoint;
        tempJoint.targetRotation = target.transform.localRotation;
    }
}
