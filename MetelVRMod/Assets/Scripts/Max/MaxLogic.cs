using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MaxLogic : MonoBehaviour
{
    public List<Footsteps> footsteps = new List<Footsteps> ();
    [SerializeField] PlayableDirector TimelineEscape;
    [SerializeField] PlayableDirector TimelineOpenDoor;
    [SerializeField] PlayableDirector TimelineContinueEscape;
    [SerializeField] TriggerSpace PlayerPlaceForOpenDoor;
    [SerializeField] AudioSource Mouth;
    [SerializeField] AudioSource LeftStepSrc;
    [SerializeField] AudioSource RightStepSrc;

    PlayableDirector ChoosedTimeline;

    bool IsPlaying = false;

    static MaxLogic _instance;
    public static MaxLogic instance { get { return _instance; } }

    private void Awake ()
    {
        if (_instance == null) _instance = this;
        else Debug.LogError ("More than one MaxLogic");
    }

    private void Update ()
    {
        if (IsPlaying) {
            if (ChoosedTimeline.state == PlayState.Paused) {
                if (ChoosedTimeline == TimelineEscape) {
                    if (PlayerPlaceForOpenDoor.Used) {
                        ChoosedTimeline = TimelineOpenDoor;
                        ChoosedTimeline.Play ();
                        return;
                    }
                    else {
                        ChoosedTimeline = TimelineContinueEscape;
                        ChoosedTimeline.Play ();
                        return;
                    }
                }
                IsPlaying = false;
            }
        }
    }

    public void StartEscape ()
    {
        ChoosedTimeline = TimelineEscape;
        ChoosedTimeline.Play ();
        IsPlaying = true;
    }

    public void KillMax ()
    {
        if (ChoosedTimeline != null) ChoosedTimeline.Stop ();
        IsPlaying = false;
    }

    public void Say (AudioClip Clip)
    {
        List<AudioClip> Clips = new List<AudioClip> ();
        Clips.Add (Resources.Load<AudioClip> ("Voice acting/Max/" + "Rus" + "/" + Clip.name));
        Clips.Add (Resources.Load<AudioClip> ("Voice acting/Max/" + "Eng" + "/" + Clip.name));
        Mouth.clip = Clips[PlayerPrefs.GetInt ("LanguageNum", 0)];
        Mouth.Play ();
        Subtitles.instance.NewSubtitle (Mouth.clip);
    }

    #region Step Clips
    public void LeftStep ()
    {
        RaycastHit hit;
        if (Physics.Raycast (transform.position + new Vector3 (0, 1, 0), Vector3.down, out hit, 2)) {
            for (int i = 0; i < footsteps.Count; i++) {
                if (footsteps[i].SurfaceTag == hit.collider.tag) {
                    // pick & play a random footstep sound from the array,
                    // excluding sound at index 0
                    int randomStep = Random.Range (1, footsteps[i].stepSounds.Length);
                    LeftStepSrc.clip = footsteps[i].stepSounds[randomStep];
                    LeftStepSrc.Play ();

                    // move picked sound to index 0 so it's not picked next time
                    footsteps[i].stepSounds[randomStep] = footsteps[i].stepSounds[0];
                    footsteps[i].stepSounds[0] = LeftStepSrc.clip;

                    break;
                }
            }
        }
    }

    public void RightStep ()
    {
        RaycastHit hit;
        if (Physics.Raycast (transform.position + new Vector3 (0, 1, 0), Vector3.down, out hit, 2)) {
            for (int i = 0; i < footsteps.Count; i++) {
                if (footsteps[i].SurfaceTag == hit.collider.tag) {
                    // pick & play a random footstep sound from the array,
                    // excluding sound at index 0
                    int randomStep = Random.Range (1, footsteps[i].stepSounds.Length);
                    RightStepSrc.clip = footsteps[i].stepSounds[randomStep];
                    RightStepSrc.Play ();

                    // move picked sound to index 0 so it's not picked next time
                    footsteps[i].stepSounds[randomStep] = footsteps[i].stepSounds[0];
                    footsteps[i].stepSounds[0] = RightStepSrc.clip;

                    break;
                }
            }
        }
    }
    #endregion
}
