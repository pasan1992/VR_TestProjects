using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTraget;
    public Transform rigTarget;
    public Vector3 trakingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        rigTarget.position = vrTraget.TransformPoint(trakingPositionOffset);
        rigTarget.rotation = vrTraget.rotation * Quaternion.Euler(trackingRotationOffset);

    }
}
public class VRRig : MonoBehaviour
{
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;


    public Transform headConstrain;
    public Vector3 headBodyOffset;
    public bool EnableTracking = true;
    void Start()
    {
        headBodyOffset = transform.position - headConstrain.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = headConstrain.position + headBodyOffset;
        //transform.forward = Vector3.ProjectOnPlane(headConstrain.forward,Vector3.up).normalized;

        if(EnableTracking)
        {
            head.Map();
            leftHand.Map();
            rightHand.Map();
        }

    }
}
