using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (ClearPlayerPrefs))]
public class ClearPlayerPrefsEd : Editor
{
    public override void OnInspectorGUI ()
    {
        base.OnInspectorGUI ();

        ClearPlayerPrefs script = (ClearPlayerPrefs)target;

        DrawDefaultInspector ();

        if (GUILayout.Button ("Delete Saves")) {
            script.DeleteSaves ();
        }
    }
}