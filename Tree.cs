using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public Item wood;
    public Transform[] ItemSpawnPos;

    public float itemSpawnTime;
    public GameObject woodItem;

    public void Spawn()
    {
        Invoke(nameof(Spawn_), itemSpawnTime);
    }
    void Spawn_()
    {
        foreach (Transform t in ItemSpawnPos)
        {
            GameObject g = Instantiate(woodItem, t.position, transform.rotation);
            g.name = "Wood";
            g.GetComponent<ItemInfo>().item = wood;
        }
        Destroy(gameObject);
    }
}
