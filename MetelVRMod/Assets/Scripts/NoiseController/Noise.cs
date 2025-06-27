using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class Noise : MonoBehaviour
{
    [SerializeField] AudioSource Mouth;
    [SerializeField] AudioSource Heartbeat;
    [SerializeField] float PickOfHeartbeat;
    [SerializeField] float DurationOfHeartbeat;
    [SerializeField] AudioClip[] AlertClip;
    [SerializeField] AudioClip FirstNoiseToCageClip;
    [SerializeField] AudioClip FirstNoiseForAdviceClip;
    [SerializeField] AudioClip[] WhatIsDatNoiseClip;
    [SerializeField] AudioClip TimerMusic;
    [SerializeField] GameObject[] ChaseCause;
    [SerializeField] AlertTimer AlertTimer;
    [SerializeField] PlayableDirector TimelineNoised;
    [SerializeField] public GameObject[] SwitchOffAfterNoise;
    [SerializeField] AudioSource[] MuteAudioIfRaged;

    List<List<AudioClip>> Clips = new List<List<AudioClip>> ();

    private float WaitingNoise = 0f;
    private float WaitingHeartbeat = 0f;

    private bool IsHeartbeat = false;

    private PlayableDirector DefaultTimeline;

    public bool makedNoise { get; private set;} = false;
    bool firstNoiseToCage = true;
    bool firstNoiseForAdvice = true;
    bool Safe = false;
    public bool CanNoise = true;

    static Noise _instance;
    public static Noise instance { get { return _instance; } }

    private void Awake ()
    {
        if (_instance == null) _instance = this;
        else Debug.LogError ("More than one Noise_Controller");

        if (DurationOfHeartbeat < PickOfHeartbeat) Debug.LogError ("Wrong data of heartbeat");

        DefaultTimeline = TimelineNoised;
    }

    private void Start ()
    {
        foreach (AudioClip AClip in AlertClip) {
            List<AudioClip> NewClip = new List<AudioClip> ();
            NewClip.Add (Resources.Load<AudioClip> ("Voice acting/" + PlayerManager.instance.PlayerName + "/" + "Rus" + "/" + AClip.name));
            NewClip.Add (Resources.Load<AudioClip> ("Voice acting/" + PlayerManager.instance.PlayerName + "/" + "Eng" + "/" + AClip.name));
            Clips.Add (NewClip);
        }
    }

    private void Update ()
    {
        if (makedNoise) {
            if (IsHeartbeat) {
                WaitingHeartbeat += Time.deltaTime;
                if (WaitingHeartbeat >= DurationOfHeartbeat) {
                    Heartbeat.Stop ();
                    IsHeartbeat = false;
                }
                else {
                    if (WaitingHeartbeat <= PickOfHeartbeat) {
                        Heartbeat.volume = (WaitingHeartbeat / PickOfHeartbeat);
                    }
                    else {
                        Heartbeat.volume = (((DurationOfHeartbeat - PickOfHeartbeat) - (WaitingHeartbeat - PickOfHeartbeat)) / (DurationOfHeartbeat - PickOfHeartbeat));
                    }
                }
            }
            WaitingNoise += Time.deltaTime;
            if (firstNoiseToCage && !ManiacLogic.instance.Rage) {
                if (WaitingNoise >= 8f) {
                    firstNoiseToCage = false;
                    FP_Controller.instance.SayWithDelay (Resources.Load<AudioClip> ("Voice acting/" + PlayerManager.instance.PlayerName + "/" + Settings.instance.GetFolderLocalization () + "/" + FirstNoiseToCageClip.name));
                }
            }
            if (WaitingNoise >= 25f) { // Истекло время поиска укрытия
                if (CheckSafe ()) {
                    ManiacLogic.instance.StartEnter ();
                }
                else {
                    ManiacLogic.instance.ChaseForPlayer ();
                }
                makedNoise = false;
                CanNoise = false;
            }
        }
    }

    public bool CheckSafe ()
    {
        bool UnUsed = true;
        foreach (GameObject Temp in ChaseCause) {
            if (Temp.active && Temp.GetComponent<Interactive> ().Used) {
                UnUsed = false;
                break;
            }
        }
        return (Noise.instance.Safe && UnUsed);
    }

    public void MakeNoise (bool NeedPlayerTalkAfter = false, bool PlayerTalk = true, bool NeedManiacTalk = true) // Нашумели
    {
        if (!CanNoise) return;

        if (makedNoise) { // Если уже нашумели, то маньяк в ярости
            if (ManiacLogic.instance.Rage) return; // Если маньяк уже в ярости, то ничего не делаем
            ManiacLogic.instance.AgainMakedNoise ();
            TimelineNoised.playableGraph.GetRootPlayable (0).SetSpeed (2);
            foreach (AudioSource Temp in MuteAudioIfRaged) {
                Temp.mute = true;
            }
            WaitingNoise = 25 - (AlertTimer.Waiting / 2);
            AlertTimer.StartTimer (AlertTimer.Waiting / 2);
            AlertTimer.HideTimer = true;
            return;
        }

        for (int i = 0; i < SwitchOffAfterNoise.Length; i++) SwitchOffAfterNoise[i].SetActive (false);

        if (!NeedPlayerTalkAfter && PlayerTalk) {
            Mouth.clip = (Clips[UnityEngine.Random.Range (0, Clips.Count)])[PlayerPrefs.GetInt ("LanguageNum", 0)];
            Mouth.Play ();
            Subtitles.instance.NewSubtitle (Mouth.clip);
            Heartbeat.volume = 0;
            Heartbeat.Play ();
            WaitingHeartbeat = 0f;
            IsHeartbeat = true;
        }
        MusicManager.instance.Silence ();
        makedNoise = true;
        WaitingNoise = -2f;
        StartCoroutine (WhatIsDatNoise (NeedPlayerTalkAfter && PlayerTalk, NeedManiacTalk));
    }

    IEnumerator WhatIsDatNoise (bool NeedPlayerTalkAfter = false, bool NeedManiacTalk = true)
    {
        yield return new WaitForSeconds (1);
        if (NeedManiacTalk) ManiacLogic.instance.Say (WhatIsDatNoiseClip[Random.Range (0, WhatIsDatNoiseClip.Length)]);
        if ((TimelineNoised = ManiacLogic.instance.NeedChangeNoise ()) == null) TimelineNoised = DefaultTimeline;
        TimelineNoised.Play ();
        yield return new WaitForSeconds (1);
        if (NeedPlayerTalkAfter) {
            Mouth.clip = (Clips[UnityEngine.Random.Range (0, Clips.Count)])[PlayerPrefs.GetInt ("LanguageNum", 0)];
            Mouth.Play ();
            Subtitles.instance.NewSubtitle (Mouth.clip);
            Heartbeat.volume = 0;
            Heartbeat.Play ();
            WaitingHeartbeat = 0f;
            IsHeartbeat = true;
        }
        WaitingNoise = 0f;
        makedNoise = true;
        MusicManager.instance.NewMusic (TimerMusic);
        AlertTimer.StartTimer (25f);
    }

    public void InSafePlace ()
    {
        if (makedNoise && !PlayerManager.instance.IsDead) {
            firstNoiseToCage = false;
            if (firstNoiseForAdvice) {
                firstNoiseForAdvice = false;
                Mouth.clip = Resources.Load<AudioClip> ("Voice acting/" + PlayerManager.instance.PlayerName + "/" + Settings.instance.GetFolderLocalization () + "/" + FirstNoiseForAdviceClip.name);
                Mouth.Play ();
                Subtitles.instance.NewSubtitle (Mouth.clip);
            }
        }
        Safe = true;
    }

    public void NotInSafePlace ()
    {
        Safe = false;
    }
}
