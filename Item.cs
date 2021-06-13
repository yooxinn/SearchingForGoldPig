using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item")]
public class Item : ScriptableObject
{
    [SerializeField] Sprite icon;
    public Sprite Icon { get { return icon; } }

    [SerializeField] GameObject model;
    public GameObject Model { get { return model; } }
}
