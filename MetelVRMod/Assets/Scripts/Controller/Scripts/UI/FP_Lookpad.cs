using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(CanvasGroup))]
public class FP_Lookpad : MonoBehaviour {

    [SerializeField] Slider SensitivitySlider;

    private Vector2 touchInput, prevDelta, dragInput, startPos;
    private bool isPressed;
    private EventTrigger eventTrigger;
    private CanvasGroup canvasGroup;
    private float Sensitivity = 2f;
    int currentTouchID = -1;

    static FP_Lookpad _instance;
    public static FP_Lookpad instance { get { return _instance; } }

    private void Awake ()
    {
        if (_instance == null) _instance = this;
        else Debug.LogError ("More than one FP_Lookpad");
    }

    void Start()
    {
        SetupListeners();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        Sensitivity = PlayerPrefs.GetFloat ("Sensitivity", 1f);
        SensitivitySlider.value = PlayerPrefs.GetFloat ("Sensitivity", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        touchInput = (dragInput - prevDelta) / Time.deltaTime;
        prevDelta = dragInput;
    }

    private void OnDisable ()
    {
        isPressed = false;
    }

    //Setup events;
    void SetupListeners()
    {
        eventTrigger = gameObject.GetComponent<EventTrigger>();

        var a = new EventTrigger.TriggerEvent();
        a.AddListener(data =>
        {
            if (isPressed) return;
            var evData = (PointerEventData)data;
            foreach (Touch Temp in Input.touches) {
                if (Temp.phase == TouchPhase.Began) {
                    currentTouchID = Temp.fingerId;
                    break;
                }
            }
            data.Use();
            isPressed = true;
            prevDelta = dragInput = startPos = evData.position;
        });

        eventTrigger.triggers.Add(new EventTrigger.Entry { callback = a, eventID = EventTriggerType.PointerDown });

        var d = new EventTrigger.TriggerEvent ();
        d.AddListener (data => {
            isPressed = false;
        });

        eventTrigger.triggers.Add (new EventTrigger.Entry { callback = d, eventID = EventTriggerType.PointerUp });


        var b = new EventTrigger.TriggerEvent();
        b.AddListener(data =>
        {
            var evData = (PointerEventData)data;
            if (evData.pointerId == currentTouchID) {
                data.Use ();
                dragInput = evData.position;
            }
        });

        eventTrigger.triggers.Add(new EventTrigger.Entry { callback = b, eventID = EventTriggerType.Drag });


        var c = new EventTrigger.TriggerEvent();
        c.AddListener(data =>
        {
            touchInput = Vector2.zero;
        });

        eventTrigger.triggers.Add(new EventTrigger.Entry { callback = c, eventID = EventTriggerType.EndDrag });
    }

    //Returns drag vector;
    public Vector2 LookInput()
    {
        return touchInput * Time.deltaTime * Sensitivity;
    }

    //Returns whether or not button is pressed
    public bool IsPressed()
    {
        return isPressed;
    }

    public bool IsDragged ()
    {
        return startPos != prevDelta;
    }

    public void ChangeSensitivity (float NewSensitivity)
    {
        Sensitivity = NewSensitivity;
        PlayerPrefs.SetFloat ("Sensitivity", NewSensitivity);
    }
}
