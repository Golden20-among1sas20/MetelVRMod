using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskOfManiac : MonoBehaviour
{
    [SerializeField] string MaskName;
    [SerializeField] Animation Notification;
    [SerializeField] AudioSource NotificationSrc;

    private void Start ()
    {
        if (PlayerPrefs.GetInt (MaskName, 0) == 1) gameObject.SetActive (false);
    }

    public void TakeMask ()
    {
        if (PlayerPrefs.GetInt (MaskName, 0) == 1) return;

        gameObject.SetActive (false);
        Notification.Play ();
        NotificationSrc.Play ();
        PlayerPrefs.SetInt (MaskName, 1);
    }
}
