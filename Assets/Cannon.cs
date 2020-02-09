using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Rigidbody cannonball;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("launchProjectile", 2f, 2f);
    }

   private void launchProjectile()
    {
        Rigidbody instance = Instantiate(cannonball, transform.position, Quaternion.identity);
        instance.velocity = new Vector3(-40, 0, 0);
    }
}
