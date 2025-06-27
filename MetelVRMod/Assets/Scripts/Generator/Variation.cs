using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variation : MonoBehaviour
{
    [System.Serializable]
    class SetParent
    {
        public Transform Item;
        public Transform Parent;
    }

    [SerializeField] List<SetParent> ForSetParent;

    private void Start ()
    {
        foreach (SetParent ForSetParentTemp in ForSetParent) {
            ForSetParentTemp.Item.parent = ForSetParentTemp.Parent;
        }
    }
}
