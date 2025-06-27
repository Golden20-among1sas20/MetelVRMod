using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertTimer : MonoBehaviour
{
    Text Target;

    public float Waiting { get; private set;} = 30f;
    bool Started = false;
    public bool HideTimer = false;

    private void Start ()
    {
        Target = GetComponent<Text> ();
        gameObject.SetActive (false);
    }

    private void Update ()
    {
        if (Started) {
            Waiting -= Time.deltaTime;
            Target.text = (Mathf.CeilToInt (Waiting)).ToString ();
            if (HideTimer) {
                Target.color = new Color32 (255, 255, 255, 0);
            }
            else if (Waiting < 7f) {
                if (Waiting > 6.75f) Target.color = new Color32 (255, 255, 255, (byte)(255 * ((Waiting - 6.75f) / 0.25f)));
                else Target.color = new Color32 (255, 255, 255, 0);
            }
            if (Waiting <= 0f) {
                Started = false;
                gameObject.SetActive (false);
            }
        }
    }

    public void StartTimer (float Value = 30f)
    {
        gameObject.SetActive (true);
        Waiting = Value;
        Started = true;
        Target.color = new Color32 (255, 255, 255, 255);
    }
}
