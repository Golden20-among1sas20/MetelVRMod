using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    [System.Serializable]
    public class Dialog
    {
        public AudioSource AudioSrc;
        public AudioClip Clip;
        public string TalkerName;
        public bool NeedTranslate;
    }

    [System.Serializable]
    public class DialogBranch
    {
        public Dialog[] Branch;
    }

    [SerializeField] DialogBranch[] Dialogs;

    int DialogNum = 0;
    int CurrentReplic = 0;
    bool IsTalking = false;
    bool Interrapt = false;

    static DialogSystem _instance;
    public static DialogSystem instance { get { return _instance; } }

    private void Awake ()
    {
        if (_instance == null) {
            _instance = this;
        }
    }

        private void Update ()
    {
        if (IsTalking) {
            if (Interrapt && !ManiacLogic.instance.ManiacNear ()) {
                Dialogs[DialogNum].Branch[CurrentReplic].AudioSrc.Play ();
                Interrapt = false;
            }
            if (!Interrapt) {
                if (Noise.instance.makedNoise) { // Нашумели, временно прерываем диалог
                    Dialogs[DialogNum].Branch[CurrentReplic].AudioSrc.Stop ();
                    Interrapt = true;
                }
                else {
                    if (!Dialogs[DialogNum].Branch[CurrentReplic].AudioSrc.isPlaying) { // Завершена реплика, берем следующую
                        if (CurrentReplic + 1 < Dialogs[DialogNum].Branch.Length) { // Следующая реплика существует
                            CurrentReplic++;
                            SetReplic ();
                        }
                        else {
                            IsTalking = false;
                        }
                    }
                }
            }
        }
    }

    public void StartDialog (int Num)
    {
        DialogNum = Num;
        IsTalking = true;
        CurrentReplic = 0;
        SetReplic ();
    }

    void SetReplic ()
    {
        if (Dialogs[DialogNum].Branch[CurrentReplic].NeedTranslate) {
            List<AudioClip> Clips = new List<AudioClip> ();
            Clips.Add (Resources.Load<AudioClip> ("Voice acting/" + Dialogs[DialogNum].Branch[CurrentReplic].TalkerName + "/" + "Rus" + "/" + Dialogs[DialogNum].Branch[CurrentReplic].Clip.name));
            Clips.Add (Resources.Load<AudioClip> ("Voice acting/" + Dialogs[DialogNum].Branch[CurrentReplic].TalkerName + "/" + "Eng" + "/" + Dialogs[DialogNum].Branch[CurrentReplic].Clip.name));
            Dialogs[DialogNum].Branch[CurrentReplic].AudioSrc.clip = Clips[PlayerPrefs.GetInt ("LanguageNum", 0)];
            Dialogs[DialogNum].Branch[CurrentReplic].AudioSrc.Play ();
            Subtitles.instance.NewSubtitle (Dialogs[DialogNum].Branch[CurrentReplic].AudioSrc.clip);
        }
        else {
            Dialogs[DialogNum].Branch[CurrentReplic].AudioSrc.clip = Dialogs[DialogNum].Branch[CurrentReplic].Clip;
            Dialogs[DialogNum].Branch[CurrentReplic].AudioSrc.Play ();
        }
    }

    public void GamePaused ()
    {
        if (IsTalking) {
            Dialogs[DialogNum].Branch[CurrentReplic].AudioSrc.Pause ();
        }
    }

    public void GameUnPaused ()
    {
        if (IsTalking) {
            Dialogs[DialogNum].Branch[CurrentReplic].AudioSrc.UnPause ();
        }
    }
}
