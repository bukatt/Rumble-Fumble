using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointTargetDisabler : MonoBehaviour
{
    private JointDrive jd0;
    private JointDrive jd;
    // Start is called before the first frame update
    void Start()
    {
        jd0 = new JointDrive();
        jd0.positionDamper = 100;
        jd0.positionSpring = 5;
        jd0.maximumForce = 100;
        jd = new JointDrive();
        jd.positionDamper = 200;
        jd.positionSpring= 10000;
        jd.maximumForce = 10000;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisableJoints()
    {
        foreach(JointTargeter jt in GetComponentsInChildren<JointTargeter>())
        {
            jt.enabled = false;
       
            jt.gameObject.GetComponent<ConfigurableJoint>().slerpDrive = jd0;
        }
    }
    
    public void EnableJoints()
    {

        foreach (JointTargeter jt in GetComponentsInChildren<JointTargeter>())
        {
            jt.enabled = true;
            jt.gameObject.GetComponent<ConfigurableJoint>().slerpDrive = jd;
        }
    }
}
