using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalMoveObject : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject followObject;
    public float followSpeed = 30f;
    public float rotationSpeed = 100f;

    public Vector3 positionOffset;
    public Vector3 rotationOffset;



    private Transform _folllowTarget;
    private Rigidbody _rigidbody;
    void Start()
    {
        _folllowTarget = followObject.transform;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.mass = 20;

        _rigidbody.transform.position = _folllowTarget.transform.position;
        _rigidbody.transform.rotation = _folllowTarget.transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Position
        var positionwithoffset = _folllowTarget.position + positionOffset;
        var rel_vec = positionwithoffset - transform.position;
        _rigidbody.velocity = rel_vec * followSpeed;

        // Rotations
        var rotationwithoffset = _folllowTarget.rotation * Quaternion.Euler(rotationOffset);
        var q = rotationwithoffset * Quaternion.Inverse(_rigidbody.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        _rigidbody.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotationSpeed);
    }
}
