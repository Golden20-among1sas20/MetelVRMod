using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePhisicsAfterHide : MonoBehaviour
{
    [SerializeField] GameObject Target;

    private void OnDisable ()
    {
        Target.GetComponent<Rigidbody> ().isKinematic = false;
    }
}
