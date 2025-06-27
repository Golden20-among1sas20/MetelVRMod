using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.AI;

public class ManiacLogic : MonoBehaviour
{
    [System.Serializable]
    class Masks
    {
        public string Name;
        public GameObject Target;
    }

    [System.Serializable]
    class Skins
    {
        public string Name;
        public GameObject Target;
    }

    [System.Serializable]
    class Successes
    {
        public Interactive Requiere;
        public PlayableDirector Tmln;
    }

    [System.Serializable]
    class ForCompressor
    {
        public PlayableDirector TmlnForEqual;
        public PlayableDirector TargetTmln;
    }

    [SerializeField] Masks[] AllMasks;
    [SerializeField] Skins[] AllSkins;
    [SerializeField] PlayableDirector TimelineSuccess;
    [SerializeField] Successes[] TimelineSuccesses;
    [SerializeField] Successes[] TimelineSuccessesFromMax;
    [SerializeField] PlayableDirector TimelineCompressor;
    [SerializeField] PlayableDirector TimelineCompressorFromMax;
    [SerializeField] ForCompressor[] TimelineCompressorSuccesses;
    [SerializeField] ForCompressor[] TimelineCompressorFailedes;
    [SerializeField] ForCompressor[] TimelineFailedesFromMax;
    [SerializeField] PlayableDirector TimelineFailed;
    [SerializeField] PlayableDirector TimelineEasyKill;
    [SerializeField] PlayableDirector TimelineDeliverySuccess;
    [SerializeField] PlayableDirector TimelineDeliveryFailed;
    [SerializeField] PlayableDirector TimelineSearching;
    [SerializeField] AudioSource Mouth;
    [SerializeField] AudioSource AudioSrc;
    [SerializeField] AudioClip LeftStep;
    [SerializeField] AudioClip RightStep;
    [SerializeField] Interactive Door;
    [SerializeField] Interactive CompressorDoor;
    [SerializeField] Interactive ToggleLight;
    [SerializeField] Interactive CompressorDoorLock;
    [SerializeField] Interactive[] ForCheck;
    [SerializeField] AudioClip ChaseClip;
    [SerializeField] GameObject CamPlaceForKilling;
    [SerializeField] Camera OldCam;
    [SerializeField] AudioClip ChaseMusic;
    [SerializeField] GameObject ClosedDoor;
    [SerializeField] AudioClip AngryClosedDoor;
    [SerializeField] AudioClip RageClip;
    [SerializeField] AudioClip WhereAreYouClip;
    [SerializeField] AudioClip GotchaClip;
    [SerializeField] AudioSource DamageDoorSrc;
    [SerializeField] AudioSource DestroyedDoorSrc;
    [SerializeField] Interactive[] ListForOpen;
    [SerializeField] Interactive CheckForDelivery;
    [SerializeField] Interactive CheckForMaxNoise;
    [SerializeField] GameObject[] DeleteAfterDelivery;
    [SerializeField] GameObject[] CheckForExitStep;
    [SerializeField] TriggerSpace[] CantChaseIfUsed;
    [SerializeField] GameObject[] CloseBeforeKill;
    [SerializeField] Interactive[] PlayerDoor;
    [SerializeField] Interactive Compressor;
    [SerializeField] AudioClip EmilyGottaOut;
    [SerializeField] AudioClip EmilyYouNeedToLock;
    [SerializeField] Interactive YouNeedToLockPassword;
    [SerializeField] AudioClip EmilyComeOn;
    [SerializeField] AudioClip EmilyCloseAdvice;
    [SerializeField] Interactive[] ForBreakDoor;
    [SerializeField] GameObject PlaceInMaxRoom;
    [SerializeField] PlayableDirector TimelineMaxReturnNoise;
    [SerializeField] PlayableDirector TimelineMaxNoise;
    //[SerializeField] PlayableDirector TimelineMaxReturnNoiseAngry;
    [SerializeField] GameObject LeverForMax;
    [SerializeField] GameObject DropIfEnable;
    [SerializeField] PlayableDirector TimelineMaxFailedBeforeCompressor;
    [SerializeField] PlayableDirector TimelineFailedBeforeCompressor;
    [SerializeField] PlayableDirector TimelineMaxReturnNoiseAngry;
    [SerializeField] Interactive[] LockAfterGasBeforeMax;
    [SerializeField] GameObject DisableAfterDelivered;
    [SerializeField] GameObject CameraForBeating;
    [SerializeField] TriggerSpace HidePlaceForEugene;

    PlayableDirector ChoosedTimeline;
    Animator Anim;
    public bool PlayTimeline { get; private set;} = false;
    public bool Rage { get; private set;} = false;
    bool Delivered = false;
    bool MaxNoised = false;
    bool YouNeedToLockSaid = false;
    bool GotchaSaid = false;
    bool CloseAdvice = false;
    bool SmoothPunch = false;
    public bool InOtherRoom { get; private set;} = false;
    bool MaxHasLever = false;
    public bool CanChase = true;
    public bool IsChasing { get; private set;} = false;
    AudioClip FailedCauseClip;
    Vector3 StartPos;
    float Waiting = 0f;
    float OldSpeed = 3.5f;
    float WaitingForCheckCanChase = 0.1f;
    string LevelName;

    const float TimeToDestroyDoor = 23f;
    const float TimeToDestroyDoorLevelB = 28f;

    static ManiacLogic _instance;
    public static ManiacLogic instance { get { return _instance; } }

    private void Awake ()
    {
        if (_instance == null) _instance = this;
        else Debug.LogError ("More than one ManiacLogic");

        LevelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;
    }

    private void Start ()
    {
        string ChoosedMask = PlayerPrefs.GetString ("MaskName", "GasMask");
        for (int i = 0; i < AllMasks.Length; i++) {
            AllMasks[i].Target.SetActive (AllMasks[i].Name == ChoosedMask);
        }
        string ChoosedSkin = PlayerPrefs.GetString ("SkinName", "ClassicSkin");
        for (int i = 0; i < AllSkins.Length; i++) {
            AllSkins[i].Target.SetActive (AllSkins[i].Name == ChoosedSkin);
        }
        Anim = GetComponent<Animator> ();
        StartPos = transform.position;
        OldSpeed = GetComponent<NavMeshAgent> ().speed;
        //StartEnter ();
    }

    private void Update ()
    {
        switch (LevelName) {
            case "Level_A":
                if (PlayTimeline) {
                    if (ChoosedTimeline.state == PlayState.Paused) {
                        if (ChoosedTimeline == TimelineDeliveryFailed || ChoosedTimeline == TimelineDeliverySuccess) {
                            foreach (GameObject Temp in DeleteAfterDelivery) Destroy (Temp);
                        }
                        PlayTimeline = false;
                        Noise.instance.CanNoise = true;
                        if (!PlayerManager.instance.IsDead) {
                            //PlayerHead.instance.DisablePlayer (false);
                            MusicManager.instance.PlayDefault ();
                            RefreshManiac ();
                            PlayerHead.instance.SetCanUseInteractive (true);
                            if (PlayerManager.instance.IsShocked ()) Door.PowerUseWithoutSound (-1f);
                            else Door.PowerUse ();
                        }
                    }
                }
                else if (IsChasing) {
                    GetComponent<NavMeshAgent> ().destination = PlayerHead.instance.transform.position;
                    if (!GotchaSaid) {
                        if (Vector3.Distance (transform.position, PlayerHead.instance.transform.position) < 3) {
                            GotchaSaid = true;
                            Say (GotchaClip);
                        }
                    }
                    if (Vector3.Distance (transform.position, PlayerHead.instance.transform.position) < 2) {
                        RaycastHit hit;
                        var layerMask = (1 << 4);
                        layerMask |= (1 << 2);
                        layerMask |= (1 << 5);
                        if (Physics.Raycast (transform.position, PlayerHead.instance.transform.position - transform.position, out hit, 2f, ~layerMask, QueryTriggerInteraction.Ignore)) {
                            if (hit.transform != null) {
                                Debug.Log (hit.transform.gameObject.name);
                            }
                        }
                        else {
                            /*if (!GetComponent<Animator> ().GetBool ("IsCrawling"))*/
                            Kill ();
                        }
                    }
                    WaitingForCheckCanChase -= Time.deltaTime;
                    if (WaitingForCheckCanChase <= 0f) {
                        WaitingForCheckCanChase = 0.1f;
                        bool CanChase = true;
                        foreach (TriggerSpace Temp in CantChaseIfUsed) if (Temp.Used) CanChase = false;
                        if (!CanChase) {
                            IsChasing = false;
                            GetComponent<Animator> ().SetBool ("IsChasing", false);
                            GetComponent<Animator> ().Play ("Maniac_Idle");
                            GetComponent<NavMeshAgent> ().enabled = false;
                        }
                    }
                }
                break;
            case "Level_D":
                if (PlayTimeline) {
                    if (IsPlayerDoorOpened ()) { // дверь открыта во время воспроизведения таймлайна, начинаем погоню
                        StartCoroutine (Chasing ());
                        PlayTimeline = false;
                        Vector3 TempPos = transform.position;
                        ChoosedTimeline.Stop ();
                        transform.position = TempPos;
                    }
                    else if (ChoosedTimeline.state == PlayState.Paused) {
                        if (ChoosedTimeline == TimelineDeliveryFailed || ChoosedTimeline == TimelineDeliverySuccess) {
                            foreach (GameObject Temp in DeleteAfterDelivery) Destroy (Temp);
                        }
                        PlayTimeline = false;
                        Noise.instance.CanNoise = true;
                        if (!PlayerManager.instance.IsDead) {
                            //PlayerHead.instance.DisablePlayer (false);
                            MusicManager.instance.PlayDefault ();
                            RefreshManiac ();
                            PlayerHead.instance.SetCanUseInteractive (true);
                            if (PlayerManager.instance.IsShocked ()) Door.PowerUseWithoutSound (-1f);
                            else Door.PowerUse ();
                        }
                    }
                }
                else if (IsChasing) {
                    GetComponent<NavMeshAgent> ().destination = PlayerHead.instance.transform.position;
                    if (!GotchaSaid) {
                        if (Vector3.Distance (transform.position, PlayerHead.instance.transform.position) < 3) {
                            GotchaSaid = true;
                            Say (GotchaClip);
                        }
                    }
                    if (Vector3.Distance (transform.position, PlayerHead.instance.transform.position) < 2) {
                        RaycastHit hit;
                        var layerMask = (1 << 4);
                        layerMask |= (1 << 2);
                        layerMask |= (1 << 5);
                        if (Physics.Raycast (transform.position, PlayerHead.instance.transform.position - transform.position, out hit, 2f, ~layerMask, QueryTriggerInteraction.Ignore)) {
                            if (hit.transform != null) {
                                Debug.Log (hit.transform.gameObject.name);
                            }
                        }
                        else {
                            /*if (!GetComponent<Animator> ().GetBool ("IsCrawling"))*/
                            Kill ();
                        }
                    }
                    WaitingForCheckCanChase -= Time.deltaTime;
                    if (WaitingForCheckCanChase <= 0f) {
                        WaitingForCheckCanChase = 0.1f;
                        bool CanChase = true;
                        foreach (TriggerSpace Temp in CantChaseIfUsed) if (Temp.Used) CanChase = false;
                        if (!CanChase) {
                            IsChasing = false;
                            GetComponent<Animator> ().SetBool ("IsChasing", false);
                            GetComponent<Animator> ().Play ("Maniac_Idle");
                            GetComponent<NavMeshAgent> ().enabled = false;
                        }
                    }
                }
                break;
            case "Level_B":
                if (PlayTimeline) {
                    if ((IsPlayerDoorOpened () || !Noise.instance.CheckSafe ()) && CanChase) { // дверь открыта во время воспроизведения таймлайна, начинаем погоню
                        StartCoroutine (Chasing (0));
                        PlayTimeline = false;
                        Vector3 TempPos = transform.position;
                        ChoosedTimeline.Stop ();
                        transform.position = TempPos;
                    }
                    else if (ChoosedTimeline.state == PlayState.Paused) {
                        if (ChoosedTimeline == TimelineMaxNoise && !MaxHasLever) {
                            MaxLogic.instance.KillMax ();
                            if (!PlayerManager.instance.IsDead) {
                                PlayTimeline = false;
                                Noise.instance.CanNoise = true;
                                Noise.instance.MakeNoise (false, false, false);
                                return;
                            }
                        }
                        else if (ChoosedTimeline == TimelineMaxNoise && MaxHasLever) {
                            if (!PlayerManager.instance.IsDead) {
                                ChoosedTimeline.Stop ();
                                ChoosedTimeline = TimelineMaxReturnNoiseAngry;
                                ChoosedTimeline.Play ();
                                /*StartCoroutine (Chasing (0, OhYouClip));
                                PlayTimeline = false;
                                ChoosedTimeline.Stop ();
                                transform.position = ChasePosAfterMax;*/
                                return;
                            }
                        }
                        else if (ChoosedTimeline == TimelineMaxReturnNoiseAngry) {
                            if (!PlayerManager.instance.IsDead) {
                                StartCoroutine (Chasing (0));
                                PlayTimeline = false;
                                Vector3 TempPos = transform.position;
                                ChoosedTimeline.Stop ();
                                transform.position = TempPos;
                                return;
                            }
                        }
                        if (Delivered) DisableAfterDelivered.SetActive (false);
                        if (ChoosedTimeline == TimelineDeliveryFailed || ChoosedTimeline == TimelineDeliverySuccess) {
                            foreach (GameObject Temp in DeleteAfterDelivery) Destroy (Temp);
                        }
                        foreach (Successes Temp in TimelineSuccessesFromMax) {
                            if (Temp.Tmln == ChoosedTimeline) InOtherRoom = false;
                        }
                        foreach (ForCompressor Temp in TimelineFailedesFromMax) {
                            if (Temp.TargetTmln == ChoosedTimeline) InOtherRoom = false;
                        }
                        PlayTimeline = false;
                        Noise.instance.CanNoise = true;
                        if (!PlayerManager.instance.IsDead) {
                            //PlayerHead.instance.DisablePlayer (false);
                            MusicManager.instance.PlayDefault ();
                            if (InOtherRoom) {
                                transform.position = PlaceInMaxRoom.transform.position;
                            }
                            else {
                                RefreshManiac ();
                                for (int i = 0; i < Noise.instance.SwitchOffAfterNoise.Length; i++) Noise.instance.SwitchOffAfterNoise[i].SetActive (true);
                            }
                            PlayerHead.instance.SetCanUseInteractive (true);
                            if (PlayerManager.instance.IsShocked () && !InOtherRoom && Door.Used) Door.PowerUseWithoutSound (-1f);
                            //else Door.PowerUse ();
                        }
                    }
                }
                else if (IsChasing) {
                    GetComponent<NavMeshAgent> ().destination = PlayerHead.instance.transform.position;
                    if (!GotchaSaid) {
                        if (Vector3.Distance (transform.position, PlayerHead.instance.transform.position) < 3) {
                            GotchaSaid = true;
                            Say (GotchaClip);
                        }
                    }
                    if (Vector3.Distance (transform.position, PlayerHead.instance.transform.position) < 2) {
                        RaycastHit hit;
                        var layerMask = (1 << 4);
                        layerMask |= (1 << 2);
                        layerMask |= (1 << 5);
                        if (Physics.Raycast (transform.position, PlayerHead.instance.transform.position - transform.position, out hit, 2f, ~layerMask, QueryTriggerInteraction.Ignore)) {
                            if (hit.transform != null) {
                                Debug.Log (hit.transform.gameObject.name);
                            }
                        }
                        else {
                            /*if (!GetComponent<Animator> ().GetBool ("IsCrawling"))*/
                            Kill ();
                        }
                    }
                    WaitingForCheckCanChase -= Time.deltaTime;
                    if (WaitingForCheckCanChase <= 0f) {
                        WaitingForCheckCanChase = 0.1f;
                        bool CanChase = true;
                        foreach (TriggerSpace Temp in CantChaseIfUsed) if (Temp.Used) CanChase = false;
                        if (!CanChase) {
                            IsChasing = false;
                            GetComponent<Animator> ().SetBool ("IsChasing", false);
                            GetComponent<Animator> ().Play ("Maniac_Idle");
                            GetComponent<NavMeshAgent> ().enabled = false;
                        }
                    }
                }
                break;

            case "Level_C":
                if (PlayTimeline) {
                    if ((IsPlayerDoorOpened () || !Noise.instance.CheckSafe ()) && CanChase && !HidePlaceForEugene.Used) { // дверь открыта во время воспроизведения таймлайна, начинаем погоню
                        StartCoroutine (Chasing (0));
                        PlayTimeline = false;
                        Vector3 TempPos = transform.position;
                        ChoosedTimeline.Stop ();
                        transform.position = TempPos;
                    }
                    else if (ChoosedTimeline.state == PlayState.Paused) {
                        PlayTimeline = false;
                        Noise.instance.CanNoise = true;
                        if (!PlayerManager.instance.IsDead) {
                            //PlayerHead.instance.DisablePlayer (false);
                            MusicManager.instance.PlayDefault ();
                            RefreshManiac ();
                            for (int i = 0; i < Noise.instance.SwitchOffAfterNoise.Length; i++) Noise.instance.SwitchOffAfterNoise[i].SetActive (true);
                            PlayerHead.instance.SetCanUseInteractive (true);
                            if (PlayerManager.instance.IsShocked () && Door.Used) Door.PowerUseWithoutSound (-1f);
                        }
                    }
                }
                else if (IsChasing) {
                    GetComponent<NavMeshAgent> ().destination = PlayerHead.instance.transform.position;
                    if (!GotchaSaid && !SmoothPunch) {
                        if (Vector3.Distance (transform.position, PlayerHead.instance.transform.position) < 3) {
                            GotchaSaid = true;
                            Say (GotchaClip);
                        }
                    }
                    if (Vector3.Distance (transform.position, PlayerHead.instance.transform.position) < 2) {
                        RaycastHit hit;
                        var layerMask = (1 << 4);
                        layerMask |= (1 << 2);
                        layerMask |= (1 << 5);
                        if (Physics.Raycast (transform.position, PlayerHead.instance.transform.position - transform.position, out hit, 2f, ~layerMask, QueryTriggerInteraction.Ignore)) {
                            if (hit.transform != null) {
                                Debug.Log (hit.transform.gameObject.name);
                            }
                        }
                        else {
                            if (SmoothPunch) Punching ();
                            else Kill ();
                        }
                    }
                    WaitingForCheckCanChase -= Time.deltaTime;
                    if (WaitingForCheckCanChase <= 0f) {
                        WaitingForCheckCanChase = 0.1f;
                        bool CanChase = true;
                        foreach (TriggerSpace Temp in CantChaseIfUsed) if (Temp.Used) CanChase = false;
                        if (!CanChase) {
                            IsChasing = false;
                            GetComponent<Animator> ().SetBool ("IsChasing", false);
                            GetComponent<Animator> ().Play ("Maniac_Idle");
                            GetComponent<NavMeshAgent> ().enabled = false;
                        }
                    }
                }
                break;
        }
    }

    bool IsPlayerDoorOpened ()
    {
        for (int i = 0; i < PlayerDoor.Length; i++) {
            if (PlayerDoor[i].IsUsed ()) return true;
        }

        return false;
    }

    public void Kill ()
    {
        if (PlayerManager.instance.IsDead) {
            GetComponent<Animator> ().Play ("Maniac_Idle");
        }
        else {
            IsChasing = false;
            Vector3 Temp = PlayerHead.instance.gameObject.transform.position;
            Temp.y = transform.position.y;
            transform.LookAt (Temp);
            PlayerHead.instance.DisablePlayer (true);
            OldCam.enabled = false;
            CamPlaceForKilling.SetActive (true);
            GetComponent<NavMeshAgent> ().enabled = false;
            GetComponent<Animator> ().Play ("Maniac_Kill");
            foreach (GameObject TempObj in CloseBeforeKill) {
                TempObj.SetActive (false);
            }
        }
    }

    public void KillInHidePlace ()
    {
        PlayTimeline = false;
        Vector3 TempPos = transform.position;
        ChoosedTimeline.Stop ();
        transform.position = TempPos;

        if (PlayerManager.instance.IsDead) {
            GetComponent<Animator> ().Play ("Maniac_Idle");
        }
        else {
            IsChasing = false;
            Vector3 Temp = PlayerHead.instance.gameObject.transform.position;
            Temp.y = transform.position.y;
            transform.LookAt (Temp);
            PlayerHead.instance.DisablePlayer (true);
            OldCam.enabled = false;
            CamPlaceForKilling.SetActive (true);
            GetComponent<NavMeshAgent> ().enabled = false;
            GetComponent<Animator> ().Play ("Maniac_Kill");
            foreach (GameObject TempObj in CloseBeforeKill) {
                TempObj.SetActive (false);
            }
        }
    }

    public void Punching ()
    {
        if (PlayerManager.instance.IsDead) {
            GetComponent<Animator> ().Play ("Maniac_Idle");
        }
        else {
            IsChasing = false;
            Vector3 Temp = PlayerHead.instance.gameObject.transform.position;
            Temp.y = transform.position.y;
            transform.LookAt (Temp);
            PlayerHead.instance.DisablePlayer (true);
            OldCam.enabled = false;
            CameraForBeating.SetActive (true);
            CameraForBeating.GetComponent<Animator> ().Play ("Camera");
            GetComponent<NavMeshAgent> ().enabled = false;
            GetComponent<Animator> ().Play ("Beating");
            foreach (GameObject TempObj in CloseBeforeKill) {
                TempObj.SetActive (false);
            }
        }
    }

    public void FinishPunching ()
    {
        StartCoroutine (PlayerManager.instance.Punched ());
        GetComponent<Animator> ().SetBool ("IsChasing", false);
        GetComponent<Animator> ().Play ("Maniac_Idle");
        OldCam.enabled = true;
        CameraForBeating.SetActive (false);
        Noise.instance.CanNoise = true;
        SmoothPunch = false;
    }

    public void Damage ()
    {
        PlayerManager.instance.InstaFade (true);
        StartCoroutine (EndGame ());
    }

    IEnumerator EndGame ()
    {
        yield return new WaitForSeconds (2);
        PlayerManager.instance.Dead ();
    }

    public void CheckClosedDoor ()
    {
        switch (LevelName) {
            case "Level_C":
                if (ClosedDoor.active) {
                    StopChase ();
                    ClosedDoor.GetComponent<BoxCollider> ().enabled = false;
                    Say (AngryClosedDoor);
                    Waiting = TimeToDestroyDoor;
                    StartCoroutine (DamageDoor ());
                }
                break;
        }
    }

    public void ChaseForPlayer ()
    {
        if (MusicManager.instance.AudioSrc.clip != ChaseMusic) MusicManager.instance.Silence ();

        switch (LevelName) {
            case "Level_A":
                if (ClosedDoor.active) {
                    ClosedDoor.GetComponent<BoxCollider> ().enabled = false;
                    Say (AngryClosedDoor);
                    Waiting = TimeToDestroyDoor;
                    MusicManager.instance.NewMusic (ChaseMusic);
                    StartCoroutine (DamageDoor ());
                }
                else {
                    Door.PowerUse ();
                    StartCoroutine (Chasing ());
                }
                break;
            case "Level_D":
                Door.PowerUse ();
                StartCoroutine (Chasing ());
                break;
            case "Level_B":
                Door.PowerUse ();
                StartCoroutine (Chasing ());
                break;
            case "Level_C":
                if (HidePlaceForEugene.Used) {
                    ChoosedTimeline = TimelineSearching;
                    ChoosedTimeline.Play ();
                    PlayTimeline = true;
                }
                else StartCoroutine (Chasing ());
                break;
        }
    }

    public IEnumerator Chasing (int WaitTime = 1, AudioClip NewChaseClip = null)
    {
        yield return new WaitForSeconds (WaitTime);
        bool CanChase = true;
        foreach (TriggerSpace Temp in CantChaseIfUsed) if (Temp.Used) CanChase = false;
        if (CanChase) {
            if (!PlayerManager.instance.IsDead) {
                switch (LevelName) {
                    case "Level_A":
                        if (Noise.instance.CheckSafe ()) { // Игрок в клетке, воспроизводим таймлайн
                            PlayerHead.instance.SetCanUseInteractive (false);
                            TimelineEasyKill.Play ();
                        }
                        else {
                            /*bool IsExitStep = true;
                            if (CheckForExitStep.Length == 0) IsExitStep = false;
                            for (int i = 0; i < CheckForExitStep.Length; i++) {
                                if (CheckForExitStep[i].GetComponent<Interactive> () != null && !CheckForExitStep[i].GetComponent<Interactive> ().Used) IsExitStep = false;
                                if (CheckForExitStep[i].GetComponent<TriggerSpace> () != null && !CheckForExitStep[i].GetComponent<TriggerSpace> ().Used) IsExitStep = false;
                            }*/
                            /*if (IsExitStep) {
                                Say (ChaseClip);
                                ChoosedTimeline = TimelineSearching;
                                ChoosedTimeline.Play ();
                            }
                            else {*/
                                MusicManager.instance.NewMusic (ChaseMusic);
                                GetComponent<NavMeshAgent> ().enabled = true;
                                GetComponent<Animator> ().SetBool ("IsChasing", true);
                                IsChasing = true;
                                GetComponent<NavMeshAgent> ().destination = PlayerHead.instance.transform.position;
                                Say (ChaseClip);
                            //}
                        }
                        break;
                    case "Level_D":
                        MusicManager.instance.NewMusic (ChaseMusic);
                        GetComponent<NavMeshAgent> ().enabled = true;
                        GetComponent<Animator> ().SetBool ("IsChasing", true);
                        IsChasing = true;
                        GetComponent<NavMeshAgent> ().destination = PlayerHead.instance.transform.position;
                        Say (ChaseClip);
                        break;
                    case "Level_B":
                        if (PlayTimeline) {
                            PlayTimeline = false;
                            Vector3 TempPos = transform.position;
                            ChoosedTimeline.Stop ();
                            transform.position = TempPos;
                        }
                        MusicManager.instance.NewMusic (ChaseMusic);
                        GetComponent<NavMeshAgent> ().enabled = true;
                        GetComponent<Animator> ().SetBool ("IsChasing", true);
                        IsChasing = true;
                        GetComponent<NavMeshAgent> ().destination = PlayerHead.instance.transform.position;
                        if (NewChaseClip != null) Say (NewChaseClip); 
                        else Say (ChaseClip);
                        if (DropIfEnable != null && DropIfEnable.active) {
                            Instantiate (DropIfEnable, DropIfEnable.transform.position, Quaternion.identity).GetComponent<Rigidbody> ().isKinematic = false;
                            DropIfEnable.SetActive (false);
                        }
                        break;
                    case "Level_C":
                        if (PlayTimeline) {
                            PlayTimeline = false;
                            Vector3 TempPos = transform.position;
                            ChoosedTimeline.Stop ();
                            transform.position = TempPos;
                        }
                        MusicManager.instance.NewMusic (ChaseMusic);
                        GetComponent<NavMeshAgent> ().enabled = true;
                        GetComponent<Animator> ().SetBool ("IsChasing", true);
                        IsChasing = true;
                        GetComponent<NavMeshAgent> ().destination = PlayerHead.instance.transform.position;
                        if (NewChaseClip != null) Say (NewChaseClip);
                        else Say (ChaseClip);
                        break;
                }
                
            }
        }
    }

    IEnumerator DamageDoor ()
    {
        float TimeToKnock = Random.Range (3, 6);
        if (TimeToKnock > Waiting) TimeToKnock = Waiting;
        yield return new WaitForSeconds (TimeToKnock);
        Waiting -= TimeToKnock;
        if (Waiting > 0) {
            DamageDoorSrc.Play ();
            StartCoroutine (DamageDoor ());
        }
        else {
            DestroyedDoorSrc.Play ();
            ClosedDoor.SetActive (false);
            Door.PowerUseWithoutSound (3f);
            StartCoroutine (Chasing (0));
        }
    }

    public void CheckCompressorDoor ()
    {
        if (!CompressorDoorLock.Used) {
            Vector3 TempPos = transform.position;
            ChoosedTimeline.Stop ();
            PlayTimeline = false;
            transform.position = TempPos;
            CompressorDoor.GetComponent<BoxCollider> ().enabled = false;
            Say (AngryClosedDoor);
            Waiting = TimeToDestroyDoorLevelB;
            MusicManager.instance.NewMusic (ChaseMusic);
            ClosedDoor.SetActive (true);
            StartCoroutine (DamageDoorLevelC ());
            FP_Controller.instance.SayWithDelay (EmilyGottaOut, 4f);
        }
        else if (!CompressorDoor.Used){
            Vector3 TempPos = transform.position;
            ChoosedTimeline.Stop ();
            PlayTimeline = false;
            transform.position = TempPos;
            StartCoroutine (Chasing (0));
        }
        else { // чек предметов
            List<Interactive> TempList = new List<Interactive> ();
            TempList.Add (ToggleLight);
            TempList.Add (CompressorDoor);
            TempList.Add (Door);
            StartEnter (TempList);
            Vector3 TempPos = transform.position;
            if (ChoosedTimeline == TimelineCompressorFromMax) InOtherRoom = false;
            ChoosedTimeline.Stop ();
            transform.position = TempPos;
            if (FailedCauseClip != null) {
                for (int i = 0; i < TimelineCompressorFailedes.Length; i++) {
                    if (TimelineCompressorFailedes[i].TmlnForEqual == ChoosedTimeline) {
                        ChoosedTimeline = TimelineCompressorFailedes[i].TargetTmln;
                        break;
                    }
                }
            }
            else {
                for (int i = 0; i < TimelineCompressorSuccesses.Length; i++) {
                    if (TimelineCompressorSuccesses[i].TmlnForEqual == ChoosedTimeline) {
                        ChoosedTimeline = TimelineCompressorSuccesses[i].TargetTmln;
                        break;
                    }
                }
            }
            ChoosedTimeline.Play ();
        }
    }

    IEnumerator DamageDoorLevelC ()
    {
        float TimeToKnock = Random.Range (3, 6);
        if (TimeToKnock > Waiting) TimeToKnock = Waiting;
        yield return new WaitForSeconds (TimeToKnock);
        Waiting -= TimeToKnock;
        int UsedLevers = 0;
        for (int i = 0; i < ForBreakDoor.Length; i++) {
            UsedLevers += (ForBreakDoor[i].IsUsed ()) ? 1 : 0;
        }
        if (Waiting > 0 && UsedLevers < 1) {
            DamageDoorSrc.Play ();
            StartCoroutine (DamageDoorLevelC ());
        }
        else {
            DestroyedDoorSrc.Play ();
            ClosedDoor.SetActive (false);
            CompressorDoor.PowerUseWithoutSound (3f);
            StartCoroutine (Chasing (0));
        }
    }

    public void DestroyDoorLevelB ()
    {
        DestroyedDoorSrc.Play ();
        ClosedDoor.SetActive (false);
        CompressorDoor.PowerUseWithoutSound (3f);
        StartCoroutine (Chasing (0));
    }

    public void CheckYouNeedToLock ()
    {
        if (YouNeedToLockPassword.IsUsed () && Noise.instance.makedNoise && !YouNeedToLockSaid && Compressor.IsUsed ()) {
            YouNeedToLockSaid = true;
            FP_Controller.instance.Say (EmilyYouNeedToLock);
        }
    }

    public void CheckComeOn ()
    {
        if (YouNeedToLockPassword.IsUsed () && PlayTimeline && Compressor.IsUsed ()) {
            FP_Controller.instance.Say (EmilyComeOn);
        }
        else if (PlayTimeline && Compressor.IsUsed ()) {
            if (!CloseAdvice) {
                CloseAdvice = true;
                FP_Controller.instance.Say (EmilyCloseAdvice);
            }
        }
    }

    public void StartEnter (List<Interactive> SkipCheck = null)
    {
        switch (LevelName) {
            case "Level_A":
                if (ClosedDoor.active || Rage) ChaseForPlayer ();
                else {
                    MusicManager.instance.Silence ();
                    PlayerHead.instance.SetCanUseInteractive (false);
                    FailedCauseClip = null;
                    CheckInteractive ();
                    if (FailedCauseClip != null) {
                        if (CheckForDelivery.Used && !Delivered) {
                            ChoosedTimeline = TimelineDeliveryFailed;
                            Delivered = true;
                        }
                        else ChoosedTimeline = TimelineFailed;
                    }
                    else {
                        if (CheckForDelivery.Used && !Delivered) {
                            ChoosedTimeline = TimelineDeliverySuccess;
                            Delivered = true;
                        }
                        else ChoosedTimeline = TimelineSuccess;
                    }
                    if (!PlayerManager.instance.IsDead) {
                        ChoosedTimeline.Play ();
                        PlayTimeline = true;
                    }
                    Door.PowerUse ();
                }
                break;
            case "Level_D":
                if (Rage) ChaseForPlayer ();
                else {
                    MusicManager.instance.Silence ();
                    //PlayerHead.instance.SetCanUseInteractive (false);
                    FailedCauseClip = null;
                    CheckInteractive ();
                    if (FailedCauseClip != null) {
                        if (CheckForDelivery.Used && !Delivered) {
                            ChoosedTimeline = TimelineDeliveryFailed;
                            Delivered = true;
                        }
                        else ChoosedTimeline = TimelineFailed;
                    }
                    else {
                        List<PlayableDirector> TempSuccesses = new List<PlayableDirector> ();
                        foreach (Successes Temp in TimelineSuccesses) {
                            if (Temp.Requiere == null) TempSuccesses.Add (Temp.Tmln);
                            else if (Temp.Requiere.IsFree ()) TempSuccesses.Add (Temp.Tmln);
                        }
                        ChoosedTimeline = TempSuccesses[Random.Range (0, TempSuccesses.Count)];
                    }
                    if (!PlayerManager.instance.IsDead) {
                        ChoosedTimeline.Play ();
                        PlayTimeline = true;
                    }
                    Door.PowerUse ();
                }
                break;
            case "Level_B":
                if (Rage) ChaseForPlayer ();
                else {
                    MusicManager.instance.Silence ();
                    //PlayerHead.instance.SetCanUseInteractive (false);
                    FailedCauseClip = null;
                    CheckInteractive (SkipCheck);
                    if (FailedCauseClip != null) {
                        ChoosedTimeline = TimelineFailed;
                        if (CheckForMaxNoise != null && CheckForMaxNoise.Used && !MaxNoised && MaxHasLever) {
                            MaxNoised = true;
                            InOtherRoom = true;
                            ChoosedTimeline = TimelineMaxNoise;
                        }
                        else {
                            if (Compressor.IsUsed () && (!CompressorDoor.IsUsed () && !ToggleLight.IsUsed ())) { // Включен компрессор. Маньяк идет выключать его в первую очередь, если не открыта дверь в ванную и выключен свет
                                if (InOtherRoom) ChoosedTimeline = TimelineCompressorFromMax;
                                else ChoosedTimeline = TimelineCompressor;
                            }
                            else if (InOtherRoom) {
                                for (int i = 0; i < TimelineFailedesFromMax.Length; i++) {
                                    if (TimelineFailedesFromMax[i].TmlnForEqual == ChoosedTimeline) {
                                        ChoosedTimeline = TimelineFailedesFromMax[i].TargetTmln;
                                        break;
                                    }
                                }
                                if (ChoosedTimeline == TimelineFailedBeforeCompressor) ChoosedTimeline = TimelineMaxFailedBeforeCompressor;
                            }
                            /*if (CheckForDelivery != null && CheckForDelivery.Used && !Delivered) {
                                Delivered = true;
                                InOtherRoom = true;
                                MaxHasLever = LeverForMax.active;
                            }*/
                        }
                    }
                    else {
                        if (CheckForMaxNoise != null && CheckForMaxNoise.Used && !MaxNoised && MaxHasLever) {
                            MaxNoised = true;
                            InOtherRoom = true;
                            ChoosedTimeline = TimelineMaxNoise;
                        }
                        else {
                            if (CheckForDelivery != null && CheckForDelivery.Used && !Delivered) {
                                ChoosedTimeline = TimelineDeliverySuccess;
                                Delivered = true;
                                InOtherRoom = true;
                                MaxHasLever = LeverForMax.active;
                            }
                            else {
                                List<PlayableDirector> TempSuccesses = new List<PlayableDirector> ();
                                if (InOtherRoom) {
                                    foreach (Successes Temp in TimelineSuccessesFromMax) {
                                        if (Temp.Requiere == null) TempSuccesses.Add (Temp.Tmln);
                                        else if (Temp.Requiere.IsFree ()) TempSuccesses.Add (Temp.Tmln);
                                    }
                                }
                                else {
                                    foreach (Successes Temp in TimelineSuccesses) {
                                        if (Temp.Requiere == null) TempSuccesses.Add (Temp.Tmln);
                                        else if (Temp.Requiere.IsFree ()) TempSuccesses.Add (Temp.Tmln);
                                    }
                                }
                                ChoosedTimeline = TempSuccesses[Random.Range (0, TempSuccesses.Count)];
                                if (Compressor.IsUsed ()) { // Включен компрессор. Маньяк идет выключать его, но не агрится
                                    if (InOtherRoom) ChoosedTimeline = TimelineCompressorFromMax;
                                    else ChoosedTimeline = TimelineCompressor;
                                }
                            }
                        }
                    }
                    if (!PlayerManager.instance.IsDead) {
                        ChoosedTimeline.Play ();
                        PlayTimeline = true;
                    }
                }
                break;
            case "Level_C":
                if (Rage) ChaseForPlayer ();
                else {
                    MusicManager.instance.Silence ();
                    //PlayerHead.instance.SetCanUseInteractive (false);
                    FailedCauseClip = null;
                    CheckInteractive ();
                    if (FailedCauseClip != null) {
                        ChoosedTimeline = TimelineFailed;
                    }
                    else {
                        List<PlayableDirector> TempSuccesses = new List<PlayableDirector> ();
                        foreach (Successes Temp in TimelineSuccesses) {
                            if (Temp.Requiere == null) TempSuccesses.Add (Temp.Tmln);
                            else if (Temp.Requiere.IsFree ()) TempSuccesses.Add (Temp.Tmln);
                        }
                        ChoosedTimeline = TempSuccesses[Random.Range (0, TempSuccesses.Count)];
                    }
                    if (!PlayerManager.instance.IsDead) {
                        ChoosedTimeline.Play ();
                        PlayTimeline = true;
                    }
                    //Door.PowerUse ();
                }
                break;
        }
    }

    public void StartMaxEscape ()
    {
        MaxLogic.instance.StartEscape ();
    }

    public PlayableDirector NeedChangeNoise ()
    {
        switch (LevelName) {
            case "Level_B":
                if (InOtherRoom) {
                    return TimelineMaxReturnNoise;
                }
                break;
        }

        return null;
    }

    public void Say (AudioClip Clip)
    {
        List<AudioClip> Clips = new List<AudioClip> ();
        Clips.Add (Resources.Load<AudioClip> ("Voice acting/Maniac/" + "Rus" + "/" + Clip.name));
        Clips.Add (Resources.Load<AudioClip> ("Voice acting/Maniac/" + "Eng" + "/" + Clip.name));
        Mouth.clip = Clips[PlayerPrefs.GetInt ("LanguageNum", 0)];
        Mouth.Play ();
        Subtitles.instance.NewSubtitle (Mouth.clip);
    }

    public void SayFailedCause ()
    {
        Say (FailedCauseClip);
    }

    public void ElectricityPlayer ()
    {
        PlayerManager.instance.Electricity ();
    }

    public void GasPlayer ()
    {
        PlayerManager.instance.Gas ();
        switch (LevelName) {
            case "Level_B":
                if (CheckForDelivery != null && CheckForDelivery.Used && !Delivered) {
                    Delivered = true;
                    InOtherRoom = true;
                    MaxHasLever = LeverForMax.active;
                    foreach (Interactive Temp in LockAfterGasBeforeMax) Temp.SetLock (10f);
                    Door.ChangeRequiereItem (-1);
                }
                break;
        }
    }

    public void CheckInteractive (List<Interactive> SkipCheck = null)
    {
        if (InOtherRoom) {
            for (int i = ForCheck.Length - 1; i >= 0; i--) {
                if (!ForCheck[i].enabled || (SkipCheck != null && SkipCheck.Find (x => x == ForCheck[i]))) {
                    continue;
                }
                if (ForCheck[i].Check ()) {
                    FailedCauseClip = ForCheck[i].GetManiacReplic ();
                    if (ForCheck[i].ManiacTimeline != null) {
                        TimelineFailed = (InOtherRoom) ? ((ForCheck[i].ManiacTimelineInOtherRoom != null) ? ForCheck[i].ManiacTimelineInOtherRoom : ForCheck[i].ManiacTimeline) : ForCheck[i].ManiacTimeline;
                    }
                    break;
                }
            }
        }
        else {
            for (int i = 0; i < ForCheck.Length; i++) {
                if (!ForCheck[i].enabled || (SkipCheck != null && SkipCheck.Find (x => x == ForCheck[i]))) {
                    continue;
                }
                if (ForCheck[i].Check ()) {
                    FailedCauseClip = ForCheck[i].GetManiacReplic ();
                    if (ForCheck[i].ManiacTimeline != null) {
                        TimelineFailed = (InOtherRoom) ? ((ForCheck[i].ManiacTimelineInOtherRoom != null) ? ForCheck[i].ManiacTimelineInOtherRoom : ForCheck[i].ManiacTimeline) : ForCheck[i].ManiacTimeline;
                    }
                    break;
                }
            }
        }
        if (LevelName == "Level_B") {
            if (SkipCheck != null && SkipCheck.Find (x => x == Door)) {
                if (!InOtherRoom) {
                    if (!Door.Used) {
                        FailedCauseClip = Door.GetManiacReplic ();
                        if (Door.ManiacTimeline != null) {
                            TimelineFailed = (InOtherRoom) ? ((Door.ManiacTimelineInOtherRoom != null) ? Door.ManiacTimelineInOtherRoom : Door.ManiacTimeline) : Door.ManiacTimeline;
                        }
                    }
                }
            }
        }
    }

    public void RefreshManiac ()
    {
        if (!InOtherRoom) transform.position = StartPos;
    }

    public void GamePaused ()
    {
        Mouth.Pause ();
    }

    public void GameUnpaused ()
    {
        Mouth.UnPause ();
    }

    public void UseListForOpen ()
    {
        foreach (Interactive Temp in ListForOpen) {
            if (Temp.gameObject.active) {
                Temp.PowerUse ();
                break;
            }
        }
    }

    public void AgainMakedNoise ()
    {
        Rage = true;
        Say (RageClip);
        MusicManager.instance.NewMusic (ChaseMusic);
    }

    public void StopChase ()
    {
        GetComponent<NavMeshAgent> ().enabled = false;
        IsChasing = false;
        GetComponent<Animator> ().SetBool ("IsChasing", false);
        GetComponent<Animator> ().Play ("Maniac_Idle");
    }

    public void WaitHere ()
    {
        StartPos = transform.position;
        if (PlayTimeline) ChoosedTimeline.Stop ();
        transform.position = StartPos;
        GetComponent<Animator> ().Play ("Maniac_Idle");
    }

    IEnumerator ContinueChase ()
    {
        yield return new WaitForSeconds (3);
        bool CanChase = true;
        foreach (TriggerSpace Temp in CantChaseIfUsed) if (Temp.Used) CanChase = false;
        if (CanChase) {
            GetComponent<NavMeshAgent> ().enabled = true;
            GetComponent<Animator> ().SetBool ("IsChasing", true);
            IsChasing = true;
            GetComponent<NavMeshAgent> ().destination = PlayerHead.instance.transform.position;
        }
    }

    public void Crawling ()
    {
        if (GetComponent<Animator> ().GetBool ("IsCrawling")) {
            GetComponent<Animator> ().SetBool ("IsCrawling", false);
            GetComponent<NavMeshAgent> ().speed = OldSpeed;
        }
        else {
            GetComponent<Animator> ().SetBool ("IsCrawling", true);
            GetComponent<NavMeshAgent> ().speed = 1;
        }
    }

    public void StartChaseForSmoothPunch ()
    {
        if (PlayTimeline) {
            PlayTimeline = false;
            Vector3 TempPos = transform.position;
            ChoosedTimeline.Stop ();
            transform.position = TempPos;
        }
        SmoothPunch = true;
        GetComponent<NavMeshAgent> ().enabled = true;
        GetComponent<Animator> ().SetBool ("IsChasing", true);
        IsChasing = true;
        GetComponent<NavMeshAgent> ().destination = PlayerHead.instance.transform.position;
    }

    public bool ManiacNear ()
    {
        return IsChasing || PlayTimeline || ((ChoosedTimeline != null) ? ChoosedTimeline.state != PlayState.Paused : false) || PlayerHead.instance.IsDisabledPlayer || Noise.instance.makedNoise || PlayerManager.instance.IsDead;
    }

    #region Clips
    public void PlayLeftStep ()
    {
        AudioSrc.clip = LeftStep;
        AudioSrc.Play ();
    }
    public void PlayRightStep ()
    {
        AudioSrc.clip = RightStep;
        AudioSrc.Play ();
    }
    #endregion
}
