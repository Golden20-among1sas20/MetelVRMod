using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerSpace : MonoBehaviour
{
    public enum MethodTriggerEnum { PlaySound, PlayerSay, MakeNoise, SafePlace, CantStand, Electricity, Trigger, EndGame, ActiveObj, UnityEvent, ManiacSay, Damage, Trap, StartTimeline, CantChase, PowerUseIfNotUsedInChasing, Gas, StopChaseAndTimeline, CatchWithoutRaycast };

    [SerializeField] MethodTriggerEnum MethodTrigger;
    [SerializeField] string ForTag = "Player";
    [SerializeField] bool SingleUse = true;
    [SerializeField] AudioClip Clip;
    [SerializeField] GameObject ActiveObj;
    [SerializeField] UnityEvent NewEvent;
    [SerializeField] float DelayBetweenDamage;
    [SerializeField] Interactive TrapTarget;
    [SerializeField] UnityEngine.Playables.PlayableDirector TimelineForPlay;
    [SerializeField] Interactive ForPowerUse;

    AudioSource Audio;
    List<AudioClip> Clips = new List<AudioClip> ();
    Animation Anim;
    public bool Used { get; private set;} = false;
    float Waiting = 0f;

    private void Start ()
    {
        switch (MethodTrigger) {
            case MethodTriggerEnum.PlayerSay:
                Audio = FP_Controller.instance.Mouth;
                Clips.Add (Resources.Load<AudioClip> ("Voice acting/" + PlayerManager.instance.PlayerName + "/" + "Rus" + "/" + Clip.name));
                Clips.Add (Resources.Load<AudioClip> ("Voice acting/" + PlayerManager.instance.PlayerName + "/" + "Eng" + "/" + Clip.name));
                break;
        }
    }

    private void Update ()
    {
        switch (MethodTrigger) {
            case MethodTriggerEnum.Damage:
                if (Used) Waiting -= Time.deltaTime;
                if (Waiting <= 0f && Used) {
                    PlayerManager.instance.Damage ();
                    Waiting = DelayBetweenDamage;
                }
                break;
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.tag == ForTag) Trigger ();
    }

    private void OnTriggerExit (Collider other)
    {
        if (other.tag == ForTag) UnTrigger ();
    }

    public void Trigger ()
    {
        if (SingleUse && Used) return;

        Used = true;
        switch (MethodTrigger) {
            case MethodTriggerEnum.PlayerSay:
                Audio.clip = Clips[PlayerPrefs.GetInt ("LanguageNum", 0)];
                Audio.Play ();
                Subtitles.instance.NewSubtitle (Audio.clip);
                break;
            case MethodTriggerEnum.MakeNoise:
                Noise.instance.MakeNoise ();
                break;
            case MethodTriggerEnum.SafePlace:
                Noise.instance.InSafePlace ();
                break;
            case MethodTriggerEnum.CantStand:
                FP_Controller.instance.SetCantStand (true);
                break;
            case MethodTriggerEnum.Electricity:
                PlayerManager.instance.ElectricityPermaDeath ();
                break;
            case MethodTriggerEnum.Gas:
                PlayerManager.instance.GasPermaDeath ();
                break;
            case MethodTriggerEnum.EndGame:
                PlayerManager.instance.EndGame ();
                break;
            case MethodTriggerEnum.ActiveObj:
                ActiveObj.SetActive (true);
                break;
            case MethodTriggerEnum.UnityEvent:
                NewEvent.Invoke ();
                break;
            case MethodTriggerEnum.ManiacSay:
                ManiacLogic.instance.Say (Clip);
                break;
            case MethodTriggerEnum.Damage:
                if (Waiting <= 0) PlayerManager.instance.Damage ();
                Waiting = DelayBetweenDamage;
                break;
            case MethodTriggerEnum.Trap:
                if (!TrapTarget.IsUsed ()) {
                    //PlayerManager.instance.Damage ();
                    FP_Controller.instance.trapped = true;
                    TrapTarget.Use (true);
                }
                break;
            case MethodTriggerEnum.StartTimeline:
                TimelineForPlay.Play ();
                break;
            case MethodTriggerEnum.CantChase:
                ManiacLogic.instance.CanChase = false;
                break;
            case MethodTriggerEnum.PowerUseIfNotUsedInChasing:
                if (ManiacLogic.instance.IsChasing) {
                    ForPowerUse.PowerUseIfNotUsed ();
                }
                break;
            case MethodTriggerEnum.StopChaseAndTimeline:
                ManiacLogic.instance.WaitHere ();
                ManiacLogic.instance.StopChase ();
                break;
            case MethodTriggerEnum.CatchWithoutRaycast:
                if (Vector3.Distance (ManiacLogic.instance.transform.position, PlayerHead.instance.transform.position) < 2) {
                    ManiacLogic.instance.Kill ();
                }
                break;
        }
    }

    public void UnTrigger ()
    {
        if (SingleUse && Used) return;

        switch (MethodTrigger) {
            case MethodTriggerEnum.CantStand:
                FP_Controller.instance.SetCantStand (false);
                break;
            case MethodTriggerEnum.SafePlace:
                Noise.instance.NotInSafePlace ();
                break;
            case MethodTriggerEnum.UnityEvent:
                NewEvent.Invoke ();
                break;
            case MethodTriggerEnum.ManiacSay:
                ManiacLogic.instance.Say (Clip);
                break;
            case MethodTriggerEnum.Damage:
                Waiting = 0;
                break;
            case MethodTriggerEnum.CantChase:
                if (Used) ManiacLogic.instance.CanChase = true;
                break;
        }

        Used = false;
    }
}
