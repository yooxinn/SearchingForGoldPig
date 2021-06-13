using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUse : MonoBehaviour
{
    Inventory inv;
    FirstPersonController fpc;
    HpManager hm;
    public int curHotSlot;
    public Image[] itemImages;
    public Text[] itemCounts;
    public GameObject hotSlot;
    public GameObject hand;
    Animator han;
    public bool itemUseable;
    public float itemCoolTime;

    public float maxDistance;
    public LayerMask worldLayer, treeLayer, leavesLayer, placeLayer, animalLayer;
    public GameObject woodBlock, woodenPlank;
    private void Awake()
    {
        inv = GetComponent<Inventory>();
        fpc = GetComponent<FirstPersonController>();
        han = hand.GetComponent<Animator>();
        hm = GetComponent<HpManager>();

        UpdateHand();
    }

    public void Update()
    {
        UpdateSlot();
        if (Time.timeScale == 0 || inv.inE)
            return;

        han.SetBool("walking", fpc.isWalking);
        han.SetFloat("walkSpeed", fpc.isSprinting ? 2 : 1);
        SetHotSlot();
        UseItem();
    }
    void SetHotSlot()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            curHotSlot = 1;
            UpdateHand();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            curHotSlot = 2;
            UpdateHand();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            curHotSlot = 3;
            UpdateHand();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            curHotSlot = 4;
            UpdateHand();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            curHotSlot = 5;
            UpdateHand();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            curHotSlot = 6;
            UpdateHand();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            curHotSlot = 7;
            UpdateHand();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            curHotSlot = 8;
            UpdateHand();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            curHotSlot = 9;
            UpdateHand();
        }

        hotSlot.transform.position = itemImages[curHotSlot - 1].transform.position;
    }

    void UpdateSlot()
    {
        for(int i = 0; i < 9; i++)
        {
            if(inv.inventory[i].count == 0)
            {
                itemImages[i].color = new Color(1, 1, 1, 0);
            }
            else
            {
                itemImages[i].color = new Color(1, 1, 1, 1);
                itemImages[i].sprite = inv.inventory[i].item.Icon;
            }
            itemCounts[i].text = inv.inventory[i].count <= 1 ? null : inv.inventory[i].count.ToString();
        }
    }
    public void UpdateHand()
    {
        if (hand.transform.childCount == 1)
            Destroy(hand.transform.GetChild(0).gameObject);

        if (inv.inventory[curHotSlot - 1].count == 0)
            return;

        GameObject g = Instantiate(inv.inventory[curHotSlot - 1].item.Model);
        g.transform.SetParent(hand.transform);
        g.transform.localEulerAngles = Vector3.zero;
        g.transform.localPosition = Vector3.zero;
    }
    void Reload()
    {
        itemUseable = true;
    }
    void UseItem()
    {
        if (!itemUseable)
            return;


        Item curItem = inv.inventory[curHotSlot - 1].item;
        if (inv.inventory[curHotSlot - 1].count == 0)
            return;

        if (Input.GetMouseButton(0))
        {
            itemUseable = false;
            Invoke(nameof(Reload), itemCoolTime);
            han.SetTrigger("use");
            switch (curItem.name)
            {
                case "WoodenAxe":

                    RaycastHit hit_wa;
                    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit_wa, maxDistance, treeLayer))
                    {
                        GameObject t = hit_wa.collider.gameObject;
                        if (t.GetComponent<Rigidbody>() != null)
                            return;
                        t.transform.position += Vector3.up * 1;
                        Rigidbody rb = t.AddComponent<Rigidbody>();
                        Destroy(t.transform.Find("Leave").GetComponent<MeshCollider>());
                        t.GetComponent<Tree>().Spawn();
                    }
                    else if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit_wa, maxDistance, animalLayer))
                    {
                        GameObject p = hit_wa.collider.gameObject;
                        p.GetComponent<Animator>().SetTrigger("die");
                        p.GetComponent<Pig>().StopAllCoroutines();
                        p.GetComponent<Pig>().dir = Vector3.zero;
                        Destroy(p, 0.5f);
                        inv.EarnItem(p.GetComponent<Pig>().samgyupsal, null, 1);
                    }

                    break;
                case "StoneShovel":
                    Debug.Log("use");
                    RaycastHit hit_ss;
                    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit_ss, maxDistance, worldLayer))
                    {



                        Vector2 usedPos = new Vector2(Mathf.FloorToInt(hit_ss.point.x), Mathf.FloorToInt(hit_ss.point.z));

                        {

                            GameObject t = hit_ss.collider.gameObject;
                            MeshFilter mf = t.GetComponent<MeshFilter>();
                            Mesh m = mf.mesh;

                            foreach (Vector3 v in m.vertices)
                            {
                                if (new Vector2(v.x, v.z) == usedPos)
                                {
                                    float y = Mathf.Floor(v.y) - 1;
                                    List<Vector3> list = new List<Vector3>(m.vertices);
                                    for (int i = 0; i < list.Count; i++)
                                    {
                                        Vector2 c = new Vector2(list[i].x, list[i].z);
                                        if (c == usedPos)
                                        {
                                            list[i] = new Vector3(usedPos.x, y, usedPos.y);
                                            m.vertices = list.ToArray();
                                        }
                                    }
                                }
                                else if (new Vector2(v.x, v.z) == usedPos + Vector2.up)
                                {
                                    float y = Mathf.Floor(v.y) - 1;
                                    List<Vector3> list = new List<Vector3>(m.vertices);
                                    for (int i = 0; i < list.Count; i++)
                                    {
                                        Vector2 c = new Vector2(list[i].x, list[i].z);
                                        if (c == usedPos + Vector2.up)
                                        {
                                            list[i] = new Vector3(usedPos.x, y, usedPos.y + 1);
                                            m.vertices = list.ToArray();
                                        }
                                    }
                                }
                                else if (new Vector2(v.x, v.z) == usedPos + Vector2.right)
                                {
                                    float y = Mathf.Floor(v.y) - 1;
                                    List<Vector3> list = new List<Vector3>(m.vertices);
                                    for (int i = 0; i < list.Count; i++)
                                    {
                                        Vector2 c = new Vector2(list[i].x, list[i].z);
                                        if (c == usedPos + Vector2.right)
                                        {
                                            list[i] = new Vector3(usedPos.x + 1, y, usedPos.y);
                                            m.vertices = list.ToArray();
                                        }
                                    }
                                }
                                else if (new Vector2(v.x, v.z) == usedPos + Vector2.right + Vector2.up)
                                {
                                    float y = Mathf.Floor(v.y) - 1;
                                    List<Vector3> list = new List<Vector3>(m.vertices);
                                    for (int i = 0; i < list.Count; i++)
                                    {
                                        Vector2 c = new Vector2(list[i].x, list[i].z);
                                        if (c == usedPos + Vector2.right + Vector2.up)
                                        {
                                            list[i] = new Vector3(usedPos.x + 1, y, usedPos.y + 1);
                                            m.vertices = list.ToArray();
                                        }
                                    }
                                }
                            }
                            m.RecalculateNormals();
                        }
                    }
                    Debug.Log("Didin't use");
                    break;
            }
        }
        else if (Input.GetMouseButton(1))
        {
            itemUseable = false;
            Invoke(nameof(Reload), itemCoolTime);
            han.SetTrigger("use");

            switch (curItem.name)
            {
                case "Wood":
                    PlaceBlock(woodBlock);
                    break;
                case "WoodenPlank":
                    PlaceBlock(woodenPlank);
                    break;
                case "Samgyupsal":
                    Debug.Log("Eat");
                    hm.hp += 6;
                    hm.UpdateHeart();
                    if (hm.hp > 20)
                        hm.hp = 20;
                    inv.inventory[curHotSlot - 1].count--;
                    break;
            }
        }
        if (inv.inventory[curHotSlot - 1].count == 0)
            UpdateHand();
    }
    public void PlaceBlock(GameObject b)
    {

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, placeLayer))
        {
            GameObject g = Instantiate(b);
            g.transform.position = hit.point - (hit.point - Camera.main.transform.position).normalized * 0.5f;
            inv.inventory[curHotSlot - 1].count--;
        }
    }
}
