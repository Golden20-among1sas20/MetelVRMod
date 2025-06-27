using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] GameObject LoadingCircle;
    [SerializeField] GameObject TapToContinue;
    [SerializeField] TextMeshProUGUI AdviceText;
    [SerializeField] string[] Advices;

    AsyncOperation NewScene;

    static LoadingScene _instance;
    public static LoadingScene instance { get { return _instance; } }

    private void Awake ()
    {
        if (_instance == null) _instance = this;
        else Debug.LogError ("More than one LoadingScene");
    }

    private void Start ()
    {
        AdviceText.text = Settings.instance.GetTranslatedPhrase (Advices[Random.Range (0, Advices.Length)]);
        StartLevel (PlayerPrefs.GetString ("LoadLevel", "Level_A"));
    }

    public void StartLevel (string Name)
    {
        StartCoroutine (StartLevelAsync (Name));
    }

    IEnumerator StartLevelAsync (string Name)
    {
        NewScene = SceneManager.LoadSceneAsync (Name, LoadSceneMode.Single);
        NewScene.allowSceneActivation = false;
        while (NewScene.progress < 0.9f) {
            LoadingCircle.transform.Rotate (new Vector3 (0, 0, 360 * Time.deltaTime));
            yield return null;
        }
        if (NewScene.progress >= 0.9f && !NewScene.allowSceneActivation) {
            LoadingCircle.SetActive (false);
            TapToContinue.SetActive (true);
            NewScene.allowSceneActivation = true;
        }
    }

    public void ContinueLevel ()
    {
        NewScene.allowSceneActivation = true;
    }
}
