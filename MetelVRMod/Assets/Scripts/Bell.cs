using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour
{
    [SerializeField] AudioSource Audio;
    [SerializeField] float Cooldown;
    [SerializeField] Interactive MustBeUnused;

    float Waiting = 0f;

    private void Update ()
    {
        Waiting -= Time.deltaTime;
    }

    public void MakeNoise ()
    {
        if (Waiting <= 0f && !MustBeUnused.Used) {
            Audio.Play ();
            Noise.instance.MakeNoise ();
            Waiting = Cooldown;
        }
    }
}
