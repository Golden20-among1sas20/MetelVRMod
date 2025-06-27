using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] ItemCell[] Cells;
    [SerializeField] HelpText Helper;
    [SerializeField] AudioClip TakeItemClip;
    [SerializeField] AudioClip ChooseItemClip;
    [SerializeField] AudioClip WrongItemClip;

    GameObject[] Database;
    ItemCell ChoosedItem;

    static Inventory _instance;
    public static Inventory instance { get { return _instance; } }

    private void Awake ()
    {
        if (_instance == null) _instance = this;
        else Debug.LogError ("More than one Inventory");

        Database = Resources.LoadAll<GameObject> ("Database");
    }

    private void Update ()
    {
        if (Input.GetKeyDown (KeyCode.Alpha1)) ChooseItem (Cells[0]);
        else if (Input.GetKeyDown (KeyCode.Alpha2)) ChooseItem (Cells[1]);
        else if (Input.GetKeyDown (KeyCode.Alpha3)) ChooseItem (Cells[2]);
        else if (Input.GetKeyDown (KeyCode.Alpha4)) ChooseItem (Cells[3]);
        else if (Input.GetKeyDown (KeyCode.Alpha5)) ChooseItem (Cells[4]);
        else if (Input.GetKeyDown (KeyCode.Alpha6)) ChooseItem (Cells[5]);
        else if (Input.mouseScrollDelta.y != 0f) { // Прокрутка инвентаря колесиком мыши
            if (Input.mouseScrollDelta.y > 0) {
                if (ChoosedItem == null) ChooseItem (Cells[0]);
                else {
                    for (int i = 0; i < Cells.Length; i++) {
                        if (Cells[i] == ChoosedItem) {
                            if (i - 1 < 0) ChooseItem (Cells[Cells.Length - 1]);
                            else ChooseItem (Cells[i - 1]);
                            break;
                        }
                    }
                }
            }
            else {
                if (ChoosedItem == null) ChooseItem (Cells[0]);
                else {
                    for (int i = 0; i < Cells.Length; i++) {
                        if (Cells[i] == ChoosedItem) {
                            if (i + 1 >= Cells.Length) ChooseItem (Cells[0]);
                            else ChooseItem (Cells[i + 1]);
                            break;
                        }
                    }
                }
            }
        }
    }

    public bool TakeItem (int id)
    {
        bool Result = false;

        GameObject NewItem = GetDBItemByID (id);
        if (NewItem == null) {
            Debug.LogError ("Не найден предмет с id: " + id);
            return false;
        }

        for (int i = 0; i < Cells.Length; i++) {
            if (Cells[i].SetItem (NewItem.GetComponent<Item> ())) { // Подняли предмет
                Helper.ShowText (NewItem.GetComponent<Item> ().ItemName);
                PlayerHead.instance.PlaySound (TakeItemClip);
                Result = true;
                break;
            }
        }

        return Result;
    }

    public void ChooseItem (ItemCell NewChoosedItem)
    {
        ChoosedItem = NewChoosedItem;
        foreach (ItemCell Cell in Cells) {
            if (Cell == ChoosedItem) {
                if (!ChoosedItem.IsFree) Helper.ShowText (GetDBItemByID (ChoosedItem.ID).GetComponent<Item> ().ItemName);
                Cell.ChooseItem (true);
                PlayerHead.instance.PlaySound (ChooseItemClip);
            }
            else Cell.ChooseItem (false);
        }
    }

    public GameObject GetDBItemByID (int id)
    {
        for (int i = 0; i < Database.Length; i++) {
            if (Database[i].GetComponent<Item> ().ID == id) return Database[i];
        }

        return null;
    }

    public bool UseRequiereItem (int id, string OnWrongUse = "", AudioSource FailedAudio = null)
    {
        if (ChoosedItem == null) {
            if (FailedAudio != null) {
                FailedAudio.Play ();
            }
            else PlayerHead.instance.PlaySound (WrongItemClip);
            Helper.ShowText (OnWrongUse);
            return false;
        }
        if (!ChoosedItem.IsFree && ChoosedItem.ID == id) {
            ChoosedItem.UseItem ();
            return true;
        }

        if (FailedAudio != null) {
            FailedAudio.Play ();
        }
        else PlayerHead.instance.PlaySound (WrongItemClip);
        Helper.ShowText (OnWrongUse);
        return false;
    }

    public void RemoveChoosedItem ()
    {
        if (!ChoosedItem.IsFree) {
            ChoosedItem.FreePlace ();
        }
    }

    public void ShowText (string Message)
    {
        Helper.ShowText (Message);
    }

    public void DropItem (int id)
    {
        for (int i = 0; i < Cells.Length; i++) {
            if (Cells[i].ID == id && !Cells[i].IsFree) {
                Cells[i].FreePlace ();
            }
        }
    }

    public bool HaveItem (int id)
    {
        for (int i = 0; i < Cells.Length; i++) {
            if (Cells[i].ID == id && !Cells[i].IsFree) {
                return true;
            }
        }
        return false;
    }
}
