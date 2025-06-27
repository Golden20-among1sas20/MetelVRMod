using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    [System.Serializable]
    public class Point
    {
        public List<GameObject> list;
    }

    [System.Serializable]
    public class Deaths
    {
        public List<Point> list;
    }

    [SerializeField] public string PlayerName = "Alan";
    [SerializeField] Image BlackFade;
    [SerializeField] float FadeTime;
    [SerializeField] bool StartWithFadeOut = true;
    [SerializeField] GameObject ElectricityEffect;
    [SerializeField] AudioClip ElectricityEffectClip;
    [SerializeField] AudioClip GasEffectClip;
    [SerializeField] AudioClip GetDamageClip;
    [SerializeField] GameObject[] BloodIntensive;
    [SerializeField] GameObject[] HideForLetter;
    [SerializeField] Deaths HideAfterDeath;
    [SerializeField] Deaths ShowAfterDeath;
    [SerializeField] int[] DropItemsAfterDeath;
    [SerializeField] GameObject[] RefreshObjectsAfterDeath;
    [SerializeField] GameObject LooseWindow;
    [SerializeField] GameObject WinnerWindow;
    [SerializeField] GameObject GoodEnding;
    [SerializeField] TriggerSpace TriggerGoodEnding;
    [SerializeField] GameObject BadEnding;
    [SerializeField] TriggerSpace TriggerBadEnding;
    [SerializeField] TextMeshProUGUI TimeText;
    [SerializeField] GameObject[] HideForPC;
    [SerializeField] float TimeForReward;
    [SerializeField] GameObject Reward;
    [SerializeField] GameObject DeadMax;
    [SerializeField] float WaitAfterPunch = 2f;

    int BloodCounter = 0;
    int DeathCounter = 0;

    bool IsFading = false;
    bool FadeOut = false;
    public bool IsDead { get; private set;} = false;

    float Waiting = 0f;
    float PlayTime = 0f;

    string LevelName;

    GameObject LastLetter;

    static PlayerManager _instance;
    public static PlayerManager instance { get { return _instance; } }

    private void Awake ()
    {
        if (_instance == null) _instance = this;
        else Debug.LogError ("More than one PlayerManager");

        LevelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;

#if UNITY_STANDALONE
            foreach (GameObject Temp in HideForPC) Temp.SetActive (false);
#endif
    }

    private void Start ()
    {
        if (StartWithFadeOut) Fade (false);
        PlayTime = 0f;
        if (DeadMax != null) {
            DeadMax.SetActive (PlayerPrefs.GetInt ("MaxAlive", 0) == 0);
        }
    }

    private void Update ()
    {
        PlayTime += Time.deltaTime;
        if (IsFading) {
            Waiting += Time.deltaTime;
            if (Waiting >= FadeTime) {
                Waiting = FadeTime;
                IsFading = false;
                if (FadeOut) BlackFade.gameObject.SetActive (false);
                else { // черный экран, выполняем что-либо
                    if (BloodCounter >= 3) {
                        Dead ();
                    }
                    else {
                        if (Ads.instance != null) Ads.instance.ShowAd ();
                        foreach (int DropItem in DropItemsAfterDeath) {
                            Inventory.instance.DropItem (DropItem);
                        }
                        foreach (GameObject RefreshObject in RefreshObjectsAfterDeath) {
                            if (!RefreshObject.active) {
                                if (!(RefreshObject.GetComponent<Interactive> () != null && RefreshObject.GetComponent<Interactive> ().MethodUse == Interactive.MethodUseEnum.SwitchObj)) {
                                    RefreshObject.SetActive (true);
                                }
                            }
                            if (RefreshObject.GetComponent<Interactive> () != null) {
                                RefreshObject.GetComponent<Interactive> ().ResetAnim ();
                            }
                        }
                        for (int i = 0; i < HideAfterDeath.list[DeathCounter].list.Count; i++) HideAfterDeath.list[DeathCounter].list[i].SetActive (false);
                        for (int i = 0; i < ShowAfterDeath.list[DeathCounter].list.Count; i++) ShowAfterDeath.list[DeathCounter].list[i].SetActive (true);
                        DeathCounter++;
                        if (ElectricityEffect != null) ElectricityEffect.SetActive (false);
                        //foreach (GameObject BloodIntensiveTemp in BloodIntensive) BloodIntensiveTemp.SetActive (false);
                        StartCoroutine (FadeAndDisable (false));
                    }
                }
            }
            if (FadeOut) BlackFade.color = new Color32 (0, 0, 0, (byte)(255 * ((FadeTime - Waiting) / FadeTime)));
            else {
                BlackFade.color = new Color32 (0, 0, 0, (byte)(255 * (Waiting / FadeTime)));
            }
        }
    }

    public void Damage ()
    {
        if (IsDead) return;

        foreach (GameObject BloodIntensiveTemp in BloodIntensive) BloodIntensiveTemp.SetActive (false);
        if (BloodIntensive.Length > BloodCounter) {
            BloodIntensive[BloodCounter].SetActive (true);
        }
        else {
            BloodIntensive[BloodCounter - 1].SetActive (true);
        }
        BloodCounter++;
        FP_Controller.instance.Say (GetDamageClip);
        if (BloodCounter >= 3) {
            PlayerHead.instance.DisablePlayer (true);
            Fade (true);
            IsDead = true;
        }
    }

    public void Fade (bool Flag)
    {
        IsFading = true;
        FadeOut = !Flag;
        BlackFade.gameObject.SetActive (true);
        Waiting = 0f;
        if (FadeOut) BlackFade.color = new Color32 (0, 0, 0, 255);
        else BlackFade.color = new Color32 (0, 0, 0, 0);
    }

    public void InstaFade (bool Flag)
    {
        BlackFade.gameObject.SetActive (true);

        if (Flag) BlackFade.color = new Color32 (0, 0, 0, 255);
        else BlackFade.color = new Color32 (0, 0, 0, 0);
    }

    IEnumerator FadeAndDisable (bool Flag)
    {
        yield return new WaitForSeconds (5);
        ManiacLogic.instance.RefreshManiac ();
        PlayerHead.instance.DisablePlayer (Flag);
        Fade (Flag);
    }

    public void Electricity ()
    {
        //Statistic.Instance.SendStatistic ("Electo");
        ElectricityEffect.SetActive (true);
        FP_Controller.instance.Mouth.clip = ElectricityEffectClip;
        FP_Controller.instance.Mouth.Play ();
        foreach (GameObject BloodIntensiveTemp in BloodIntensive) BloodIntensiveTemp.SetActive (false);
        if (BloodIntensive.Length > BloodCounter) {
            BloodIntensive[BloodCounter].SetActive (true);
        }
        else {
            BloodIntensive[BloodCounter - 1].SetActive (true);
        }
        BloodCounter++;
        PlayerHead.instance.DisablePlayer (true);
        Fade (true);
    }

    public void Gas ()
    {
        //Statistic.Instance.SendStatistic ("Gas");
        FP_Controller.instance.Mouth.clip = GasEffectClip;
        FP_Controller.instance.Mouth.Play ();
        foreach (GameObject BloodIntensiveTemp in BloodIntensive) BloodIntensiveTemp.SetActive (false);
        if (BloodIntensive.Length > BloodCounter) {
            BloodIntensive[BloodCounter].SetActive (true);
        }
        else {
            BloodIntensive[BloodCounter - 1].SetActive (true);
        }
        BloodCounter++;
        PlayerHead.instance.DisablePlayer (true);
        Fade (true);
    }

    public IEnumerator Punched ()
    {
        foreach (GameObject BloodIntensiveTemp in BloodIntensive) BloodIntensiveTemp.SetActive (false);
        if (BloodIntensive.Length > BloodCounter) {
            BloodIntensive[BloodCounter].SetActive (true);
        }
        else {
            BloodIntensive[BloodCounter - 1].SetActive (true);
        }
        BloodCounter++;
        PlayerHead.instance.DisablePlayer (true);
        InstaFade (true);

        yield return new WaitForSeconds (WaitAfterPunch);

        IsFading = true;
        Waiting = FadeTime;
        FadeOut = false;
    }

    public void GasPermaDeath ()
    {
        FP_Controller.instance.Mouth.clip = GasEffectClip;
        FP_Controller.instance.Mouth.Play ();
        PlayerHead.instance.DisablePlayer (true);
        BloodCounter = 9999;
        IsDead = true;
        Fade (true);
    }

    public void ElectricityPermaDeath ()
    {
        //Statistic.Instance.SendStatistic ("ElectoPermaDeath");
        ElectricityEffect.SetActive (true);
        FP_Controller.instance.Mouth.clip = ElectricityEffectClip;
        FP_Controller.instance.Mouth.Play ();
        PlayerHead.instance.DisablePlayer (true);
        BloodCounter = 9999;
        IsDead = true;
        Fade (true);
    }

    public void PermaDeath ()
    {
        //Statistic.Instance.SendStatistic ("PermaDeath");
        PlayerHead.instance.DisablePlayer (true);
        BloodCounter = 9999;
        IsDead = true;
        Fade (true);
    }

    public void ChangeFog ()
    {
        RenderSettings.fogStartDistance = -0.5f;
        RenderSettings.fogEndDistance = 0.5f;
    }

    public void Dead ()
    {
        BlackFade.gameObject.SetActive (false);
        LooseWindow.SetActive (true);
        if (!FP_Input.instance.UseMobileInput) {
            Cursor.visible = true;
            Screen.lockCursor = false;
        }
        IsDead = true;
    }

    public void EndGame ()
    {
        //Statistic.Instance.SendStatistic ("EndGame" + LevelName);
        switch (LevelName) {
            case "Level_A":
                WinnerWindow.SetActive (true);
                if (TriggerGoodEnding.Used) {
                    PlayerPrefs.SetInt ("WinLevelA", 1);
                    //PlayerPrefs.SetInt ("EndGame", 1);
                    GoodEnding.SetActive (true);
                    BadEnding.SetActive (false);
                }
                else if (TriggerBadEnding.Used) {
                    PlayerPrefs.SetInt ("WinLevelA", 1);
                    //PlayerPrefs.SetInt ("EndGame", 1);
                    BadEnding.SetActive (true);
                    GoodEnding.SetActive (false);
                }
                TimeText.text = ((Mathf.FloorToInt (PlayTime / 60) < 10) ? "0" + Mathf.FloorToInt (PlayTime / 60).ToString () : Mathf.FloorToInt (PlayTime / 60).ToString ()) + ":" + (((PlayTime % 60) < 10) ? "0" + (Mathf.FloorToInt (PlayTime % 60)).ToString () : (Mathf.FloorToInt (PlayTime % 60)).ToString ());
                if (!FP_Input.instance.UseMobileInput) {
                    Cursor.visible = true;
                    Screen.lockCursor = false;
                }
                PlayerHead.instance.DisablePlayer (true);
                if (TimeForReward >= PlayTime) {
                    Reward.GetComponent<MaskOfManiac> ().TakeMask ();
                }
                break;

            case "Level_B":
                WinnerWindow.SetActive (true);
                PlayerPrefs.SetInt ("WinLevelB", 1);
                PlayerPrefs.SetInt ("EndGame", 1);
                if (TriggerGoodEnding.Used) {
                    PlayerPrefs.SetInt ("MaxAlive", 1);
                    GoodEnding.SetActive (true);
                    BadEnding.SetActive (false);
                }
                else if (TriggerBadEnding.Used) {
                    PlayerPrefs.SetInt ("MaxAlive", 0);
                    BadEnding.SetActive (true);
                    GoodEnding.SetActive (false);
                }
                TimeText.text = ((Mathf.FloorToInt (PlayTime / 60) < 10) ? "0" + Mathf.FloorToInt (PlayTime / 60).ToString () : Mathf.FloorToInt (PlayTime / 60).ToString ()) + ":" + (((PlayTime % 60) < 10) ? "0" + (Mathf.FloorToInt (PlayTime % 60)).ToString () : (Mathf.FloorToInt (PlayTime % 60)).ToString ());
                if (!FP_Input.instance.UseMobileInput) {
                    Cursor.visible = true;
                    Screen.lockCursor = false;
                }
                PlayerHead.instance.DisablePlayer (true);
                ManiacLogic.instance.StopChase ();
                if (TimeForReward >= PlayTime) {
                    Reward.GetComponent<MaskOfManiac> ().TakeMask ();
                }
                break;
        }
    }

    public void ShowLetter (GameObject Letter)
    {
        foreach (GameObject HideForLetterTemp in HideForLetter) HideForLetterTemp.SetActive (false);
        Letter.SetActive (true);
        LastLetter = Letter;
    }

    public void HideLetter ()
    {
        LastLetter.SetActive (false);
        foreach (GameObject HideForLetterTemp in HideForLetter) HideForLetterTemp.SetActive (true);
    }

    public bool IsShowLetter ()
    {
        if (LastLetter == null) return false;
        return LastLetter.active;
    }

    public bool IsShocked ()
    {
        return BlackFade.gameObject.active;
    }
}
