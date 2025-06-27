using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpText : MonoBehaviour
{
    TMPro.TextMeshProUGUI Target;
    Color32 TargetColor;
    float Waiting = 0f;
    bool IsShowing = false;

    static float ShowTime = 3;

    private void Start ()
    {
        Target = GetComponent<TMPro.TextMeshProUGUI> ();
        TargetColor = Target.color;
    }

    private void Update ()
    {
        if (IsShowing) {
            Waiting += Time.deltaTime;
            if (Waiting >= ShowTime) {
                IsShowing = false;
                Target.text = "";
            }
        }
    }

    public void ShowText (string NewText)
    {
        if (NewText == "") return;

        Target.text = Settings.instance.GetTranslatedPhrase (NewText);
        Target.color = new Color32 (TargetColor.r, TargetColor.g, TargetColor.b, TargetColor.a);
        Waiting = 0f;
        IsShowing = true;
    }
}
