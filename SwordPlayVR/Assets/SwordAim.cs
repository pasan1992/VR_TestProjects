using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAim : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform reference;
    public Transform aim;

    public Vector3 up;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        reference.LookAt(aim.transform.position,up);
    }
}
