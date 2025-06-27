using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSound : MonoBehaviour
{
    AudioSource Source;

    private void Awake ()
    {
        Source = GetComponent<AudioSource> ();
    }

    private void Update ()
    {
        if (!Source.isPlaying) Destroy (gameObject);
    }

    public void PlaySound (AudioClip Clip)
    {
        Source.clip = Clip;
        Source.Play ();
    }
}
