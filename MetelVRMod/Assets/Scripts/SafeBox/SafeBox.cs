using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SafeBox : MonoBehaviour
{
    [System.Serializable]
    public class ForNum
    {
        public Sprite Icon;
        public int Num;
    }

    [SerializeField] GameObject[] ForHide;
    [SerializeField] GameObject ForAds;
    [SerializeField] InputField LoginField;
    [SerializeField] ForNum[] Numbers;
    [SerializeField] GameObject[] NumbersPlace;
    [SerializeField] Interactive UnlockSafeBox;
    [SerializeField] GameObject[] HideAfterRightPassword;
    [Header ("Messages")]
    [HideInInspector] string Password;

    int TryCount = 0;
    float LastCloseTime = 0f;
    float Waiting = -30f;

    static SafeBox _instance;
    public static SafeBox instance { get { return _instance; } }

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
        Password = "";
        int Index = Random.Range (0, Temp.Count);
        Password += Temp[Index];
        Sprite TempSprite = null;
        foreach (ForNum TempSprt in Numbers) {
            if (TempSprt.Num == Temp[Index]) {
                TempSprite = TempSprt.Icon;
            }
        }
        NumbersPlace[0].GetComponent<SpriteRenderer> ().sprite = TempSprite;
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
        Temp.RemoveAt (Index);
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
            //ForAds.SetActive (false);
        }
        else {
            //ForAds.SetActive (TryCount >= 2);
        }
        foreach (GameObject Temp in ForHide) Temp.SetActive (false);
        PlayerHead.instance.DisablePlayer (true);
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
            UnlockSafeBox.ChangeRequiereItem (-1);
            foreach (GameObject Temp in HideAfterRightPassword) Temp.SetActive (false);
            gameObject.SetActive (false);
        }
        else {
            TryCount++;
            if (TryCount >= 2 && FP_Input.instance.UseMobileInput) {
                //ForAds.SetActive (true);
            }
        }
    }

    public void SkipPassword ()
    {
        if (Waiting + 30 >= Time.time) return;

        Ads.instance.ShowRewardedVideoSkip ();
    }

    public void ContinueSkipPassword ()
    {
        LoginField.text = Password;
        TryLogin ();
    }
}
