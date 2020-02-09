using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable_1 : MonoBehaviour
{
    public LayerMask layerMask;
    public GameObject destructed;
    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & layerMask)!= 0)
        {
            if (collision.impulse.magnitude > 8)
            {
                Instantiate(destructed, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
