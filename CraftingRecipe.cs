using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="CraftingRecipe")]
public class CraftingRecipe : ScriptableObject
{
    [SerializeField] Requirement[] requirements;
    public Requirement[] Requirements { get { return requirements; } }
    [SerializeField] Item item;
    public Item Item_ { get { return item; } }

    [SerializeField] byte count;
    public byte Count { get { return count; } }
}
[System.Serializable]
public class Requirement
{
    public Item item;
    public int count;
}