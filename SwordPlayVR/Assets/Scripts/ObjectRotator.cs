using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ObjectRotator : MonoBehaviour
{
    // Start is called before the first frame update
    public XRControllerInput leftHandController;
    public XRControllerInput rightHandController;
    public GameObject controlObject;
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

            var _axis_value_left = leftHandController.primary2DAxisValue;
            var _axis_value_right = rightHandController.primary2DAxisValue;
            Debug.Log(_axis_value_left);


            controlObject.transform.Translate(Vector3.up*_axis_value_left.x*0.001f,Space.Self);
            controlObject.transform.Translate(Vector3.forward*_axis_value_left.y*0.001f,Space.Self);


            controlObject.transform.Rotate(Vector3.left *5 * _axis_value_right.y * Time.deltaTime,Space.Self);

    }
}
