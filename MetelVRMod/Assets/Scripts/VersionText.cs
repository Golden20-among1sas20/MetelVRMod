using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VersionText : MonoBehaviour
{
    private void Start ()
    {
        if (GetComponent<TextMeshProUGUI> () != null) {
            GetComponent<TextMeshProUGUI> ().text = Application.version.ToString ();
        }
        else if (GetComponent<Text> () != null) {
            GetComponent<Text> ().text = Application.version.ToString ();
        }
    }
}
