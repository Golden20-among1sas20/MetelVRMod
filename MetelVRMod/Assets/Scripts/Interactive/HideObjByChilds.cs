using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjByChilds : MonoBehaviour
{
    private void Update ()
    {
        bool IsFound = false;
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild (i).gameObject.active) {
                IsFound = true;
                break;
            }
        }
        if (!IsFound) gameObject.SetActive (false);
    }
}
