using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimAfterUse : MonoBehaviour
{
    [SerializeField] Interactive Ref;
    [SerializeField] AudioSource AudioSrc;
    [SerializeField] AudioClip Clip;

    bool Used = false;

    private void Start ()
    {
        if (AudioSrc == null) AudioSrc = GetComponent<AudioSource> ();
    }

    private void Update ()
    {
        if (!Used) {
            if (Ref.Used) {
                GetComponent<Animation> ().Play ();
                if (AudioSrc != null) {
                    AudioSrc.clip = Clip;
                    AudioSrc.Play ();
                }
                Used = true;
            }
        }
    }
}
