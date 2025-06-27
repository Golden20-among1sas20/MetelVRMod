using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class FP_Input : MonoBehaviour 
{
    [SerializeField] GameObject MobileCanvas;
    [SerializeField] GameObject Crosshair;
    [SerializeField] GameObject[] ShowInMobile;

    public bool UseMobileInput = true;
    public Inputs mobileInputs;

    static FP_Input _instance;
    public static FP_Input instance { get { return _instance;} }

    private void Awake ()
    {
        if (_instance == null) _instance = this;
        else Debug.LogError ("More than one FP_Input");
    }

    private void Start ()
    {
        MobileCanvas.SetActive (UseMobileInput);
        foreach (GameObject Temp in ShowInMobile) Temp.SetActive (UseMobileInput);
        if (UseMobileInput) {
            if (Crosshair != null) Crosshair.SetActive (false);
        }
        else {
            Cursor.visible = false;
            Screen.lockCursor = true;
        }
    }

    public Vector3 MoveInput()
    {
        return mobileInputs.moveJoystick.MoveInput();
    }

    public Vector2 LookInput()
    {
        return mobileInputs.lookPad != null ? mobileInputs.lookPad.LookInput() : Vector2.zero;
    }

    public Vector2 ShotInput()
    {
        return mobileInputs.shotButton != null ? mobileInputs.shotButton.MoveInput() : Vector2.zero;
    }

    public bool Shoot()
    {
        return mobileInputs.shotButton != null ? mobileInputs.shotButton.IsPressed() : false;
    }

    public bool Reload()
    {
        return mobileInputs.reloadButton != null ? mobileInputs.reloadButton.OnRelease() : false;
    }

    public bool Run()
    {
        return mobileInputs.runButton != null ? mobileInputs.runButton.IsPressed() : false;
    }

    public bool Jump()
    {
        return mobileInputs.jumpButton != null ? mobileInputs.jumpButton.IsPressed() : false;
    }

    public bool Crouch()
    {
        return mobileInputs.crouchButton != null ? mobileInputs.crouchButton.Toggle() : false;
    }

    public void SetToggle (bool Flag)
    {
        if (mobileInputs.crouchButton != null) mobileInputs.crouchButton.SetToggle (Flag);
    }
}

[System.Serializable]
public class Inputs
{
    public FP_Joystick moveJoystick;
    public FP_Lookpad lookPad;
    public FP_Button runButton, jumpButton, crouchButton, shotButton, reloadButton;
}