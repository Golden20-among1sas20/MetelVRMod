using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveNearTarget : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] GameObject ActiveObj;

    private void Update ()
    {
        if (Vector3.Distance (transform.position, Target.position) < 5) {
            ActiveObj.SetActive (true);
        }
        else {
            ActiveObj.SetActive (false);
        }
    }
}
