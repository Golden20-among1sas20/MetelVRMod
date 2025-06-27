using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManiacMask : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ChooseText;
    [SerializeField] Button ChooseBtn;
    [SerializeField] string PlayerPrefForEnable;
    [SerializeField] public string MaskName;
    [SerializeField] int AdQuantity = 0;

    int CurrentAd = 0;

    public void Start ()
    {
        if (AdQuantity > 0) {
            CurrentAd = PlayerPrefs.GetInt (PlayerPrefForEnable + "Ad", 0);
        }

        if (((AdQuantity > 0 && PlayerPrefs.GetInt ("NoAds", 0) == 1)) || (PlayerPrefs.GetInt (PlayerPrefForEnable, 0) == 1 || PlayerPrefForEnable == "")) {
            ChooseBtn.interactable = true;
            if (PlayerPrefs.GetString ("MaskName", "GasMask") == MaskName) {
                ChooseText.text = Settings.instance.GetTranslatedPhrase ("Выбрана");
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
                ChooseText.text = Settings.instance.GetTranslatedPhrase ("Секретная");
            }
#else
            ChooseBtn.interactable = false;
            ChooseText.text = Settings.instance.GetTranslatedPhrase ("Секретная");
#endif
        }
    }

    public void Choose ()
    {
        if (PlayerPrefs.GetInt ("NoAds", 0) == 0 && (! (PlayerPrefs.GetInt (PlayerPrefForEnable, 0) == 1 || PlayerPrefForEnable == "") && AdQuantity > 0 && CurrentAd < AdQuantity)) {
            Ads.instance.ShowRewardedVideoForMaskOrSkin (MaskName);
        }
        else {
            PlayerPrefs.SetString ("MaskName", MaskName);
            foreach (ManiacMask Temp in GameObject.FindObjectsOfType<ManiacMask> ()) Temp.Start ();
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
