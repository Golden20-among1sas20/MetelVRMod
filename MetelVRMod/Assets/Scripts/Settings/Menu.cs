using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject MainFone;
    [SerializeField] GameObject MenuPause;
    [SerializeField] GameObject Options;
    [SerializeField] GameObject Restart;
    [SerializeField] GameObject QuitMenu;
    [SerializeField] GameObject[] BlockEscapeIfActive;
    [SerializeField] AudioSource[] PauseIfPaused;

    private void Update ()
    {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            foreach (GameObject Temp in BlockEscapeIfActive) {
                if (Temp.active) return;
            }
            if (MainFone.active) {
                if (MenuPause.active) CloseMenu ();
                else OpenMenu ();
            }
            else OpenMenu ();
        }
    }

    public void OpenMenu ()
    {
        if (PlayerManager.instance.IsDead) return;

        MainFone.SetActive (true);
        MenuPause.SetActive (true);
        Options.SetActive (false);
        Restart.SetActive (false);
        QuitMenu.SetActive (false);

        if (!FP_Input.instance.UseMobileInput) {
            Cursor.visible = true;
            Screen.lockCursor = false;
        }
        PlayerHead.instance.DisablePlayer (true);
        Time.timeScale = 0;
        MusicManager.instance.GamePaused ();
        foreach (Interactive Temp in GameObject.FindObjectsOfType<Interactive> ()) {
            if (Temp.gameObject.active) Temp.GamePaused ();
        }
        foreach (AudioSource Temp in PauseIfPaused) {
            Temp.Pause ();
        }
        FP_Controller.instance.GamePaused ();
        ManiacLogic.instance.GamePaused ();
        if (DialogSystem.instance != null) DialogSystem.instance.GamePaused ();
    }

    public void OpenOptions ()
    {
        MainFone.SetActive (true);
        MenuPause.SetActive (false);
        Options.SetActive (true);
        Restart.SetActive (false);
        QuitMenu.SetActive (false);
    }

    public void CloseMenu ()
    {
        MainFone.SetActive (false);

        if (!FP_Input.instance.UseMobileInput) {
            Cursor.visible = false;
            Screen.lockCursor = true;
        }
        PlayerHead.instance.DisablePlayer (false);
        Time.timeScale = 1;
        MusicManager.instance.GameUnpaused ();
        foreach (Interactive Temp in GameObject.FindObjectsOfType<Interactive> ()) {
            if (Temp.gameObject.active) Temp.GameUnpaused ();
        }
        foreach (AudioSource Temp in PauseIfPaused) {
            Temp.UnPause ();
        }
        FP_Controller.instance.GameUnpaused ();
        ManiacLogic.instance.GameUnpaused ();
        if (DialogSystem.instance != null) DialogSystem.instance.GamePaused ();
    }
}
