using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [System.Serializable]
    public class SceneList
    {
        public string Name;
        public string Character;
        public AudioSource AudioSrc;
        public GameObject Obj;
        public string PlayerPrefForEnable;
        public Button ForStart;
        public TextMeshProUGUI ForStartText;
    }

    [SerializeField] GameObject PanelStartGame;
    [SerializeField] GameObject PanelLoading;
    [SerializeField] GameObject PanelLanguage;
    [SerializeField] GameObject PanelPrivacy;
    [SerializeField] GameObject PanelAnonimInfo;
    [SerializeField] GameObject ButtonPrivacy;
    [SerializeField] GameObject PanelDevelopers;
    [SerializeField] SceneList[] ScenesObj;
    [SerializeField] GameObject LoadingCircle;
    [SerializeField] GameObject TapToContinue;
    [SerializeField] GameObject[] HideForPC;
    [SerializeField] GameObject[] ShowForPC;
    [SerializeField] GameObject MainPanel;
    [SerializeField] GameObject Store;
    [SerializeField] Button NoAdsButton;
    [SerializeField] Text NoAdsCost;
    //[SerializeField] GameObject PsychopathMaskBuy;
    //[SerializeField] GameObject PsychopathMaskChoose;
    AudioSource AudioSrc;

    AsyncOperation NewScene;
    bool IsDone = false;

    static MainMenu _instance;
    public static MainMenu instance { get { return _instance; } }

    private void Awake ()
    {
        if (_instance == null) _instance = this;
        else Debug.LogError ("More than one MainMenu");
    }

    private void Start ()
    {
#if UNITY_STANDALONE
        for (int i = 0; i < HideForPC.Length; i++) HideForPC[i].SetActive (false);
        for (int i = 0; i < ShowForPC.Length; i++) ShowForPC[i].SetActive (true);
#else
        for (int i = 0; i < HideForPC.Length; i++) HideForPC[i].SetActive (true);
        for (int i = 0; i < ShowForPC.Length; i++) ShowForPC[i].SetActive (false);
#endif
#if (UNITY_ANDROID)
        if (Time.time < 5) {
            MainPanel.SetActive (false);
            Store.SetActive (true);
        }
        else {
            if (PlayerPrefs.GetInt ("WinLevelA", 0) == 1 ^ PlayerPrefs.GetInt ("WinLevelB", 0) == 1) {
                PanelStartGame.SetActive (true);
                MainPanel.SetActive (false);
            }
        }
        if (PlayerPrefs.GetInt ("WinLevelA", 0) == 1 && PlayerPrefs.GetInt ("WinLevelB", 0) == 1 && PlayerPrefs.GetInt ("EndGame", 0) == 1) {
            MainPanel.SetActive (false);
            Store.SetActive (false);
            PanelDevelopers.SetActive (true);
            PlayerPrefs.SetInt ("EndGame", 0);
        }
        NoAdsButton.interactable = (PlayerPrefs.GetInt ("NoAds", 0) == 0);
        if (PlayerPrefs.GetInt ("NoAds", 0) == 1) {
            NoAdsCost.text = Settings.instance.GetTranslatedPhrase ("Приобретено");
        }
        //PsychopathMaskBuy.SetActive (PlayerPrefs.GetInt ("PsychopathMask", 0) == 0);
        //PsychopathMaskChoose.SetActive (PlayerPrefs.GetInt ("PsychopathMask", 0) == 1);
        PurchaseManager.OnPurchaseNonConsumable += PurchaseManager_OnPurchaseNonConsumable;
#endif
        if (PlayerPrefs.GetInt ("FirstLanguage", 0) == 0) {
            PanelLanguage.SetActive (true);
            MainPanel.SetActive (false);
            //Store.SetActive (false);
        }
#if (UNITY_ANDROID)
        if (PlayerPrefs.GetInt ("Privacy", 0) == 0) {
            PanelPrivacy.SetActive (true);
        }
        ButtonPrivacy.SetActive (true);
#else
        ButtonPrivacy.SetActive (false);
#endif
        if (PlayerPrefs.GetInt ("Statistic", 0) == 0) {
            PanelAnonimInfo.SetActive (true);
        }
        foreach (SceneList SL in ScenesObj) {
            if (SL.PlayerPrefForEnable != "") {
                if (PlayerPrefs.GetInt (SL.PlayerPrefForEnable, 0) == 0) {
                    SL.ForStart.interactable = false;
                    SL.ForStartText.text = Settings.instance.GetTranslatedPhrase ("Недоступно");
                }
            }
        }
    }

    //private void LateUpdate ()
    //{
    //    if (PurchaseManager.CheckBuyState (PurchaseManager.instance.NC_PRODUCTS[0])) {
    //        PlayerPrefs.SetInt ("NoAds", 1);
    //        NoAdsButton.interactable = (PlayerPrefs.GetInt ("NoAds", 0) == 0);
    //        if (PlayerPrefs.GetInt ("NoAds", 0) == 1) {
    //            NoAdsCost.text = Settings.instance.GetTranslatedPhrase ("Приобретено");
    //        }
    //    }
    //    if (PurchaseManager.CheckBuyState (PurchaseManager.instance.NC_PRODUCTS[1])) {
    //        PlayerPrefs.SetInt ("PsychopathMask", 1);
    //        //PsychopathMaskBuy.SetActive (PlayerPrefs.GetInt ("PsychopathMask", 0) == 0);
    //        //PsychopathMaskChoose.SetActive (PlayerPrefs.GetInt ("PsychopathMask", 0) == 1);
    //    }
    //}

    //private void PurchaseManager_OnPurchaseNonConsumable (PurchaseEventArgs args)
    //{
    //    switch (args.purchasedProduct.definition.id) {
    //        case "disable_ads":
    //            PlayerPrefs.SetInt ("NoAds", 1);
    //            NoAdsButton.interactable = (PlayerPrefs.GetInt ("NoAds", 0) == 0);
    //            NoAdsCost.text = Settings.instance.GetTranslatedPhrase ("Приобретено");
    //            foreach (ManiacMask Temp in GameObject.FindObjectsOfType<ManiacMask> ()) Temp.Start ();
    //            foreach (ManiacSkin Temp in GameObject.FindObjectsOfType<ManiacSkin> ()) Temp.Start ();
    //            break;
    //        case "psychopath_mask":
    //            PlayerPrefs.SetInt ("PsychopathMask", 1);
    //            //PsychopathMaskBuy.SetActive (PlayerPrefs.GetInt ("PsychopathMask", 0) == 0);
    //            //PsychopathMaskChoose.SetActive (PlayerPrefs.GetInt ("PsychopathMask", 0) == 1);
    //            break;
    //    }
    //}

    public void StartLevel (string Name)
    {
        //Ads.instance.ShowAd ();
        //Statistic.Instance.SendStatistic ("Start" + Name);
        StartCoroutine (StartLevelAsync (Name));
    }

    IEnumerator StartLevelAsync (string Name)
    {
        PanelStartGame.SetActive (false);
        PanelLoading.SetActive (true);
        for (int i = 0; i < ScenesObj.Length; i++) {
            if (ScenesObj[i].Name == Name) {
                ScenesObj[i].Obj.SetActive (true);
                AudioSrc = ScenesObj[i].AudioSrc;
                List<AudioClip> Clips = new List<AudioClip> ();
                Clips.Add (Resources.Load<AudioClip> ("Voice acting/" + ScenesObj[i].Character + "/" + "Rus" + "/" + AudioSrc.clip.name));
                Clips.Add (Resources.Load<AudioClip> ("Voice acting/" + ScenesObj[i].Character + "/" + "Eng" + "/" + AudioSrc.clip.name));
                AudioSrc.clip = Clips[PlayerPrefs.GetInt ("LanguageNum", 0)];
                AudioSrc.Play ();
                break;
            }
        }
        yield return null;
        NewScene = SceneManager.LoadSceneAsync (Name, LoadSceneMode.Single);
        NewScene.allowSceneActivation = false;
        while (NewScene.progress < 0.9f) {
            LoadingCircle.transform.Rotate (new Vector3 (0, 0, 360 * Time.deltaTime));
            yield return null;
        }
        if (NewScene.progress >= 0.9f) {
            LoadingCircle.SetActive (false);
            TapToContinue.SetActive (true);
        }
    }

    public void ContinueLevel ()
    {
        NewScene.allowSceneActivation = true;
    }

    public void CloseGame ()
    {
        Application.Quit ();
    }

    public void OpenVK ()
    {
        string url = "https://vk.com/linkedsquad";
        Application.OpenURL (url);
    }

    public void OpenYouTube ()
    {
        string url = "https://www.youtube.com/channel/UCNCy53LrCVk5PBME-2_vYtg?view_as=subscriber";
        Application.OpenURL (url);
    }

    public void OpenEMail ()
    {
        string url = "mailto:linkedsquad.help@gmail.com?subject=FromMetel";
        Application.OpenURL (url);
    }

    public void OpenInst ()
    {
        string url = "https://www.instagram.com/nar_qoteak/";
        Application.OpenURL (url);
    }

    public void OpenTopsy()
    {
        string url = "https://www.youtube.com/user/Icqmag";
        Application.OpenURL(url);
    }

    public void SetLanguage (int Num)
    {
        Settings.instance.SetLanguage (Num);
        PlayerPrefs.SetInt ("FirstLanguage", 1);
        PanelLanguage.SetActive (false);
        //MainPanel.SetActive (true);
#if UNITY_STANDALONE
        MainPanel.SetActive (true);
        Store.SetActive (false);
        PanelPrivacy.SetActive (false);
#endif
    }

    public void SetStatisticPref (int Num)
    {
        PlayerPrefs.SetInt ("Statistic", Num);
        PanelAnonimInfo.SetActive (false);
    }
}
