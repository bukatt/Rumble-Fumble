using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableColliders : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Collider c in gameObject.GetComponentsInChildren<Collider>())
        {
            //Debug.Log("Flag1");
            foreach (Collider d in gameObject.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(c, d);
            }
        }
    }

    
}
