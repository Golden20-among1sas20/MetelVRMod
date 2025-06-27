using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    [System.Serializable]
    public class ObjAndMarker
    {
        public GameObject Object;
        public GameObject Marker;
    }

    [System.Serializable]
    public class ListOfVariations
    {
        public Variation Var;
        public ObjAndMarker[] Objects;
    }

    [SerializeField] ListOfVariations[] ListOfVar;
    [SerializeField] GameObject HelpHint;

    static Helper _instance;
    public static Helper instance { get { return _instance; } }

    private void Awake ()
    {
        if (_instance == null) _instance = this;
        else Debug.LogError ("More than one HelperManager");

        if (PlayerPrefs.GetInt ("HelpHint", 0) == 0) {
            PlayerPrefs.SetInt ("HelpHint", 1);
            if (HelpHint != null) HelpHint.SetActive (true);
        }
    }

    public void ActiveHelp ()
    {
        //Statistic.Instance.SendStatistic ("Hint");
        Ads.instance.ShowRewardedVideoHelp ();
    }

    public void ContinueActiveHelp ()
    {
        for (int i = 0; i < ListOfVar.Length; i++) { // Скрываем все маркеры
            if (ListOfVar[i].Var.gameObject.active) { // Активная вариация спавна предметов, по которой проводим подсказку
                for (int j = 0; j < ListOfVar[i].Objects.Length; j++) {
                    ListOfVar[i].Objects[j].Marker.SetActive (false);
                }
                break;
            }
        }

        if (Noise.instance.makedNoise) { // Нашумели, поэтому сообщаем, что нужно вернуться в клетку
            Subtitles.instance.NewAdvice (Settings.instance.GetTranslatedPhrase ("H2"));
            return;
        }
        for (int i = 0; i < ListOfVar.Length; i++) {
            if (ListOfVar[i].Var.gameObject.active) { // Активная вариация спавна предметов, по которой проводим подсказку
                for (int j = 0; j < ListOfVar[i].Objects.Length; j++) {
                    // Определяем тип объекта
                    if (ListOfVar[i].Objects[j].Object.GetComponent<Item> () != null) { // Предмет
                        if (ListOfVar[i].Objects[j].Object.active) { // Активный предмет, необходимо подобрать
                            ListOfVar[i].Objects[j].Marker.SetActive (true);
                            Subtitles.instance.NewAdvice (Settings.instance.GetTranslatedPhrase ("H1"));
                            break;
                        }
                    }
                    else if (ListOfVar[i].Objects[j].Object.GetComponent<Interactive> () != null) { // Интерактивный объект
                        if (!ListOfVar[i].Objects[j].Object.GetComponent<Interactive> ().WasUsedOneTime && ListOfVar[i].Objects[j].Object.active) { // Объект ни разу не использовался и является активным, ведем игрока к нему
                            ListOfVar[i].Objects[j].Marker.SetActive (true);
                            Subtitles.instance.NewAdvice (Settings.instance.GetTranslatedPhrase ("H1"));
                            break;
                        }
                    }
                }
                break;
            }
        }
    }

    public void CheckUsedHelp (GameObject UsedObj)
    {
        for (int i = 0; i < ListOfVar.Length; i++) {
            if (ListOfVar[i].Var.gameObject.active) { // Активная вариация спавна предметов, по которой проводим подсказку
                for (int j = 0; j < ListOfVar[i].Objects.Length; j++) {
                    if (ListOfVar[i].Objects[j].Object == UsedObj) {
                        ListOfVar[i].Objects[j].Marker.SetActive (false);
                        break;
                    }
                }
                break;
            }
        }
    }
}
