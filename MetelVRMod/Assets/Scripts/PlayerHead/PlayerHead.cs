using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerHead : MonoBehaviour
{
    [SerializeField] KeyCode UseKey;
    [SerializeField] SteamVR_Action_Boolean UseKeyVR;
    [SerializeField] SteamVR_Behaviour_Pose HandVR;
    [SerializeField] GameObject NewSoundPrefab;

    public bool IsDisabledPlayer { get; private set;} = false;
    public bool CanUseInteractive { get; private set; } = true;


    bool IsPressed = false;
    float Waiting = 0f;
    Interactive LastInteractive;

    static PlayerHead _instance;
    public static PlayerHead instance { get { return _instance; } }

    private void Awake ()
    {
        if (_instance == null) _instance = this;
        else Debug.LogError ("More than one PlayerHead");
    }

    private void Update ()
    {
        if (IsPressed) {
            Waiting += Time.deltaTime;
        }

        if (!FP_Input.instance.UseMobileInput && !IsDisabledPlayer) {
            if (Input.GetKeyDown (UseKey)) {
                RaycastHit hit;
                if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit, 1.5f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {
                    if (hit.transform.GetComponent<Interactive> () != null) { // Указали на интерактивный объект
                        LastInteractive = hit.transform.GetComponent<Interactive> ();
                        IsPressed = true;
                        Waiting = 0f;
                    }
                }
            }
            else if (Input.GetKeyUp (UseKey)) {
                if (Waiting <= 0.3f) {
                    IsPressed = false;
                    RaycastHit hit;
                    if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit, 1.5f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {
                        if (hit.transform.GetComponent<Interactive> () != null) { // Указали на интерактивный объект
                            if (LastInteractive == hit.transform.GetComponent<Interactive> ()) {
                                hit.transform.GetComponent<Interactive> ().Use ();
                            }
                        }
                    }
                }
            }
            if (UseKeyVR.GetStateDown(HandVR.inputSource))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1.5f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform.GetComponent<Interactive>() != null)
                    { // Указали на интерактивный объект
                        LastInteractive = hit.transform.GetComponent<Interactive>();
                        IsPressed = true;
                        Waiting = 0f;
                    }
                }
            }
            else if (UseKeyVR.GetStateUp(HandVR.inputSource))
            {
                if (Waiting <= 0.3f)
                {
                    IsPressed = false;
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1.5f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                    {
                        if (hit.transform.GetComponent<Interactive>() != null)
                        { // Указали на интерактивный объект
                            if (LastInteractive == hit.transform.GetComponent<Interactive>())
                            {
                                hit.transform.GetComponent<Interactive>().Use();
                            }
                        }
                    }
                }
            }
        }
    }

    public void DisablePlayer (bool Flag)
    {
        IsDisabledPlayer = Flag;
        FP_Controller.instance.canControl = !Flag;
    }

    public void SetCanUseInteractive (bool Flag)
    {
        CanUseInteractive = Flag;
    }

    public void PlaySound (AudioClip Clip)
    {
        Instantiate (NewSoundPrefab, transform).GetComponent<NewSound> ().PlaySound (Clip);
    }
}
