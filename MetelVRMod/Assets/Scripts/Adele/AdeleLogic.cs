using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AdeleLogic : MonoBehaviour
{
    [SerializeField] AudioSource Mouth;

    public void Say (AudioClip Clip)
    {
        List<AudioClip> Clips = new List<AudioClip> ();
        Clips.Add (Resources.Load<AudioClip> ("Voice acting/Adele/" + "Rus" + "/" + Clip.name));
        Clips.Add (Resources.Load<AudioClip> ("Voice acting/Adele/" + "Eng" + "/" + Clip.name));
        Mouth.clip = Clips[PlayerPrefs.GetInt ("LanguageNum", 0)];
        Mouth.Play ();
        Subtitles.instance.NewSubtitle (Mouth.clip);
    }

    public void EndTimeline (PlayableDirector Timeline)
    {
        Vector3 Pos = transform.position;
        Quaternion Quat = transform.rotation;
        Timeline.Stop ();
        transform.position = Pos;
        transform.rotation = Quat;
        GetComponent<HeadLooking> ().IsLooking = true;
    }
}
