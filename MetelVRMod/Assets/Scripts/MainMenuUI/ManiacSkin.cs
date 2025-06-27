using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManiacSkin : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ChooseText;
    [SerializeField] Button ChooseBtn;
    [SerializeField] string PlayerPrefForEnable;
    [SerializeField] public string SkinName;
    [SerializeField] int AdQuantity = 0;

    int CurrentAd = 0;

    public void Start ()
    {
        if (AdQuantity > 0) {
            CurrentAd = PlayerPrefs.GetInt (PlayerPrefForEnable + "Ad", 0);
        }

        if (((AdQuantity > 0 && PlayerPrefs.GetInt ("NoAds", 0) == 1)) || (PlayerPrefs.GetInt (PlayerPrefForEnable, 0) == 1 || PlayerPrefForEnable == "")) {
            ChooseBtn.interactable = true;
            if (PlayerPrefs.GetString ("SkinName", "ClassicSkin") == SkinName) {
                ChooseText.text = Settings.instance.GetTranslatedPhrase ("Выбран");
            }
            else {
                ChooseText.text = Settings.instance.GetTranslatedPhrase ("Выбрать");
            }
        }
        else {
#if UNITY_ANDROID
            if (AdQuantity > 0) {
                ChooseBtn.interactable = true;
                ChooseText.text = Settings.instance.GetTranslatedPhrase ("Открыть") + " " + CurrentAd.ToString () + "/" + AdQuantity.ToString ();
            }
            else {
                ChooseBtn.interactable = false;
                ChooseText.text = Settings.instance.GetTranslatedPhrase ("Секретный");
            }
#else
            ChooseBtn.interactable = false;
            ChooseText.text = Settings.instance.GetTranslatedPhrase ("Секретный");
#endif
        }
    }

    public void Choose ()
    {
        if (PlayerPrefs.GetInt ("NoAds", 0) == 0 && (! (PlayerPrefs.GetInt (PlayerPrefForEnable, 0) == 1 || PlayerPrefForEnable == "") && AdQuantity > 0 && CurrentAd < AdQuantity)) {
            Ads.instance.ShowRewardedVideoForMaskOrSkin (SkinName);
        }
        else {
            PlayerPrefs.SetString ("SkinName", SkinName);
            foreach (ManiacSkin Temp in GameObject.FindObjectsOfType<ManiacSkin> ()) Temp.Start ();
        }
    }

    public void IncAdQuantity ()
    {
        PlayerPrefs.SetInt (PlayerPrefForEnable + "Ad", ++CurrentAd);
        if (CurrentAd >= AdQuantity) {
            PlayerPrefs.SetInt (PlayerPrefForEnable, 1);
        }
        Start ();
    }
}
