using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private void Start ()
    {
        int Choosed = Random.Range (0, transform.childCount);
        //Statistic.Instance.SendStatistic ("Spawn" + Choosed.ToString ());
        transform.GetChild (Choosed).gameObject.SetActive (true);
    }
}
