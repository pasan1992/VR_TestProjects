using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Sabbered : MonoBehaviour
{
    private AimConstraint aimConstraint;
    void Start()
    {
        aimConstraint = GetComponent<AimConstraint>();
    }

    public void onSwordEnter()
    {
        Debug.Log("Enter");
        aimConstraint.weight = 1;
    }

    public void onSwordLeave()
    {
         Debug.Log("Leave");
        StartCoroutine(DecreaseValue());
    }

    private IEnumerator DecreaseValue()
    {
        while (aimConstraint.weight > 0)
        {
            aimConstraint.weight -= 10f * Time.deltaTime;
            yield return null;
        }
        aimConstraint.weight = 0;
    }
}
