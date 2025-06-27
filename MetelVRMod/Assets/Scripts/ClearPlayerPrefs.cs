using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ClearPlayerPrefs : MonoBehaviour
{
    public void DeleteSaves ()
    {
        PlayerPrefs.DeleteAll ();
        PlayerPrefs.Save ();
    }
}