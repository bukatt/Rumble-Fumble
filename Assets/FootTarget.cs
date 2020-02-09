using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootTarget : MonoBehaviour
{
    [SerializeField] GameObject targeter;
    [SerializeField] LayerMask layermask;
    public Vector3 targetSpot;
    private void Awake()
    {
        
       // RaycastHit hit;
       // if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layermask))
       // {
       //     targetSpot = hit.transform.position;
      //  } else
       // {
      //      targetSpot = new Vector3(transform.position.x, targeter.transform.position.y, transform.position.z);
      //  }
        
    }

    private void LateUpdate()
    {
        Vector3 tspot = new Vector3();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layermask))
        {
            //tspot = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            //print(hit.collider.gameObject.name);
        }
        else
        {
           // targetSpot = new Vector3(transform.position.x, targeter.transform.position.y, transform.position.z);
        }
        //transform.position = tspot;
    }

}
