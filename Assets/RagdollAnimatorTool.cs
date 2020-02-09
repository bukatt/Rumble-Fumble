using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RagdollAnimatorTool : EditorWindow
{
    GameObject ragdoll;
    GameObject animated;
    GameObject toEdit;
    float springValue;
    float dampValue;
    float maxforce;
    bool worldspace;
    [MenuItem("Window/RagdollTool")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<RagdollAnimatorTool>("RagdollTool");
    }
    private void OnGUI()
    {
        GUILayout.Label("Spring", EditorStyles.boldLabel);
        springValue = EditorGUILayout.FloatField(springValue);
        GUILayout.Label("Damper", EditorStyles.boldLabel);
        dampValue = EditorGUILayout.FloatField(dampValue);
        GUILayout.Label("Max Force", EditorStyles.boldLabel);
        maxforce = EditorGUILayout.FloatField(maxforce);
        GUILayout.Label("Ragdoll", EditorStyles.boldLabel);
        ragdoll = (GameObject)EditorGUILayout.ObjectField(ragdoll, typeof(GameObject), true);
        GUILayout.Label("Animated", EditorStyles.boldLabel);
        animated = (GameObject)EditorGUILayout.ObjectField(animated, typeof(GameObject), true);

        GUILayout.Label("To Edit", EditorStyles.boldLabel);
        toEdit = (GameObject)EditorGUILayout.ObjectField(toEdit, typeof(GameObject), true);

        GUILayout.Label("WorldSpace", EditorStyles.boldLabel);
        worldspace = EditorGUILayout.Toggle(worldspace);

        if (GUILayout.Button("Generate"))
        {
            if (ragdoll != null && animated != null)
            {
                createRagdoll();
            } else
            {
                ShowNotification(new GUIContent("Error"));
            }
        }

        if (GUILayout.Button("Edit"))
        {
            if (toEdit!=null)
            {
                editValues();
            }
            else
            {
                ShowNotification(new GUIContent("Error"));
            }
        }

    }
    private void editValues()
    {
        foreach (ConfigurableJoint j in toEdit.transform.gameObject.GetComponentsInChildren<ConfigurableJoint>())
        {
            JointDrive jd = new JointDrive();
            jd.positionDamper = dampValue;
            jd.positionSpring = springValue;
            jd.maximumForce = maxforce;
            j.slerpDrive = jd;
            j.configuredInWorldSpace = worldspace;
            j.swapBodies = true;
        }
    }
    private void createRagdoll()
    {
        Dictionary<string, GameObject> animatedDict = new Dictionary<string, GameObject>();
        foreach (Transform t in animated.transform.gameObject.GetComponentsInChildren<Transform>())
        {
            if (t.gameObject != animated.gameObject)
            {
                animatedDict.Add(t.gameObject.name, t.gameObject);
            }
            
        }
        ragdoll.gameObject.AddComponent<Rigidbody>();
        //ragdoll.transform.gameObject.AddComponent<Rigidbody>();
        //ragdoll.transform.gameObject.AddComponent<DisableColliders>();
        foreach (Transform t2 in ragdoll.transform.gameObject.GetComponentsInChildren<Transform>())
        {
            if (animatedDict.ContainsKey(t2.transform.name)){
                Rigidbody rb=t2.transform.gameObject.AddComponent<Rigidbody>();
                ConfigurableJoint cf=t2.transform.gameObject.AddComponent<ConfigurableJoint>();
                JointTargeter jt=t2.transform.gameObject.AddComponent<JointTargeter>();
                t2.transform.gameObject.AddComponent<CapsuleCollider>();
                
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                rb.mass = 3;
                rb.drag = 1;
                
                cf.xMotion = ConfigurableJointMotion.Locked;
                cf.yMotion = ConfigurableJointMotion.Locked;
                cf.zMotion = ConfigurableJointMotion.Locked;
                cf.connectedBody = t2.parent.GetComponent<Rigidbody>();
                cf.rotationDriveMode = RotationDriveMode.Slerp;
                JointDrive jd = new JointDrive();
                jd.positionDamper = dampValue;
                jd.positionSpring = springValue;
                jd.maximumForce = maxforce;
                cf.projectionMode = JointProjectionMode.None;
                cf.slerpDrive = jd;
                cf.configuredInWorldSpace = true;

                jt.target =animatedDict[t2.transform.name].transform;
                jt.thisJoint = cf;
               
            }
        }
        GameObject hipTarget = new GameObject();
        hipTarget.transform.parent = ragdoll.transform.parent;
        Rigidbody rb2 = hipTarget.AddComponent<Rigidbody>();
        FixedJoint fj = hipTarget.AddComponent<FixedJoint>();
        fj.connectedBody = ragdoll.GetComponent<Rigidbody>();
        fj.transform.position = ragdoll.transform.position;
        rb2.useGravity = false;
        ConfigurableJoint aj=ragdoll.AddComponent<ConfigurableJoint>();
        JointDrive jd2 = new JointDrive();
        jd2.positionDamper = dampValue;
        jd2.positionSpring = springValue;
        jd2.maximumForce = maxforce;
        aj.slerpDrive = jd2;
        aj.xMotion = ConfigurableJointMotion.Locked;
        aj.yMotion = ConfigurableJointMotion.Locked;
        aj.zMotion = ConfigurableJointMotion.Locked;
        aj.connectedBody = rb2;
        aj.rotationDriveMode = RotationDriveMode.Slerp;
        aj.projectionMode = JointProjectionMode.None;
        aj.configuredInWorldSpace = true;
        

       // ragdoll.AddComponent<DisableColliders>();
    }

    private void createRagdoll2() {
        Dictionary<string, GameObject> animatedDict = new Dictionary<string, GameObject>();
        GameObject prevGO = animated.gameObject;
        foreach (Transform t in animated.transform.gameObject.GetComponentsInChildren<Transform>())
        {
            if (t.gameObject != animated.gameObject)
            {
                GameObject tg = new GameObject();
                tg.transform.parent = t.gameObject.transform.parent;
                t.gameObject.transform.parent = tg.transform;
                tg.transform.name = t.transform.name;
                foreach (Transform child in t.transform)
                {
                    child.parent = tg.transform;
                }
                animatedDict.Add(tg.gameObject.name, tg.gameObject);
            }

        }
        ragdoll.gameObject.AddComponent<Rigidbody>();
        //ragdoll.transform.gameObject.AddComponent<Rigidbody>();
        //ragdoll.transform.gameObject.AddComponent<DisableColliders>();
        foreach (Transform t2 in ragdoll.transform.gameObject.GetComponentsInChildren<Transform>())
        {
            if (animatedDict.ContainsKey(t2.transform.name))
            {
                GameObject tg2 = new GameObject();
                tg2.transform.position = t2.position;
                tg2.transform.parent = t2.parent;
                
                foreach(Transform child in t2.transform)
                {
                    child.parent = tg2.transform;
                }
               
                t2.parent = tg2.transform;
                tg2.transform.name = t2.name;
                Rigidbody rb = tg2.transform.gameObject.AddComponent<Rigidbody>();
                ConfigurableJoint cf = tg2.transform.gameObject.AddComponent<ConfigurableJoint>();
                JointTargeter jt = tg2.transform.gameObject.AddComponent<JointTargeter>();
                tg2.transform.gameObject.AddComponent<CapsuleCollider>();

                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                rb.mass = 3;
                rb.drag = 1;

                cf.xMotion = ConfigurableJointMotion.Locked;
                cf.yMotion = ConfigurableJointMotion.Locked;
                cf.zMotion = ConfigurableJointMotion.Locked;
                cf.connectedBody = tg2.transform.parent.GetComponent<Rigidbody>();
                cf.rotationDriveMode = RotationDriveMode.Slerp;
                JointDrive jd = new JointDrive();
                jd.positionDamper = dampValue;
                jd.positionSpring = springValue;
                jd.maximumForce = maxforce;
                cf.projectionMode = JointProjectionMode.None;
                cf.slerpDrive = jd;
                cf.configuredInWorldSpace = true;

                jt.target = animatedDict[tg2.transform.name].transform;
                jt.thisJoint = cf;

            }
        }
        /*GameObject hipTarget = new GameObject();
        hipTarget.transform.parent = ragdoll.transform.parent;
        Rigidbody rb2 = hipTarget.AddComponent<Rigidbody>();
        FixedJoint fj = hipTarget.AddComponent<FixedJoint>();
        fj.connectedBody = ragdoll.GetComponent<Rigidbody>();
        fj.transform.position = ragdoll.transform.position;
        rb2.useGravity = false;
        ConfigurableJoint aj = ragdoll.AddComponent<ConfigurableJoint>();
        JointDrive jd2 = new JointDrive();
        jd2.positionDamper = dampValue;
        jd2.positionSpring = springValue;
        jd2.maximumForce = maxforce;
        aj.slerpDrive = jd2;
        aj.xMotion = ConfigurableJointMotion.Locked;
        aj.yMotion = ConfigurableJointMotion.Locked;
        aj.zMotion = ConfigurableJointMotion.Locked;
        aj.connectedBody = rb2;
        aj.rotationDriveMode = RotationDriveMode.Slerp;
        aj.projectionMode = JointProjectionMode.None;
        aj.configuredInWorldSpace = true;*/
    }
}
