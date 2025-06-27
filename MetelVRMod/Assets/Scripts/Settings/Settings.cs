using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] Transform[] ForTranslate;
    [SerializeField] Toggle SubtitlesToggle;
    [SerializeField] Image Gamma;
    [SerializeField] Slider GammaSlider;
    [SerializeField] Toggle FPSToggle;
    [SerializeField] FPSDisplay FPS;
    [SerializeField] bool AddHashtag = false;

    List<string[]> Parsed = new List<string[]> ();

    static Settings _instance;
    public static Settings instance { get { return _instance; } }

    private void Awake ()
    {
        Time.maximumDeltaTime = 0.1f;

        if (_instance == null) _instance = this;
        else Debug.LogError ("More than one Settings");

        PlayerPrefs.SetInt ("LanguageNum", PlayerPrefs.GetInt ("LanguageNum", 0));

        TextAsset txt = (TextAsset)Resources.Load ("Localization/Localization", typeof (TextAsset));
        string Localization = txt.text;

        string[] AllLine = Localization.Split ('\n');
        Parsed = new List<string[]> ();
        for (int i = 0; i < AllLine.Length; i++) {
            Parsed.Add (AllLine[i].Split ('|'));
        }
    }

    private void Start ()
    {
        if (Time.timeScale != 1) Time.timeScale = 1; // Если ставили на паузу игру, то снова возвращаем нормальную скорость
        foreach (Transform ForTranslateParent in ForTranslate) TranslateAllChilds (ForTranslateParent);
        if (SubtitlesToggle != null) SubtitlesToggle.isOn = (PlayerPrefs.GetInt ("Subtitles", 1) == 1) ? true : false;
        if (GammaSlider != null) GammaSlider.value = PlayerPrefs.GetInt ("Gamma", 0);
        if (Gamma != null) Gamma.color = new Color32 (255, 255, 255, (byte)PlayerPrefs.GetInt ("Gamma", 0));
        FPS.ShowFps = PlayerPrefs.GetInt ("FPS", 1) == 1;
        if (FPSToggle != null) FPSToggle.isOn = PlayerPrefs.GetInt ("FPS", 1) == 1;
    }

    public void SetSubtitles (bool Flag)
    {
        PlayerPrefs.SetInt ("Subtitles", (Flag) ? 1 : 0);
    }

    public string GetTranslatedPhrase (string Phrase)
    {
        if (Phrase.Contains ("#")) {
            Phrase = Phrase.Substring (1);
        }
        try {
            if (int.Parse (Phrase).ToString () == Phrase) return Phrase; // Строка является числом, не переводим
        }
        catch (Exception e) {}

        if (Phrase == "") return "";

        Phrase = Phrase.Replace ("\n", "\\n");
        string Result = Phrase;
        if (AddHashtag) Result = "#" + Phrase;

        try {
            foreach (string[] Line in Parsed) {
                for (int i = 0; i < Line.Length; i++) {
                    if (Line[i].ToLower () == Phrase.ToLower ()) {
                        return Line[PlayerPrefs.GetInt ("LanguageNum", 1) + 1].Replace ("\\n", "\n");
                    }
                }
            }
        }
        catch (Exception e) {
            return "";
        }

        return Result;
    }

    public string GetFolderLocalization ()
    {
        string Result = "Rus";

        switch (PlayerPrefs.GetInt ("LanguageNum", 0)) {
            case 0:
                Result = "Rus";
                break;
            case 1:
                Result = "Eng";
                break;
        }

        return Result;
    }

    public void ChangeLanguage ()
    {
        switch (PlayerPrefs.GetInt ("LanguageNum", 0)) {
            case 0:
                PlayerPrefs.SetInt ("LanguageNum", 1);
                break;
            case 1:
                PlayerPrefs.SetInt ("LanguageNum", 0);
                break;
        }

        foreach (Transform ForTranslateParent in ForTranslate) TranslateAllChilds (ForTranslateParent);
    }

    public void SetLanguage (int Num)
    {
        PlayerPrefs.SetInt ("LanguageNum", Num);

        foreach (Transform ForTranslateParent in ForTranslate) TranslateAllChilds (ForTranslateParent);
    }

    public void TranslateAllChilds (Transform Parent)
    {
        for (int i = 0; i < Parent.childCount; i++) {
            Transform Child = Parent.GetChild (i);
            Text TempText = Child.GetComponent<Text> ();
            if (TempText != null) {
                if (TempText.text != "") TempText.text = GetTranslatedPhrase (TempText.text);
            }
            TextMeshProUGUI TempTextMesh = Child.GetComponent<TextMeshProUGUI> ();
            if (TempTextMesh != null) {
                if (TempTextMesh.text != "") TempTextMesh.text = GetTranslatedPhrase (TempTextMesh.text);
            }
            if (Child.childCount > 0) TranslateAllChilds (Child);
        }
    }

    public void RefreshLevel ()
    {
        //Ads.instance.ShowAd ();
        //if (Statistic.Instance) Statistic.Instance.SendStatistic ("Restart" + SceneManager.GetActiveScene ().name);
        PlayerPrefs.SetString ("LoadLevel", SceneManager.GetActiveScene ().name);
        SceneManager.LoadScene ("Restart");
    }

    public void ShowAdForRestart ()
    {
        if (PlayerPrefs.GetInt ("NoAds", 0) == 0) {
            if (!Ads.instance.ShowAd ()) LoadingScene.instance.ContinueLevel ();
        }
        else {
            LoadingScene.instance.ContinueLevel ();
        }
    }

    public void ShowAdForStart ()
    {
        if (PlayerPrefs.GetInt ("NoAds", 0) == 0) {
            if (!Ads.instance.ShowAd ()) MainMenu.instance.ContinueLevel ();
        }
        else {
            MainMenu.instance.ContinueLevel ();
        }
    }

    public void ChangeGamma (float NewGamma)
    {
        PlayerPrefs.SetInt ("Gamma", Mathf.CeilToInt (NewGamma));
        Gamma.color = new Color32 (255, 255, 255, (byte)Mathf.CeilToInt (NewGamma));
    }

    public void ActiveFPS (bool Flag)
    {
        PlayerPrefs.SetInt ("FPS", (Flag) ? 1 : 0);
        FPS.ShowFps = Flag;
    }

    public void QuitToMenu ()
    {
        //Ads.instance.ShowAd ();
        //Statistic.Instance.SendStatistic ("QuitToMenu");
        SceneManager.LoadScene ("MainMenu");
    }

    public void OpenFeedback ()
    {
        string url = "https://play.google.com/store/apps/details?id=com.LinkedSquad.Metel";
        Application.OpenURL (url);
    }
}
