using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Terminal : MonoBehaviour
{
    [System.Serializable]
    public class ForNum
    {
        public Sprite Icon;
        public int Num;
    }
    [SerializeField] GameObject[] ForHide;
    [SerializeField] GameObject LoginPage;
    [SerializeField] GameObject ControllerPage;
    [SerializeField] GameObject ForAds;
    [SerializeField] InputField LoginField;
    [SerializeField] GameObject Alert;
    [SerializeField] GameObject ButtonElevatorSwitchOn;
    [SerializeField] GameObject ButtonDoor;
    [SerializeField] Interactive ElectricalDoor;
    [SerializeField] Text Console;
    [SerializeField] Interactive Power220;
    [SerializeField] Interactive Elevator;
    [SerializeField] AudioClip AdviceForDoorClip;
    [SerializeField] Interactive[] RequieredForAdvice;
    [SerializeField] GameObject RequieredItemForAdvice;
    [SerializeField] ForNum[] Numbers;
    [SerializeField] Interactive[] NumbersPlace;
    [Header("Messages")]
    [HideInInspector] string Password;
    [SerializeField] string Welcome;
    [SerializeField] string Open;
    [SerializeField] string Close;
    [SerializeField] string DoorIsOpened;
    [SerializeField] string DoorIsClosed;
    [SerializeField] string TryAgainAfter30sec;
    [SerializeField] string ElevatorStarted;
    [SerializeField] string NoPower;

    bool IsLoggedIn = false;
    int TryCount = 0;
    float LastCloseTime = 0f;
    float Waiting = -30f;

    static Terminal _instance;
    public static Terminal instance { get { return _instance; } }

    private void Awake ()
    {
        if (_instance == null) {
            _instance = this;
        }

        List<int> Temp = new List<int> ();
        Temp.Add (1);
        Temp.Add (2);
        Temp.Add (3);
        Temp.Add (4);
        Temp.Add (5);
        Temp.Add (6);
        Temp.Add (7);
        Temp.Add (8);
        Temp.Add (9);
        Temp.Add (0);
        Password = PlayerPrefs.GetString ("TerminalPassword", "");
        if (Password == "") {
            int Index = Random.Range (0, Temp.Count);
            Password += Temp[Index];
            Sprite TempSprite = null;
            foreach (ForNum TempSprt in Numbers) {
                if (TempSprt.Num == Temp[Index]) {
                    TempSprite = TempSprt.Icon;
                }
            }
            NumbersPlace[0].GetComponent<SpriteRenderer> ().sprite = TempSprite;
            NumbersPlace[0].InfoMessage = Temp[Index].ToString ();
            Temp.RemoveAt (Index);
            Index = Random.Range (0, Temp.Count);
            Password += Temp[Index];
            TempSprite = null;
            foreach (ForNum TempSprt in Numbers) {
                if (TempSprt.Num == Temp[Index]) {
                    TempSprite = TempSprt.Icon;
                }
            }
            NumbersPlace[1].GetComponent<SpriteRenderer> ().sprite = TempSprite;
            NumbersPlace[1].InfoMessage = Temp[Index].ToString ();
            Temp.RemoveAt (Index);
            Index = Random.Range (0, Temp.Count);
            Password += Temp[Index];
            TempSprite = null;
            foreach (ForNum TempSprt in Numbers) {
                if (TempSprt.Num == Temp[Index]) {
                    TempSprite = TempSprt.Icon;
                }
            }
            NumbersPlace[2].GetComponent<SpriteRenderer> ().sprite = TempSprite;
            NumbersPlace[2].InfoMessage = Temp[Index].ToString ();
            Temp.RemoveAt (Index);
            Index = Random.Range (0, Temp.Count);
            Password += Temp[Index];
            TempSprite = null;
            foreach (ForNum TempSprt in Numbers) {
                if (TempSprt.Num == Temp[Index]) {
                    TempSprite = TempSprt.Icon;
                }
            }
            NumbersPlace[3].GetComponent<SpriteRenderer> ().sprite = TempSprite;
            NumbersPlace[3].InfoMessage = Temp[Index].ToString ();
            Temp.RemoveAt (Index);
            PlayerPrefs.SetString ("TerminalPassword", Password);
        }
        else {
            Sprite TempSprite = null;
            foreach (ForNum TempSprt in Numbers) {
                if (TempSprt.Num == char.GetNumericValue (Password[0])) {
                    TempSprite = TempSprt.Icon;
                }
            }
            NumbersPlace[0].GetComponent<SpriteRenderer> ().sprite = TempSprite;
            NumbersPlace[0].InfoMessage = Password[0].ToString ();
            TempSprite = null;
            foreach (ForNum TempSprt in Numbers) {
                if (TempSprt.Num == char.GetNumericValue (Password[1])) {
                    TempSprite = TempSprt.Icon;
                }
            }
            NumbersPlace[1].GetComponent<SpriteRenderer> ().sprite = TempSprite;
            NumbersPlace[1].InfoMessage = Password[1].ToString ();
            TempSprite = null;
            foreach (ForNum TempSprt in Numbers) {
                if (TempSprt.Num == char.GetNumericValue (Password[2])) {
                    TempSprite = TempSprt.Icon;
                }
            }
            NumbersPlace[2].GetComponent<SpriteRenderer> ().sprite = TempSprite;
            NumbersPlace[2].InfoMessage = Password[2].ToString ();
            TempSprite = null;
            foreach (ForNum TempSprt in Numbers) {
                if (TempSprt.Num == char.GetNumericValue (Password[3])) {
                    TempSprite = TempSprt.Icon;
                }
            }
            NumbersPlace[3].GetComponent<SpriteRenderer> ().sprite = TempSprite;
            NumbersPlace[3].InfoMessage = Password[3].ToString ();
        }
    }

    private void OnEnable ()
    {
        if (LastCloseTime + 0.01f > Time.time || FP_Input.instance == null) {
            gameObject.SetActive (false);
            return;
        }
        
        if (!FP_Input.instance.UseMobileInput) {
            Cursor.visible = true;
            Screen.lockCursor = false;
            ForAds.SetActive (false);
        }
        else {
            ForAds.SetActive (TryCount >= 2);
        }
        foreach (GameObject Temp in ForHide) Temp.SetActive (false);
        PlayerHead.instance.DisablePlayer (true);
        LoginPage.SetActive (!IsLoggedIn);
        ControllerPage.SetActive (IsLoggedIn);
        if (IsLoggedIn) Console.text = Settings.instance.GetTranslatedPhrase (Welcome);
        ButtonElevatorSwitchOn.SetActive (false);
        ButtonDoor.SetActive (false);
    }

    private void OnDisable ()
    {
        if (FP_Input.instance == null) return;
        if (!FP_Input.instance.UseMobileInput) {
            Cursor.visible = false;
            Screen.lockCursor = true;
        }
        foreach (GameObject Temp in ForHide) Temp.SetActive (true);
        PlayerHead.instance.DisablePlayer (false);
        LastCloseTime = Time.time;
    }

    private void Update ()
    {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            HideTerminal ();
        }
    }

    public void HideTerminal ()
    {
        gameObject.SetActive (false);
    }

    public void TryLogin ()
    {
        if (Waiting + 30 >= Time.time) return;

        if (LoginField.text == Password) {
            //Statistic.Instance.SendStatistic ("TerminalSuccess");
            IsLoggedIn = true;
            OnEnable ();
        }
        else {
            //Statistic.Instance.SendStatistic ("TerminalFail");
            TryCount++;
            if (TryCount >= 2 && FP_Input.instance.UseMobileInput) {
                ForAds.SetActive (true);
            }
            if (TryCount >= 3) { // Сигнализация
                Waiting = Time.time;
                Alert.GetComponent<Animation> ().Play ();
                Alert.GetComponent<AudioSource> ().Play ();
                Noise.instance.MakeNoise ();
                Console.text = Settings.instance.GetTranslatedPhrase (TryAgainAfter30sec);
            }
        }
    }

    public void ShowElevatorButton ()
    {
        ButtonElevatorSwitchOn.SetActive (true);
        ButtonDoor.SetActive (false);
    }

    public void ShowDoorButton ()
    {
        ButtonElevatorSwitchOn.SetActive (false);
        ButtonDoor.SetActive (true);
        ButtonDoor.transform.GetChild (0).GetComponent<TextMeshProUGUI> ().text = Settings.instance.GetTranslatedPhrase ((ElectricalDoor.Used) ? Close : Open);
    }

    public void PressButtonDoor ()
    {
        ElectricalDoor.PowerUse ();
        ButtonDoor.transform.GetChild (0).GetComponent<TextMeshProUGUI> ().text = Settings.instance.GetTranslatedPhrase ((ElectricalDoor.Used) ? Close : Open);
        Console.text = Settings.instance.GetTranslatedPhrase ((ElectricalDoor.Used) ? DoorIsOpened : DoorIsClosed);
    }

    public void PressButtonElevatorSwitchOn ()
    {
        if (Power220.Used) { // Запускаем лифт
            if (!Elevator.Used) {
                //Statistic.Instance.SendStatistic ("Elevator");
                Elevator.Use ();
                Noise.instance.MakeNoise ();
                Console.text = Settings.instance.GetTranslatedPhrase (ElevatorStarted);
                CheckAdvice ();
            }
        }
        else {
            Console.text = Settings.instance.GetTranslatedPhrase (NoPower);
        }
    }

    public void SkipPassword ()
    {
        if (Waiting + 30 >= Time.time) return;

        //Statistic.Instance.SendStatistic ("SkipTerminal");
        Ads.instance.ShowRewardedVideoSkip ();
    }

    public void ContinueSkipPassword ()
    {
        LoginField.text = Password;
        TryLogin ();
    }

    public void CheckAdvice ()
    {
        foreach (Interactive Temp in RequieredForAdvice) if (!Temp.Used) return;
        if (!RequieredItemForAdvice.active) FP_Controller.instance.SayWithDelay (AdviceForDoorClip, 3f);
    }
}
