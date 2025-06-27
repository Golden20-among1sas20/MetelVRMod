using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Subtitles : MonoBehaviour
{
    [System.Serializable]
    public class Texts
    {
        public AudioClip Clip;
        public string Subtitle;
    }

    [SerializeField] TextMeshProUGUI SubtitlesText;
    [SerializeField] GameObject SubtitlesMain;
    [SerializeField] Texts[] TextsArray;

    float Waiting = 0f;
    const float TimePerSymbol = 0.07f;

    static Subtitles _instance;
    public static Subtitles instance { get { return _instance; } }

    private void Awake ()
    {
        if (_instance == null) _instance = this;
        else Debug.LogError ("More than one SubtitlesManager");
    }

    private void Start ()
    {
        SubtitlesMain.SetActive (false);
    }

    private void Update ()
    {
        if (Waiting > 0f) {
            Waiting -= Time.deltaTime;
            if (Waiting <= 0f) {
                SubtitlesMain.SetActive (false);
            }
        }
    }

    public void NewSubtitle (AudioClip NewClip)
    {
        if (PlayerPrefs.GetInt ("Subtitles", 1) == 0) return;

        string Result = "#" + NewClip.name;

        for (int i = 0; i < TextsArray.Length; i++) {
            if (TextsArray[i].Clip.name == NewClip.name) {
                Result = Settings.instance.GetTranslatedPhrase (TextsArray[i].Subtitle);
                break;
            }
        }

        SubtitlesMain.SetActive (true);
        SubtitlesText.text = Result;
        Waiting = Result.Length * TimePerSymbol;
        if (Waiting < 1f) Waiting = 1f;
    }

    public void NewAdvice (string Advice)
    {
        SubtitlesMain.SetActive (true);
        SubtitlesText.text = Advice;
        Waiting = Advice.Length * TimePerSymbol;
        if (Waiting < 1f) Waiting = 1f;
    }
}
