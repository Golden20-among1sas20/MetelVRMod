using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLooking : MonoBehaviour
{
    [SerializeField] Transform Head;
    [SerializeField] Transform Target;

    public bool IsLooking = false;

    private void Start ()
    {
        ;
    }

    private void LateUpdate ()
    {
        if (IsLooking) {
            Vector3 targetDir = Target.transform.position - Head.transform.parent.position;
            Vector3 forward = Head.transform.parent.forward;
            float angle = Vector3.Angle (targetDir, forward);
            if (angle < 70 && Vector3.Distance (Target.transform.position, Head.transform.position) < 5) {
                Head.LookAt (Target);
            }
            //else Head.transform.localRotation = Quaternion.Euler (0,0,0);
        }
    }
}
