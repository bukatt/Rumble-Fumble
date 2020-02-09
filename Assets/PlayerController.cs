using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody myRigidbody;
    InputActions inputAction;
    Vector2 movementInput;
    public float movementSpeed=100;
    public LayerMask myLM;
    public ConfigurableJoint cf;
    public bool tackle=false;
    public bool fallen = false;
    public float tackleForce = .4f;
    public Animator animator;
    public JointTargetDisabler jtd;
    public JointDrive jd;
    public JointDrive jd0;
    public Collider myCollider;
    public GameObject camera;
    private Collision myCollision;
    private bool gettingUp=false;
    public JointDrive jdGrad;
    private StateMachine stateMachine;
    public StandingUp standingUpState;
    public Recovering recoveringState;
    public Idle idleState;
    public Tackling tacklingState;
    public Moving movingState;
    public Fallen fallenState;
    public float h, v = 0f;
    public Vector3 collisionDirection;
    private void Awake()
    {
        inputAction = new InputActions();
        inputAction.PlayerControls.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
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
        jdGrad.positionDamper = 200;
        jdGrad.positionSpring = 10000;
        jdGrad.maximumForce = 10000;
        myCollider = GetComponent<Collider>();
        myRigidbody = GetComponent<Rigidbody>();

        //disable collisions with controller collider and self
        foreach (Collider d in gameObject.GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(myCollider, d);
        }

        camera = GameObject.FindGameObjectWithTag("MainCamera");
        stateMachine = new StateMachine();
        standingUpState = gameObject.AddComponent<StandingUp>();
        standingUpState.Initialize(this, stateMachine, "StandingUp");
        idleState = gameObject.AddComponent<Idle>();
        idleState.Initialize(this, stateMachine, "Idle");
        tacklingState = gameObject.AddComponent<Tackling>();
        tacklingState.Initialize(this, stateMachine, "Tackling");
        recoveringState = gameObject.AddComponent<Recovering>(); ;
        recoveringState.Initialize(this, stateMachine, "Recovering");
        movingState = gameObject.AddComponent<Moving>(); ;
        movingState.Initialize(this, stateMachine, "Moving");
        fallenState = gameObject.AddComponent<Fallen>();
        fallenState.Initialize(this, stateMachine, "Fallen");
        stateMachine.Initialize(idleState);
        


    }
    // Start is called before the first frame update
    void Start()
    {
        
        
    }
     public void stopMovement()
    {
        print("stopping movement");
        myRigidbody.velocity = Vector3.zero;
        tackle = false;
    }

    
    public void OnMove(InputValue value)
    {
        h = value.Get<Vector2>().x;
        v = value.Get<Vector2>().y;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //print(movementInput.x);
       // h = movementInput.x;
       // v = movementInput.y;
        //if (!fallen && !tackle)
         //   Movement(h, v);
        
      /* 
        if (h !=0 || v!=0)
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("speed", myRigidbody.velocity.magnitude);
            
        } else
        {
            animator.SetBool("isWalking", false);
        }

        if (tackle)
        {
            myRigidbody.velocity=transform.localRotation * Vector3.forward * tackleForce;
        } else if (fallen && myCollision!=null)
        {
            //myRigidbody.AddForce(myCollision.impulse * -100, ForceMode.Force);
            print("throwing");
            //myRigidbody.velocity = new Vector3(myCollision.impulse.x * -5, 0, myCollision.impulse.z*-5);
            
        } else if (gettingUp)
        {
            if (jdGrad.positionSpring >= jd.positionSpring)
            {
                jdGrad = jd0;
                gettingUp = false;
                cf.slerpDrive = jd;
            }
            else
            {
                jdGrad.positionDamper += 10;
                jdGrad.positionSpring += .2f;
                jdGrad.maximumForce += 10;
            }
        }
        */
        
        
        //print(h);
       // float angle = Vector3.Angle(Vector3.zero, new Vector3(h, 0, v));
       
            //myRigidbody.rotation = Quaternion.Euler(0, angle, 0);
        //myRigidbody.MoveRotation(Quaternion.AngleAxis(angle, Vector3.up));
    }
    private void Update()
    {
    }
    public void OnTackle()
    {
        print("tackle");
        tackle = true;
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
        myRigidbody.velocity = new Vector3(h, 0, v) * movementSpeed;
        float angle = Mathf.Rad2Deg * angleRad;
        angle = -angle;
        float rotationAngle = Mathf.Atan2(h, v) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
        rotationAngle = -rotationAngle;
        angle = -angle;
       
        if (h != 0 && v != 0)
        {

            Quaternion q = Quaternion.AngleAxis(rotationAngle, Vector3.up);
          
            cf.targetRotation = q;

        }
    }
    private void OnEnable()
    {
        inputAction.Enable();
    }
    private void OnDisable()
    {
        inputAction.Disable();
    }

    public void goLimp(Collision collision)
    {
        collisionDirection = collision.impulse;
        fallen = true;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print(collision.impulse.magnitude);
        if (((1 << collision.gameObject.layer) & myLM) != 0 && collision.impulse.magnitude > 7)
        {
            goLimp(collision);
        }
    }
}
