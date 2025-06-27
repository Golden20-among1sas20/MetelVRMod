using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Playables;
using UnityEngine.Events;

public class Interactive : MonoBehaviour
{
    public enum MethodUseEnum { PlayAnim, Item, ShowLetter, ShowMesh, PlayAnimAndActiveObj, ShowMessage, DropItem, DisactiveObj, ActiveObj, SwitchObj, ShowMessageAndSwitchObj, PlayAnimAndActivePhisics, PlayAnimAndActiveEmission, Destruction, ActivePhysics, PlayAnimAndDisactiveObj, ActiveObjAndChangeMaterial, Trap, PlayAnimAndDisactiveParticle, PlaySound, UnityEvent };

    [SerializeField] public MethodUseEnum MethodUse;
    [SerializeField] bool SingleUse = true;
    [SerializeField] bool CanUnUse = true;
    [SerializeField] bool HideColliderAfterUse = false;
    [SerializeField] bool DisattachParentAfterUse = false;
    [SerializeField] bool IsUsedFromStart = false;
    [SerializeField] public Animation Anim;
    [SerializeField] AnimationClip ResetAnimClip;
    [SerializeField] AudioSource AudioSrc;
    [SerializeField] AudioClip SuccessAudio;
    [SerializeField] AudioClip Audio;
    [SerializeField] AudioClip CloseAudio;
    [SerializeField] AudioClip FailedAudio;
    [SerializeField] AudioClip FailedAudioSay;
    [SerializeField] GameObject RequierePlace;
    [SerializeField] int RequiereItem = -1;
    [SerializeField] GameObject ShowObjAfterUseItem;
    [SerializeField] GameObject HideObjAfterUseItem;
    [SerializeField] Interactive CantUseBeforeUse;
    [SerializeField] Interactive CantUseBeforeUnUse;
    [SerializeField] bool PlayAfterItemUse = false;
    [SerializeField] string MessageOnWrongUse = "";
    [SerializeField] public string InfoMessage = "";
    [SerializeField] bool MakeNoise = false;
    [SerializeField] bool MakeNoiseOnWrong = false;
    [SerializeField] GameObject Letter;
    [SerializeField] GameObject ActiveObj;
    [SerializeField] GameObject DisactiveObj;
    [SerializeField] GameObject RequiereHideObj;
    [SerializeField] Interactive[] RequiereUsed;
    [SerializeField] Interactive[] RequiereUnUsed;
    [SerializeField] GameObject[] DestroyParts;
    [SerializeField] Vector3 VelocityParts;
    [SerializeField] Interactive SendUseAfterUse;
    [SerializeField] AudioClip PlayerReplic;
    [SerializeField] TriggerSpace TriggerForPlayerReplic;
    [SerializeField] bool CheckByManiac = false;
    [SerializeField] public AudioClip ManiacReplic;
    [SerializeField] public PlayableDirector ManiacTimeline;
    [SerializeField] bool CanUseWhileChasing = true;
    [SerializeField] Material ChangeToMaterial;
    [SerializeField] GameObject TargetForMaterial;
    [SerializeField] int CountToReleaseTrap = 1;
    [SerializeField] float SpeedOfCloseTrap = 0.1f;
    [SerializeField] AudioClip SoundForTryReleaseTrap;
    [SerializeField] AudioClip SoundForSayPlayerTrap;
    [SerializeField] float SpeedAnimToReleaseTrap = -1;
    [SerializeField] UnityEvent NewEvent;
    [SerializeField] bool CanUseWhileManiacCheck = true;
    [SerializeField] bool Interraptable = true;
    [SerializeField] bool ChangeCheckInOtherRoom = false;
    [SerializeField] public AudioClip ManiacReplicInOtherRoom;
    [SerializeField] public PlayableDirector ManiacTimelineInOtherRoom;
    [SerializeField] bool OneTimeCheck = false;

    public bool Used { get; private set;} = false;
    public bool WasUsedOneTime { get; private set; } = false;
    bool IsPressed = false;
    bool IsReseting = false;
    bool IsUsedFixed = false;
    float Waiting = 0f;
    float WaitingForLock = 0f;
    float LockForUse = 0f;
    bool IsInitialized = false;
    bool WasChecked = false;
    int CounterForTrap = 0;
    float TimeToReleaseTrap;

    AnimationClip DefaultClip;

    private void Start ()
    {
        if (IsInitialized) return;

        if (MethodUse == MethodUseEnum.PlayAnimAndActiveEmission) ActiveObj.GetComponent<MeshRenderer> ().sharedMaterial.SetFloat ("_On", 0);
        if (Anim == null) Anim = GetComponent<Animation> ();
        if (Anim != null) {
            DefaultClip = Anim.clip;
        }
        if (AudioSrc == null) AudioSrc = GetComponent<AudioSource> ();
        if (MethodUse == MethodUseEnum.DropItem) {
            if (ActiveObj.active) {
                Used = true;
                GetComponent<BoxCollider> ().enabled = false;
            }
        }

        if (IsUsedFromStart) Used = true;

        IsInitialized = true;
    }

    private void OnMouseDown ()
    {
        Waiting = 0f;
        IsPressed = true;

        if (!FP_Input.instance.UseMobileInput) return;
        if (EventSystem.current.IsPointerOverGameObject () && !FP_Lookpad.instance.IsPressed ()) return;
    }

    private void OnMouseUpAsButton ()
    {
        IsPressed = false;
        
        if (!FP_Input.instance.UseMobileInput) return;
        /*foreach (Touch Temp in Input.touches) {
            if (Temp.phase == TouchPhase.Ended) {
                if (EventSystem.current.IsPointerOverGameObject (Temp.fingerId) && !FP_Lookpad.instance.IsPressed ()) return;
                break;
            }
        }*/
        if (FP_Lookpad.instance.IsDragged ()) return;
        if (Waiting <= 0.3f) {
            if (!PlayerHead.instance.IsDisabledPlayer) {
                if (Vector3.Distance (transform.position, PlayerHead.instance.transform.position) <= 2f) {
                    Use ();
                }
            }
        }
    }

    private void Update ()
    {
        if (WaitingForLock > 0f) {
            WaitingForLock -= Time.deltaTime;
        }
        if (LockForUse > 0f) {
            LockForUse -= Time.deltaTime;
        }
        if (IsPressed) Waiting += Time.deltaTime;
        if (IsReseting) {
            if (!Anim.isPlaying) {
                IsReseting = false;
            }
        }
        if (MethodUse == MethodUseEnum.DropItem) {
            if (!ActiveObj.active) GetComponent<BoxCollider> ().enabled = true;
        }
        else if (MethodUse == MethodUseEnum.PlayAnim && WaitingForLock <= 0f) {
            if (Interraptable && Anim.isPlaying && Used && CantUseBeforeUse != null) {
                if (!CantUseBeforeUse.Used) {
                    ResetAnim ();
                }
            }
            if (Interraptable && Anim.isPlaying && Used && CantUseBeforeUnUse != null) {
                if (CantUseBeforeUnUse.Used) {
                    ResetAnim ();
                }
            }
        }
        else if (MethodUse == MethodUseEnum.Trap) {
            if (Used/* && Anim.isPlaying*/) {
                if (TimeToReleaseTrap <= 0f && Anim[Anim.clip.name].time <= 0) {
                    if (CloseAudio != null) {
                        AudioSrc.clip = CloseAudio;
                        AudioSrc.Play ();
                    }
                    else if (Audio != null) {
                        AudioSrc.clip = Audio;
                        AudioSrc.Play ();
                    }
                    FP_Controller.instance.trapped = false;
                    if (!SingleUse) Used = !Used;
                    ActiveObj.SetActive (false);
                    return;
                }
                if (Anim[Anim.clip.name].time <= TimeToReleaseTrap && TimeToReleaseTrap != Anim[Anim.clip.name].length) {
                    Anim[Anim.clip.name].speed = SpeedOfCloseTrap;
                }
                if (Anim[Anim.clip.name].speed == SpeedOfCloseTrap && Anim[Anim.clip.name].time != 0) { // Трап закрывается, смещаем TimeToReleaseTrap ко времени клипа
                    TimeToReleaseTrap = Anim[Anim.clip.name].time;
                }
            }
        }
    }

    private void OnEnable ()
    {
        if (MethodUse == MethodUseEnum.Item) {
            Used = false;
        }
    }

    public void Use (bool HardUse = false)
    {
        if (LockForUse > 0f) return;
        if (!HardUse) {
            if (!PlayerHead.instance.CanUseInteractive) return;
            if (!CanUseWhileChasing && ManiacLogic.instance.IsChasing) return;
            if (!CanUseWhileManiacCheck && !Noise.instance.CanNoise) return;

            if (CantUseBeforeUse != null) {
                if (!CantUseBeforeUse.IsUsed () || !CantUseBeforeUse.gameObject.active) {
                    Inventory.instance.ShowText (MessageOnWrongUse);
                    if (MakeNoiseOnWrong) Noise.instance.MakeNoise (true);
                    return;
                }
            }
            if (CantUseBeforeUnUse != null) {
                if (CantUseBeforeUnUse.IsUsed () || !CantUseBeforeUnUse.gameObject.active) {
                    Inventory.instance.ShowText (MessageOnWrongUse);
                    if (MakeNoiseOnWrong) Noise.instance.MakeNoise (true);
                    return;
                }
            }
            if (RequierePlace != null) { // Проверка на нахождение игрока в этой области
                if (!RequierePlace.GetComponent<BoxCollider> ().bounds.Contains (FP_Controller.instance.transform.position)) return;
            }
            if (RequiereItem >= 0) {
                if (MethodUse == MethodUseEnum.DropItem && ActiveObj.active) return;
                if (FailedAudioSay != null) FP_Controller.instance.Say (FailedAudioSay);
                if (FailedAudio != null) AudioSrc.clip = FailedAudio;
                if (Inventory.instance.UseRequiereItem (RequiereItem, MessageOnWrongUse, (FailedAudio != null) ? AudioSrc : null)) {
                    if (MethodUse == MethodUseEnum.DropItem) {
                        Inventory.instance.RemoveChoosedItem ();
                    }
                    if (SuccessAudio != null) {
                        AudioSrc.clip = SuccessAudio;
                        AudioSrc.Play ();
                    }
                    if (MethodUse != MethodUseEnum.DropItem) {
                        RequiereItem = -1;
                        if (ShowObjAfterUseItem != null) ShowObjAfterUseItem.SetActive (true);
                        if (HideObjAfterUseItem != null) HideObjAfterUseItem.SetActive (false);
                        if (!PlayAfterItemUse) return;
                    }
                }
                else {
                    if (MakeNoiseOnWrong) Noise.instance.MakeNoise (true);
                    return;
                }
            }
            if (RequiereUsed.Length > 0) {
                for (int i = 0; i < RequiereUsed.Length; i++) {
                    if (!RequiereUsed[i].IsUsed ()) {
                        Inventory.instance.ShowText (MessageOnWrongUse);
                        if (MakeNoiseOnWrong) Noise.instance.MakeNoise (true);
                        return;
                    }
                }
            }
            if (RequiereUnUsed.Length > 0) {
                for (int i = 0; i < RequiereUnUsed.Length; i++) {
                    if (RequiereUnUsed[i].IsUsed ()) {
                        Inventory.instance.ShowText (MessageOnWrongUse);
                        if (MakeNoiseOnWrong) Noise.instance.MakeNoise (true);
                        return;
                    }
                }
            }
            if (!CanUnUse && Used) {
                Inventory.instance.ShowText (MessageOnWrongUse);
                return;
            }
            if (MethodUse == MethodUseEnum.Trap && !Used) {
                return;
            }
        }
        else {
            //if (WaitingForLock > 0f) return;
            WaitingForLock = 5f;
        }

        switch (MethodUse) {
            case MethodUseEnum.PlayAnim:
                if (Anim.isPlaying) return;
                if (RequiereHideObj != null && RequiereHideObj.active) {
                    Inventory.instance.ShowText (MessageOnWrongUse);
                    return;
                }
                
                if (SingleUse) {
                    if (!Used) {
                        Anim.clip = DefaultClip;
                        Anim.Play ();
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        Used = true;
                    }
                    else return;
                }
                else {
                    if (!Used) {
                        Anim.clip = DefaultClip;
                        Anim[Anim.clip.name].speed = 1;
                        //Anim[Anim.clip.name].time = 0;
                        Anim.Play ();
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                    }
                    else {
                        Anim.clip = DefaultClip;
                        Anim[Anim.clip.name].speed = -1;
                        Anim[Anim.clip.name].time = Anim[Anim.clip.name].length;
                        Anim.Play ();
                        if (CloseAudio != null) {
                            AudioSrc.clip = CloseAudio;
                            AudioSrc.Play ();
                        }
                        else if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                    }
                    Used = !Used;
                }
                break;
            case MethodUseEnum.PlaySound:
                if (RequiereHideObj != null && RequiereHideObj.active) {
                    Inventory.instance.ShowText (MessageOnWrongUse);
                    return;
                }

                if (SingleUse) {
                    if (!Used) {
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        Used = true;
                    }
                    else return;
                }
                else {
                    if (!Used) {
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                    }
                    else {
                        if (CloseAudio != null) {
                            AudioSrc.clip = CloseAudio;
                            AudioSrc.Play ();
                        }
                        else if (Audio != null) {
                            AudioSrc.Stop ();
                        }
                    }
                    Used = !Used;
                }
                break;
            case MethodUseEnum.Trap:
                //if (Anim.isPlaying) return;
                if (RequiereHideObj != null && RequiereHideObj.active) {
                    Inventory.instance.ShowText (MessageOnWrongUse);
                    return;
                }

                if (false/*SingleUse*/) {
                    if (!Used) {
                        Anim.clip = DefaultClip;
                        Anim.Play ();
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        Used = true;
                    }
                    else return;
                }
                else {
                    if (SingleUse && Used) return;
                    if (!Used) {
                        Anim.clip = DefaultClip;
                        Anim[Anim.clip.name].speed = 1;
                        TimeToReleaseTrap = Anim[Anim.clip.name].length;
                        //Anim[Anim.clip.name].time = 0;
                        Anim.Play ();
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        Noise.instance.MakeNoise (false, false);
                        FP_Controller.instance.Say (SoundForSayPlayerTrap);
                        ActiveObj.SetActive (true);
                    }
                    else {
                        /*if (TimeToReleaseTrap == Anim[Anim.clip.name].length) {
                            Anim.clip = DefaultClip;
                            Anim[Anim.clip.name].time = Anim[Anim.clip.name].length;
                            Anim[Anim.clip.name].speed = SpeedAnimToReleaseTrap;
                            Anim.Play ();
                        }*/
                        if (Anim[Anim.clip.name].time == Anim[Anim.clip.name].length || !Anim.isPlaying) {
                            Anim.clip = DefaultClip;
                            Anim[Anim.clip.name].time = Anim[Anim.clip.name].length;
                            Anim[Anim.clip.name].speed = SpeedAnimToReleaseTrap;
                            Anim.Play ();
                        }
                        Anim[Anim.clip.name].speed = SpeedAnimToReleaseTrap;
                        TimeToReleaseTrap -= (Anim[Anim.clip.name].length / CountToReleaseTrap);
                        if (!AudioSrc.isPlaying) {
                            if (SoundForTryReleaseTrap != null) {
                                AudioSrc.clip = SoundForTryReleaseTrap;
                                AudioSrc.Play ();
                            }
                        }
                        /*if (CounterForTrap >= CountToReleaseTrap) {
                            CounterForTrap = 0;
                            if (CloseAudio != null) {
                                AudioSrc.clip = CloseAudio;
                                AudioSrc.Play ();
                            }/*
                            else if (Audio != null) {
                                AudioSrc.clip = Audio;
                                AudioSrc.Play ();
                            }
                            FP_Controller.instance.trapped = false;
                        }
                        else */
                    return;
                    }
                    Used = !Used;
                }
                break;
            case MethodUseEnum.PlayAnimAndActiveObj:
                if (Anim.isPlaying) return;

                if (SingleUse) {
                    if (!Used) {
                        Anim.clip = DefaultClip;
                        Anim.Play ();
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        Used = true;
                        ActiveObj.SetActive (true);
                    }
                    else return;
                }
                else {
                    if (!Used) {
                        Anim.clip = DefaultClip;
                        Anim[Anim.clip.name].speed = 1;
                        //Anim[Anim.clip.name].time = 0;
                        Anim.Play ();
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                    }
                    else {
                        Anim.clip = DefaultClip;
                        Anim[Anim.clip.name].speed = -1;
                        Anim[Anim.clip.name].time = Anim[Anim.clip.name].length;
                        Anim.Play ();
                        if (CloseAudio != null) {
                            AudioSrc.clip = CloseAudio;
                            AudioSrc.Play ();
                        }
                        else if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                    }
                    Used = !Used;
                    ActiveObj.SetActive (Used);
                }
                break;
            case MethodUseEnum.PlayAnimAndDisactiveObj:
                if (Anim.isPlaying) return;

                if (SingleUse) {
                    if (!Used) {
                        Anim.clip = DefaultClip;
                        Anim.Play ();
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        Used = true;
                        ActiveObj.SetActive (false);
                    }
                    else return;
                }
                else {
                    if (!Used) {
                        Anim.clip = DefaultClip;
                        Anim[Anim.clip.name].speed = 1;
                        //Anim[Anim.clip.name].time = 0;
                        Anim.Play ();
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                    }
                    else {
                        Anim.clip = DefaultClip;
                        Anim[Anim.clip.name].speed = -1;
                        Anim[Anim.clip.name].time = Anim[Anim.clip.name].length;
                        Anim.Play ();
                        if (CloseAudio != null) {
                            AudioSrc.clip = CloseAudio;
                            AudioSrc.Play ();
                        }
                        else if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                    }
                    Used = !Used;
                    ActiveObj.SetActive (!Used);
                }
                break;
            case MethodUseEnum.Item:
                if (CantUseBeforeUse != null) {
                    if (!CantUseBeforeUse.Used) {
                        return;
                    }
                }
                if (ActiveObj != null && ActiveObj.active) {
                    Inventory.instance.ShowText (MessageOnWrongUse);
                    return;
                }
                if (!GetComponent<Item> ().TakeItem ()) return;
                Used = true;
                break;
            case MethodUseEnum.ShowLetter:
                if (RequierePlace != null) { // Проверка на нахождение игрока в этой области
                    if (!RequierePlace.GetComponent<BoxCollider> ().bounds.Contains (FP_Controller.instance.transform.position)) return;
                }

                if (SingleUse) {
                    if (!Used) {
                        PlayerManager.instance.ShowLetter (Letter);
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        Used = true;
                    }
                    else return;
                }
                else {
                    if (!Used) {
                        PlayerManager.instance.ShowLetter (Letter);
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                    }
                    else {
                        PlayerManager.instance.ShowLetter (Letter);
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                    }
                    Used = !Used;
                }
                break;
            case MethodUseEnum.ShowMesh:
                if (RequierePlace != null) { // Проверка на нахождение игрока в этой области
                    if (!RequierePlace.GetComponent<BoxCollider> ().bounds.Contains (FP_Controller.instance.transform.position)) return;
                }

                if (SingleUse) {
                    if (!Used) {
                        GetComponent<MeshRenderer> ().enabled = true;
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        Used = true;
                    }
                    else return;
                }
                else {
                    if (!Used) {
                        GetComponent<MeshRenderer> ().enabled = true;
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                    }
                    else {
                        GetComponent<MeshRenderer> ().enabled = false;
                        if (CloseAudio != null) {
                            AudioSrc.clip = CloseAudio;
                            AudioSrc.Play ();
                        }
                        else if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                    }
                    Used = !Used;
                }
                break;
            case MethodUseEnum.ShowMessage:
                Inventory.instance.ShowText (InfoMessage);
                break;
            case MethodUseEnum.DropItem:
                ActiveObj.SetActive (true);
                GetComponent<BoxCollider> ().enabled = false;
                break;
            case MethodUseEnum.DisactiveObj:
                ActiveObj.SetActive (false);
                Used = true;
                break;
            case MethodUseEnum.ActiveObj:
                ActiveObj.SetActive (true);
                if (ActiveObj.GetComponent<BoxCollider> () != null) ActiveObj.GetComponent<BoxCollider> ().enabled = true;
                Used = true;
                break;
            case MethodUseEnum.SwitchObj:
                ActiveObj.SetActive (!Used);
                if (ActiveObj.GetComponent<BoxCollider> () != null) ActiveObj.GetComponent<BoxCollider> ().enabled = !Used;
                Used = !Used;
                break;
            case MethodUseEnum.ShowMessageAndSwitchObj:
                gameObject.SetActive (false);
                ActiveObj.SetActive (true);
                Inventory.instance.ShowText (InfoMessage);
                Used = true;
                break;
            case MethodUseEnum.PlayAnimAndActivePhisics:
                if (Anim.isPlaying) return;

                if (SingleUse) {
                    if (!Used) {
                        Anim.clip = DefaultClip;
                        Anim.Play ();
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        ActiveObj.GetComponent<Rigidbody> ().isKinematic = false;
                        Used = true;
                    }
                    else return;
                }
                else {
                    if (!Used) {
                        Anim.clip = DefaultClip;
                        Anim[Anim.clip.name].speed = 1;
                        //Anim[Anim.clip.name].time = 0;
                        Anim.Play ();
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                    }
                    else {
                        Anim.clip = DefaultClip;
                        Anim[Anim.clip.name].speed = -1;
                        Anim[Anim.clip.name].time = Anim[Anim.clip.name].length;
                        Anim.Play ();
                        if (CloseAudio != null) {
                            AudioSrc.clip = CloseAudio;
                            AudioSrc.Play ();
                        }
                        else if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                    }
                    Used = !Used;
                    ActiveObj.GetComponent<Rigidbody> ().isKinematic = false;
                }
                break;
            case MethodUseEnum.ActivePhysics:
                if (SingleUse) {
                    if (!Used) {
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        ActiveObj.GetComponent<Rigidbody> ().isKinematic = false;
                        Used = true;
                    }
                    else return;
                }
                else {
                    if (!Used) {
                        ActiveObj.GetComponent<Rigidbody> ().isKinematic = false;
                    }
                    else {
                        if (CloseAudio != null) {
                            AudioSrc.clip = CloseAudio;
                            AudioSrc.Play ();
                        }
                        else if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        ActiveObj.GetComponent<Rigidbody> ().isKinematic = true;
                    }
                    Used = !Used;
                }
                break;
            case MethodUseEnum.PlayAnimAndActiveEmission:
                if (Anim.isPlaying) return;

                if (SingleUse) {
                    if (!Used) {
                        Anim.clip = DefaultClip;
                        Anim.Play ();
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        ActiveObj.GetComponent<MeshRenderer> ().sharedMaterial.SetFloat ("_On", 1);
                        Used = true;
                    }
                    else return;
                }
                else {
                    if (!Used) {
                        Anim.clip = DefaultClip;
                        Anim[Anim.clip.name].speed = 1;
                        //Anim[Anim.clip.name].time = 0;
                        Anim.Play ();
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        ActiveObj.GetComponent<MeshRenderer> ().sharedMaterial.SetFloat ("_On", 1);
                    }
                    else {
                        Anim.clip = DefaultClip;
                        Anim[Anim.clip.name].speed = -1;
                        Anim[Anim.clip.name].time = Anim[Anim.clip.name].length;
                        Anim.Play ();
                        if (CloseAudio != null) {
                            AudioSrc.clip = CloseAudio;
                            AudioSrc.Play ();
                        }
                        else if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        ActiveObj.GetComponent<MeshRenderer> ().sharedMaterial.SetFloat ("_On", 0);
                    }
                    Used = !Used;
                }
                break;
            case MethodUseEnum.Destruction:
                Used = true;
                foreach (GameObject Temp in DestroyParts) {
                    Rigidbody Rb = Temp.GetComponent<Rigidbody> ();
                    Rb.isKinematic = false;
                    Rb.velocity = VelocityParts;
                }
                break;
            case MethodUseEnum.ActiveObjAndChangeMaterial:
                ActiveObj.SetActive (true);
                if (ActiveObj.GetComponent<BoxCollider> () != null) ActiveObj.GetComponent<BoxCollider> ().enabled = true;
                TargetForMaterial.GetComponent<MeshRenderer> ().material = ChangeToMaterial;
                Used = true;
                break;
            case MethodUseEnum.PlayAnimAndDisactiveParticle:
                if (Anim.isPlaying) return;
                if (RequiereHideObj != null && RequiereHideObj.active) {
                    Inventory.instance.ShowText (MessageOnWrongUse);
                    return;
                }

                if (SingleUse) {
                    if (!Used) {
                        Anim.clip = DefaultClip;
                        Anim.Play ();
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        ActiveObj.GetComponent<ParticleSystem> ().enableEmission = false;
                        if (ActiveObj.GetComponent<BoxCollider> () != null) ActiveObj.GetComponent<BoxCollider> ().enabled = false;
                        Used = true;
                    }
                    else return;
                }
                else {
                    if (!Used) {
                        Anim.clip = DefaultClip;
                        Anim[Anim.clip.name].speed = 1;
                        //Anim[Anim.clip.name].time = 0;
                        Anim.Play ();
                        if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        ActiveObj.GetComponent<ParticleSystem> ().enableEmission = false;
                        if (ActiveObj.GetComponent<BoxCollider> () != null) ActiveObj.GetComponent<BoxCollider> ().enabled = false;
                    }
                    else {
                        Anim.clip = DefaultClip;
                        Anim[Anim.clip.name].speed = -1;
                        Anim[Anim.clip.name].time = Anim[Anim.clip.name].length;
                        Anim.Play ();
                        if (CloseAudio != null) {
                            AudioSrc.clip = CloseAudio;
                            AudioSrc.Play ();
                        }
                        else if (Audio != null) {
                            AudioSrc.clip = Audio;
                            AudioSrc.Play ();
                        }
                        ActiveObj.GetComponent<ParticleSystem> ().enableEmission = true;
                        if (ActiveObj.GetComponent<BoxCollider> () != null) ActiveObj.GetComponent<BoxCollider> ().enabled = true;
                    }
                    Used = !Used;
                }
                break;
            case MethodUseEnum.UnityEvent:
                if (SingleUse) {
                    if (!Used) NewEvent.Invoke ();
                    else return;
                }
                else NewEvent.Invoke ();
                Used = !Used;
                break;

        }

        WasUsedOneTime = true;
        if (SendUseAfterUse != null) {
            SendUseAfterUse.PowerUse ();
        }
        Helper.instance.CheckUsedHelp (gameObject);
        if (MakeNoise) Noise.instance.MakeNoise ();
        if (Used && HideColliderAfterUse) {
            GetComponent<BoxCollider> ().enabled = false;
        }
        if (DisattachParentAfterUse) transform.parent = null;
    }

    public void PowerUse ()
    {
        int Temp = RequiereItem;
        RequiereItem = -1;
        Use (true);
        RequiereItem = Temp;
    }

    public void PowerUseIfNotUsed ()
    {
        if (!Used) PowerUse ();
    }

    public void PowerUseIfUsed ()
    {
        if (Used) PowerUse ();
    }

    public void PowerUseWithoutSound (float speed = 1f)
    {
        if (LockForUse > 0f) return;
        int Temp = RequiereItem;
        RequiereItem = -1;
        Use (true);
        if (Anim != null) Anim[Anim.clip.name].speed = speed;
        RequiereItem = Temp;
        if (AudioSrc != null) AudioSrc.Stop ();
    }

    public void ChangeRequiereItem (int NewID)
    {
        RequiereItem = NewID;
    }

    public bool IsUsed ()
    {
        bool Result = false;

        if (Used) { 
            Result = true;
            if (MethodUse == MethodUseEnum.PlayAnim) {
                Result = !Anim.isPlaying;
            }
        }

        return Result;
    }

    public void ResetAnim ()
    {
        if (!IsInitialized) Start ();
        if (LockForUse > 0f) return;

        if (Anim != null) {
            if (ResetAnimClip != null) {
                IsReseting = true;
                WaitingForLock = 2f;
                Anim.clip = ResetAnimClip;
                Anim.Play ();
            }
            else {
                Anim.Stop ();
            }
            if (AudioSrc != null) {
                AudioSrc.Stop ();
            }
            if (MethodUse == MethodUseEnum.PlayAnimAndActiveEmission) ActiveObj.GetComponent<MeshRenderer> ().sharedMaterial.SetFloat ("_On", 0);
            else if (MethodUse == MethodUseEnum.PlayAnimAndActiveObj) ActiveObj.SetActive (false);
            if (!IsUsedFixed) Used = false;
        }
        else if (MethodUse == MethodUseEnum.SwitchObj) {
            if (IsUsed ()) Use (true);
        }
        else return;
    }

    public void FixUsed ()
    {
        IsUsedFixed = true;
    }

    public bool Check ()
    {
        bool Result = false;

        switch (MethodUse) {
            case MethodUseEnum.Item:
                Result = !gameObject.activeSelf;
                break;

            default:
                if (ManiacLogic.instance.InOtherRoom && ChangeCheckInOtherRoom) Result = !Used;
                else Result = Used;
                break;
        }

        if (Result && OneTimeCheck) {
            if (WasChecked) return false;
            else WasChecked = true;
        }

        return Result;
    }

    public void TriggerSay ()
    {
        if (PlayerReplic != null && Used) {
            if (TriggerForPlayerReplic != null && !TriggerForPlayerReplic.Used) return;
            FP_Controller.instance.SayWithDelay (PlayerReplic);
        }
    }

    public void GamePaused ()
    {
        if (AudioSrc != null) AudioSrc.Pause ();
    }

    public void GameUnpaused ()
    {
        if (AudioSrc != null) AudioSrc.UnPause ();
    }

    public void Hide ()
    {
        gameObject.SetActive (false);
    }

    public bool IsFree ()
    {
        return (RequiereItem == -1 || IsUsed ());
    }

    public void MakeNoiseFunc ()
    {
        if (ManiacLogic.instance.PlayTimeline) StartCoroutine (ManiacLogic.instance.Chasing (0));
        else Noise.instance.MakeNoise ();
    }

    public AudioClip GetManiacReplic ()
    {
        if (ManiacLogic.instance.InOtherRoom && ChangeCheckInOtherRoom) return ManiacReplicInOtherRoom;
        else return ManiacReplic;
    }

    public void SetLock (float NewWaitingForLock)
    {
        LockForUse = NewWaitingForLock;
    }
}
