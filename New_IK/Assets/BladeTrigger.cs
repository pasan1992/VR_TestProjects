using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class BladeTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEvent OnScabberdEnter;
    public UnityEvent OnScabberdLeave;

    private bool isColliding;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        var tag = other.tag;
        switch(tag)
        {
            case "Sabbered":
            OnScabberdEnter.Invoke();
            break;
            case "EnemySword":
            isColliding = true;
            break;
            default:
            Debug.Log(tag);
            break;
        }
    }

    private void OnTriggerExit(Collider other) {
        var tag = other.tag;
        switch(tag)
        {
            case "Sabbered":
                OnScabberdLeave.Invoke();
            break;
            case "EnemySword":
                isColliding = true;
            break;
            default:
                Debug.Log(tag);
            break;
        }       
    }


    private void OnTriggerStay(Collider other)
    {
        if (isColliding)
        {
            ContactPoint[] contacts = new ContactPoint[1];
            other.ClosestPointOnBounds(this.transform.position);

            Debug.Log("Collision point: " + contacts[0].point);

            isColliding = false;
        }
    }
}
