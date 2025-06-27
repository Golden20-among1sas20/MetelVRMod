using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    [SerializeField] GameObject[] Requiere;
    [SerializeField] GameObject[] HideAfterCraft;
    [SerializeField] GameObject ShowAfterCraft;

    bool IsCrafted = false;

    private void Update ()
    {
        if (!IsCrafted) {
            bool IsFound = true;
            for (int i = 0; i < Requiere.Length; i++) {
                if (!Requiere[i].active) {
                    IsFound = false;
                    break;
                }
            }
            if (IsFound) {
                foreach (GameObject Temp in HideAfterCraft) Temp.SetActive (false);
                ShowAfterCraft.SetActive (true);
                IsCrafted = true;
            }
        }
    }
}
