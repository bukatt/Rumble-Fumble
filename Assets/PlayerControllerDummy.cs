using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerDummy : MonoBehaviour
{
    private Rigidbody myRigidbody;
    InputActions inputAction;
    Vector2 movementInput;
    public float movementSpeed = 100;
    public LayerMask myLM;
    private ConfigurableJoint cf;
    private bool tackle = false;
    private bool fallen = false;
    public float tackleForce = .5f;
    public Animator animator;
    private JointTargetDisabler jtd;
    private JointDrive jd;
    private JointDrive jd0;
    private Collider myCollider;
    private Collision recentCollision;
    public float tackledForce=100;
   // public bool fallen = false;
    //private Camera camera;
    private void Awake()
    {
        //inputAction = new InputActions();
       // inputAction.PlayerControls.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        cf = GetComponent<ConfigurableJoint>();
        jtd = GetComponent<JointTargetDisabler>();
        //inputAction.PlayerControls.Tackle.performed += ctx => tackle = ctx.ReadValue<float>();

        //set joint drive values
        jd = new JointDrive();
        jd0 = new JointDrive();
        jd.positionSpring = 10000;
        jd.positionDamper = 200;
        jd.maximumForce = 10000;
        jd0.positionDamper = 0;
        jd0.positionSpring = 0;
        jd0.maximumForce = 0;


        //disable collisions with controller collider and self
        myCollider = GetComponent<Collider>();
        foreach (Collider d in gameObject.GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(myCollider, d);
        }

        

    }
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //print(movementInput.x);
        float h = movementInput.x;
        float v = movementInput.y;
        //if (!fallen && !tackle)
          //  Movement(h, v);


        if (myRigidbody.velocity.magnitude <= .00001f)
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            
            animator.SetBool("isWalking", true);
        }

        if (tackle)
        {
            myRigidbody.velocity = cf.targetRotation * Vector3.forward * tackleForce;
        }
        if (fallen)
        {
            myRigidbody.AddForceAtPosition(recentCollision.impulse * -1 * 200, recentCollision.transform.position, ForceMode.Force);
        }
        //print(h);
        // float angle = Vector3.Angle(Vector3.zero, new Vector3(h, 0, v));

        //myRigidbody.rotation = Quaternion.Euler(0, angle, 0);
        //myRigidbody.MoveRotation(Quaternion.AngleAxis(angle, Vector3.up));
    }
    public void Tackle()
    {
        print("tackle");
        cf.angularXMotion = ConfigurableJointMotion.Free;
        cf.angularZMotion = ConfigurableJointMotion.Free;
        tackle = true;
        //myRigidbody.AddForce(cf.targetRotation*Vector3.forward*tackleForce, ForceMode.Force);
        StartCoroutine("TackleRoutine");
    }
    public void disableControl()
    {
        fallen = true;
    }
    public void enableControl()
    {
        fallen = false;
    }

    public void Movement(float h, float v)
    {

        float angleRad = Mathf.Atan2(h, v);
        //myRigidbody.AddForce(movementSpeed * h, 0, movementSpeed * v);
        myRigidbody.velocity = new Vector3(h, 0, v) * movementSpeed;
        float angle = Mathf.Rad2Deg * angleRad;
        

        //angle -= 90;
        //print(angle);
        if (h != 0 && v != 0)
        {

            Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);

            cf.targetRotation = q;

        }
    }
   /* private void OnEnable()
    {
        inputAction.Enable();
    }
    private void OnDisable()
    {
        inputAction.Disable();
    }
    */
    public void goLimp(Collision collision, float time)
    {
        if (tackle)
        {
            myRigidbody.velocity = Vector3.zero;
        }
        else
        {
            jtd.DisableJoints();
            myRigidbody.constraints = RigidbodyConstraints.None;
            cf.slerpDrive = jd0;
            cf.angularXMotion = ConfigurableJointMotion.Free;
            cf.angularZMotion = ConfigurableJointMotion.Free;
            //myRigidbody.AddForceAtPosition(collision.impulse * -1*200, collision.transform.position, ForceMode.Force);
            //myRigidbody.velocity = collision.impulse.normalized * -1 * tackledForce;
            disableControl();
            object[] prams = new object[1] { time };
            fallen = true;
            recentCollision = collision;
            StartCoroutine("StandUp", prams);
        }
    }



    IEnumerator StandUp(object[] prams)
    {
        yield return new WaitForSeconds((float)prams[0]);
        print("standing up");
        jtd.EnableJoints();
        //topRigidbody.constraints = RigidbodyConstraints.None;
        //topRigidbody.MoveRotation(Quaternion.identity);
        cf.slerpDrive = jd;
        enableControl();
        cf.angularXMotion = ConfigurableJointMotion.Locked;
        cf.angularZMotion = ConfigurableJointMotion.Locked;
        fallen = false;

    }

    IEnumerator TackleRoutine()
    {
        yield return new WaitForSeconds(.3f);
        cf.angularXMotion = ConfigurableJointMotion.Locked;
        cf.angularZMotion = ConfigurableJointMotion.Locked;
        tackle = false;

    }

    private void OnCollisionEnter(Collision collision)
    {
        /*print(collision.gameObject.layer);
        if (collision.gameObject.layer == myLM)
        {
            float collisionForce = collision.impulse.magnitude;
            print(collisionForce);
            if (collisionForce > 10)
            {
                print(collision.transform.name);
                myRigidbody.constraints = RigidbodyConstraints.None;
            }
        }*/
    }
}
