using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionHandler : MonoBehaviour
{
    Rigidbody myRigidbody;
    public Rigidbody topRigidbody;
    public LayerMask layerMask;
    private GameObject topGO;
    private ConfigurableJoint topCJ;
    private JointDrive jd;
    private JointDrive jd0;
    private JointTargetDisabler jtd;
    private PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        topGO = topRigidbody.gameObject;
        topCJ = topGO.GetComponent<ConfigurableJoint>();
        jd = new JointDrive();
        jd0 = new JointDrive();
        jd.positionSpring = 10000;
        jd.positionDamper = 200;
        jd.maximumForce = 10000;
        jd0.positionDamper = 0;
        jd0.positionSpring = 0;
        jd0.maximumForce = 0;
        jtd = topGO.GetComponent<JointTargetDisabler>();
        pc = topGO.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1<<collision.gameObject.layer) & layerMask)!=0)
        {
            print(collision.gameObject.layer);
            print(layerMask.value);
            float collisionForce = collision.impulse.magnitude;
            if (pc.tackle)
            {
                pc.stopMovement();
            }
            //print(collisionForce);
            else if(collisionForce > 10)
            {
                //pc.goLimp(collision);
                
            }
        }
    }

    IEnumerator StandUp()
    {
        yield return new WaitForSeconds(2f);
        print("standing up");
        jtd.EnableJoints();
        //topRigidbody.constraints = RigidbodyConstraints.None;
        //topRigidbody.MoveRotation(Quaternion.identity);
        topCJ.slerpDrive = jd;
        pc.enableControl();
        topCJ.angularXMotion = ConfigurableJointMotion.Locked;
        topCJ.angularZMotion = ConfigurableJointMotion.Locked;

    }

   
}
