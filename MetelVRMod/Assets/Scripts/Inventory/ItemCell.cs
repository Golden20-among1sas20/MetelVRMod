using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour
{
    [SerializeField] Image Icon;
    [SerializeField] Image Border;

    public bool IsFree { get; private set;} = true;
    public int ID { get; private set;} = -1;
    public uint QuantityUse { get; private set;} = 1;

    public bool SetItem (Item NewItem)
    {
        bool Result = false;

        if (IsFree) {
            ID = NewItem.ID;
            IsFree = false;
            Icon.sprite = NewItem.Icon;
            Icon.gameObject.SetActive (true);
            QuantityUse = NewItem.QuantityUse;
            Result = true;
        }

        return Result;
    }

    public void ChooseItem (bool Flag)
    {
        Border.color = (Flag) ? new Color32 (255, 0, 0, 78) : new Color32 (255, 255, 255, 36);
    }

    public void UseItem ()
    {
        QuantityUse--;
        if (QuantityUse == 0) {
            IsFree = true;
            ID = -1;
            Icon.gameObject.SetActive (false);
        }
    }

    public void FreePlace ()
    {
        IsFree = true;
        ID = -1;
        Icon.gameObject.SetActive (false);
    }
}
