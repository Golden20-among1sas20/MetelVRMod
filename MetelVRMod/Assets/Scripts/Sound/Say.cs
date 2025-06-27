using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Say : MonoBehaviour
{
    [SerializeField] AudioSource AudioSrc;
    [SerializeField] string CharacterName;
    [SerializeField] float Delay;

    List<AudioClip> Clips = new List<AudioClip> ();

    private void Awake ()
    {
        if (AudioSrc == null) AudioSrc = GetComponent<AudioSource> ();
    }

    private void Start ()
    {
        Clips.Add (Resources.Load<AudioClip> ("Voice acting/" + CharacterName + "/" + "Rus" + "/" + AudioSrc.clip.name));
        Clips.Add (Resources.Load<AudioClip> ("Voice acting/" + CharacterName + "/" + "Eng" + "/" + AudioSrc.clip.name));
        StartCoroutine (StartPlay ());
    }

    IEnumerator StartPlay ()
    {
        yield return new WaitForSeconds (Delay);
        AudioSrc.clip = Clips[PlayerPrefs.GetInt ("LanguageNum", 0)];
        AudioSrc.Play ();
    }
}
