using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public int ID;
    [SerializeField] public Sprite Icon;
    [SerializeField] public string ItemName = "Unknown";
    [SerializeField] public uint QuantityUse = 1;

    public bool TakeItem ()
    {
        if (Inventory.instance.TakeItem (ID)) {
            gameObject.SetActive (false);
            return true;
        }
        return false;
    }
}
