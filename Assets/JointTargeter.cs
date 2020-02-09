using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointTargeter : MonoBehaviour
{
    public ConfigurableJoint thisJoint;
    public Transform target;
    private Quaternion startingRotation;
    private Quaternion startingRot2;
    private Quaternion startingRotationWorld;
    ConfigurableJoint joint;
    public bool invert = false;
    private void Awake()
    {
        joint = GetComponent<ConfigurableJoint>();
        startingRotation = Quaternion.Inverse(joint.transform.localRotation);
        startingRot2 = joint.transform.localRotation;
        startingRotationWorld = joint.transform.rotation;
    }
    private void Start()
    {
        //joint.SetTargetRotationLocal(Quaternion.Euler(0, 90, 0), startingRotation);

    }
    private void LateUpdate()
    {
        if (!invert)
        {
            SetTargetRotation3();
        }
        else
        {
            SetTargetRotation4();
        }
        
    }

    private void SetTargetRotation()
    {
        ConfigurableJoint tempJoint;
        tempJoint = thisJoint;
        Quaternion targetRotation = target.transform.localRotation;
        //Quaternion startRotation = this.transform.localRotation;
        //tempJoint.targetRotation = target.transform.localRotation;

        // Calculate the rotation expressed by the joint's axis and secondary axis
        var right = thisJoint.axis;
        var forward = Vector3.Cross(thisJoint.axis, thisJoint.secondaryAxis).normalized;
        var up = Vector3.Cross(forward, right).normalized;
        Quaternion worldToJointSpace = Quaternion.LookRotation(forward, up);

        // Transform into world space
        Quaternion resultRotation = Quaternion.Inverse(worldToJointSpace);

        // Counter-rotate and apply the new local rotation.
        // Joint space is the inverse of world space, so we need to invert our value
       // if (Space.Self == Space.World)
        //{
          //  resultRotation *= startingRotation * Quaternion.Inverse(targetRotation);
        //}
        //else
        //{
            resultRotation = Quaternion.Inverse(targetRotation * startingRotation);
        //}

        // Transform back into joint space
        resultRotation *= worldToJointSpace;

        // Set target rotation to our newly calculated rotation
        thisJoint.targetRotation = resultRotation;

      //  startingRotation = Quaternion.Inverse(target.localRotation);
        //thisJoint = tempJoint;
    }

    private void SetTargetRotation2()
    {
        thisJoint.targetRotation = Quaternion.Euler(target.transform.rotation.eulerAngles);
    }

    private void SetTargetRotation3()
    {
        
        thisJoint.targetRotation = target.localRotation*startingRotation;
    }

    private void SetTargetRotation4()
    {
        joint.targetRotation = Quaternion.Inverse(target.localRotation * startingRotation) ;
    }

    private void SetTargetRotation5()
    {
        joint.targetRotation = target.rotation * startingRotationWorld;
    }

}
