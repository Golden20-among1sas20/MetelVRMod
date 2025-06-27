using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent (typeof (CharacterController))]
[RequireComponent(typeof(FP_Input))]
[RequireComponent(typeof(FP_CameraLook))]
[RequireComponent(typeof(FP_FootSteps))]
public class FP_Controller : MonoBehaviour
{
    public Image ChangeStateBtn;
    public AudioSource Mouth;
    public bool CrouchAtStart = false;
    public bool canControl = true;
    public bool trapped = false;
    public float gravity = 20.0f;
	public float walkSpeed = 6.0f;
    public float runSpeed = 11.0f;
    public float jumpForce = 8.0f;
    public float crouchSpeed = 2.0F;
    public float crouchHeight = 1.0F;

    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;

    public bool airControl = true;
    public bool canCrouch = true;
    public bool canJump = true;
    public bool canRun = true;
    public bool crouch = false;

    [HideInInspector]
    public CharacterController controller;

    private Vector3 moveDirection;
    private Vector3 contactPoint;
    private Vector3 hitNormal;
    private AudioSource JumpLandSource;
    private FP_FootSteps footSteps;
	private Transform myTransform;
    private FP_Input playerInput;
	private RaycastHit hit;


    private bool playerControl = false;
    private bool isCrouching = false;
    private bool grounded = false;
    private bool sliding = false;
    private bool jump = false;
    private bool run = false;
    private bool ChangedState = true;
    private bool CantStand = false;

    private int antiBunnyHopFactor = 1;
	private int jumpTimer;
    private int landTimer;
    private int jumpState;
    private int runState;

    private float antiBumpFactor = 0.75F;
    private float inputModifyFactor;
    private float slideSpeed = 2.0F;
    private float minCrouchHeight;
    private float inputX, inputZ;
    private float fallStartLevel;
    private float defaultHeight;
    private float rayDistance;
    private float slideLimit;
    private float speed;

    private string surfaceTag;

    static private int SpeedOfChangeState = 15;

    static FP_Controller _instance;
    public static FP_Controller instance { get { return _instance; } }


    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<FP_Input>();
        footSteps = GetComponent<FP_FootSteps>();

        if (_instance == null) _instance = this;
        else Debug.LogError ("More than one FP_Controller");
    }
	
	void Start() 
    {
		defaultHeight = controller.height;
        minCrouchHeight = crouchHeight > controller.radius * 2 ? crouchHeight : controller.radius * 2;
		myTransform = transform;
		speed = walkSpeed;
		rayDistance = controller.height * 0.5F + controller.radius;
		slideLimit = controller.slopeLimit - 0.1F;
		jumpTimer = antiBunnyHopFactor;
        JumpLandSource = gameObject.AddComponent<AudioSource>();

        crouch = CrouchAtStart;
        if (CrouchAtStart) {
            controller.center = new Vector3 (controller.center.x, -(defaultHeight - minCrouchHeight) / 2, controller.center.z);
            controller.height = minCrouchHeight;
            playerInput.SetToggle (true);
        }
	}
	
	void FixedUpdate()
    {
		// If both horizontal and vertical are used simultaneously, limit speed (if allowed), so the total doesn't exceed normal move speed
		inputModifyFactor = (inputX != 0.0F && inputZ != 0.0F)? 0.7071F : 1.0F;
		
		if (grounded) {
			sliding = false;
			// See if surface immediately below should be slid down. We use this normally rather than a ControllerColliderHit point,
			// because that interferes with step climbing amongst other annoyances
			if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, rayDistance)) {
				if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit && CanSlide())
					sliding = true;
			}
			// However, just raycasting straight down from the center can fail when on steep slopes
			// So if the above raycast didn't catch anything, raycast down from the stored ControllerColliderHit point instead
			else {
				Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit);
				if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit && CanSlide())
					sliding = true;
			}

            speed = isCrouching || !CanStand() ? crouchSpeed : run ? canRun ? runSpeed : walkSpeed : walkSpeed;
            if (Mathf.RoundToInt (controller.height * 100) == Mathf.RoundToInt (defaultHeight * 100)) { // если игрок стоит, то скорость пешая
                speed = walkSpeed;
            }

            // If sliding (and it's allowed), or if we're on an object tagged "Slide", get a vector pointing down the slope we're on
            if (sliding) 
            {
				hitNormal = hit.normal;
				moveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
				Vector3.OrthoNormalize (ref hitNormal, ref moveDirection);
				moveDirection *= slideSpeed;
				playerControl = false;
			}
			// Otherwise recalculate moveDirection directly from axes, adding a bit of -y to avoid bumping down inclines
			else
            {
				moveDirection = new Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputZ * inputModifyFactor);
				moveDirection = myTransform.TransformDirection(moveDirection) * speed;
				playerControl = true;
			}
			
			// Jump! But only if canJump, the jump button has been released and player has been grounded for a given number of frames
			if (!jump)
				jumpTimer++;
			else if (canJump && jumpTimer >= antiBunnyHopFactor) 
            {
				moveDirection.y = jumpForce;
				jumpTimer = 0;
			}
		}
		else 
        {
			// If air control is allowed, check movement but don't touch the y component
			if (airControl && playerControl)
            {
				moveDirection.x = inputX * speed * inputModifyFactor;
				moveDirection.z = inputZ * speed * inputModifyFactor;
				moveDirection = myTransform.TransformDirection(moveDirection);
			}
		}
		
		// Apply gravity
		moveDirection.y -= gravity * Time.deltaTime;
		// Move the controller, and set grounded true or false depending on whether we're standing on something
		if (!PlayerHead.instance.IsDisabledPlayer && !trapped) grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
    }

    void Update()
    {
        if (!canControl)
            return;

        switch (playerInput.UseMobileInput)
        {
            case true:
                runState = playerInput.Run() && canRun && !isCrouching ? 1 : 0;
                inputX = playerInput.MoveInput().x;
                inputZ = playerInput.MoveInput().z + runState;
                crouch = playerInput.Crouch();
                run = playerInput.Run();
                jump = playerInput.Jump();
            break;
            case false:
                inputX = Input.GetAxis("Horizontal");
                inputZ = Input.GetAxis("Vertical");
                if (Input.anyKeyDown) if (PlayerManager.instance.IsShowLetter ()) PlayerManager.instance.HideLetter ();
                if (Input.GetKeyDown (crouchKey) && ChangedState && !CantStand) crouch = !crouch;
                run = Input.GetKey(runKey);
                jump = Input.GetKey(jumpKey);
            break;
        }

        if (jumpState == 0 && CanStand() && jump && jumpTimer >= antiBunnyHopFactor)
        {
            PlaySound(footSteps.jumpSound, JumpLandSource);
            jumpState++;
        }

        if ((Mathf.Abs((transform.position - contactPoint).magnitude) > 2))
            landTimer = 1;

        isCrouching = crouch && canCrouch;

        if (grounded)
        {
            if (isCrouching)
            {
                controller.center = Vector3.Lerp(controller.center, new Vector3(controller.center.x, -(defaultHeight - minCrouchHeight) / 2, controller.center.z), SpeedOfChangeState * Time.deltaTime);
                controller.height = Mathf.Lerp(controller.height, minCrouchHeight, SpeedOfChangeState * Time.deltaTime);
                if (ChangedState) {
                    if (Mathf.RoundToInt (controller.height * 100) != Mathf.RoundToInt (minCrouchHeight * 100)) {
                        ChangedState = false;
                        ChangeStateBtn.raycastTarget = false;
                    }
                }
                else {
                    if (Mathf.RoundToInt (controller.height * 100) == Mathf.RoundToInt (minCrouchHeight * 100)) {
                        ChangedState = true;
                        ChangeStateBtn.raycastTarget = true;
                    }
                }
            }
            else
            {
                if (CanStand())
                {
                    controller.center = Vector3.Lerp(controller.center, Vector3.zero, SpeedOfChangeState * Time.deltaTime);
                    controller.height = Mathf.Lerp(controller.height, defaultHeight, SpeedOfChangeState * Time.deltaTime);
                    if (ChangedState) {
                        if (Mathf.RoundToInt (controller.height * 100) != Mathf.RoundToInt (defaultHeight * 100)) {
                            ChangedState = false;
                            ChangeStateBtn.raycastTarget = false;
                        }
                    }
                    else {
                        if (Mathf.RoundToInt (controller.height * 100) == Mathf.RoundToInt (defaultHeight * 100)) {
                            ChangedState = true;
                            ChangeStateBtn.raycastTarget = true;
                        }
                    }
                }
            }
        }
    }

    void OnControllerColliderHit (ControllerColliderHit hit) {
		if (!IsGrounded () && landTimer == 1)
            PlaySound(footSteps.landSound, JumpLandSource);
		landTimer = 0;
        jumpState = 0;
		contactPoint = hit.point;
        surfaceTag = hit.collider.tag;
	}

	void PlaySound(AudioClip audio, AudioSource source)
	{
		source.clip = audio;
		if (audio)
			source.Play ();
	}

	public bool IsGrounded()
	{
		return grounded;
	}

    public bool IsCrouching()
    {
        return crouch;
    }

    public bool IsRunning()
    {
        return run;
    }

	private bool CanStand()
	{
        return true;

        RaycastHit hitAbove = new RaycastHit();
        
        return !Physics.SphereCast (controller.bounds.center, controller.radius, Vector3.up, out hitAbove,
                                    controller.height / 2 + 0.5F, ~0, QueryTriggerInteraction.Ignore);
	}

	private bool CanSlide()
	{
		return new Vector3 (controller.velocity.x, 0, controller.velocity.z).magnitude < walkSpeed/2;
	}

    public string SurfaceTag()
    {
        return surfaceTag;
    }

    public void SetCantStand (bool Flag)
    {
        CantStand = Flag;
        ChangeStateBtn.gameObject.SetActive (!CantStand);
    }

    public void GamePaused ()
    {
        Mouth.Pause ();
    }

    public void GameUnpaused ()
    {
        Mouth.UnPause ();
    }

    public void Say (AudioClip Clip)
    {
        List<AudioClip> Clips = new List<AudioClip> ();
        Clips.Add (Resources.Load<AudioClip> ("Voice acting/" + PlayerManager.instance.PlayerName + "/" + "Rus" + "/" + Clip.name));
        Clips.Add (Resources.Load<AudioClip> ("Voice acting/" + PlayerManager.instance.PlayerName + "/" + "Eng" + "/" + Clip.name));
        if (Mouth.isPlaying && Mouth.clip == Clips[PlayerPrefs.GetInt ("LanguageNum", 0)]) return;
        Mouth.clip = Clips[PlayerPrefs.GetInt ("LanguageNum", 0)];
        Mouth.Play ();
        Subtitles.instance.NewSubtitle (Mouth.clip);
    }

    public void SayWithDelay (AudioClip NewClip, float Delay = 0f)
    {
        StartCoroutine (SayWithDelayC (NewClip, Delay));
    }

    public IEnumerator SayWithDelayC (AudioClip NewClip, float Delay = 0f)
    {
        yield return new WaitForSeconds (Delay);
        if (!PlayerManager.instance.IsDead) {
            List<AudioClip> Clips = new List<AudioClip> ();
            Clips.Add (Resources.Load<AudioClip> ("Voice acting/" + PlayerManager.instance.PlayerName + "/" + "Rus" + "/" + NewClip.name));
            Clips.Add (Resources.Load<AudioClip> ("Voice acting/" + PlayerManager.instance.PlayerName + "/" + "Eng" + "/" + NewClip.name));
            FP_Controller.instance.Mouth.clip = Clips[PlayerPrefs.GetInt ("LanguageNum", 0)];
            FP_Controller.instance.Mouth.Play ();
            Subtitles.instance.NewSubtitle (NewClip);
        }
    }
}