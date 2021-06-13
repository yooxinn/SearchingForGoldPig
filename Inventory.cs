using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    ItemUse iu;
    FirstPersonController fpc;
    Rigidbody rb;
    public Slot[] inventory;
    public GameObject crafting;
    public bool inE;
    void Awake()
    {
        iu = GetComponent<ItemUse>();
        fpc = GetComponent<FirstPersonController>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {

        if (Time.timeScale == 0)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            inE = !inE;
        }
        crafting.SetActive(inE);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            Item i = collision.gameObject.GetComponent<ItemInfo>().item;
            EarnItem(i, collision.gameObject, 1);
        }
    }
    public void Craft(CraftingRecipe c)//(CraftingRecipe c)
    {
        foreach (Requirement r in c.Requirements)
        {
            Item i = r.item;
            int countInInv = 0;
            foreach(Slot s in inventory)
            {
                if (s.item == i)
                    countInInv += s.count;
            }
            if (countInInv < r.count)
                return;
        }
        foreach (Requirement r in c.Requirements)
        {
            int needed = r.count;
            int cur = 0;
            foreach (Slot s in inventory)
            {
                if (s.item == r.item)
                {
                    if(s.count >= needed - cur)
                    {
                        s.count -= (byte)(needed - cur);
                        EarnItem(c.Item_, null, c.Count);
                        return;
                    }
                    else
                    {
                        cur += s.count;
                        s.count = 0;
                    }
                }
            }
        }
    }
    public void EarnItem(Item item, GameObject col, int count)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].item == item && inventory[i].count <= 64 - count)
            {
                inventory[i].count += (byte)count;
                if (col != null)
                    Destroy(col);
                return;
            }
        }

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].count == 0)
            {
                inventory[i].item = item;
                inventory[i].count += (byte)count;
                iu.UpdateHand();
                if (col != null)
                    Destroy(col);
                return;
            }
        }
    }

}
[System.Serializable]
public class Slot
{
    public Item item;
    public byte count;
}


