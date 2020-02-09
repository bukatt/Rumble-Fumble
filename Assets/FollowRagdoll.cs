using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRagdoll : MonoBehaviour
{
    public GameObject target;

    private void LateUpdate()
    {
        gameObject.transform.localRotation = target.transform.localRotation;
    }
}
