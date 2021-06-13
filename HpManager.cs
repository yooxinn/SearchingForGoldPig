using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HpManager : MonoBehaviour
{
    public int hp;
    public Image[] heart;
    public Sprite f, h, n;
    FirstPersonController fpc;
    public Animator cam;
    public bool inAir = false;
    public float airY;
    public float cactusCoolTime;
    public bool cactusHurtable = true;
    public List<GameObject> collidingCactuses = new List<GameObject>();
    public bool died = false;
    public GameObject dieUI;
    private void Awake()
    {
        inAir = false;
        fpc = GetComponent<FirstPersonController>();

        UpdateHeart();
    }

    private void Update()
    {
        if (!fpc.isGrounded)
        {
            if (!inAir)
            {
                airY = transform.position.y;
                inAir = true;
            }
            airY = transform.position.y > airY ? transform.position.y : airY;
        }
        else if (fpc.isGrounded)
        {
            if (inAir)
            {
                inAir = false;
                int fallH = Mathf.FloorToInt(airY - transform.position.y);
                if (fallH >= 4)
                {
                    Damage(fallH - 3);
                }
            }
        }

        if(collidingCactuses.Count > 0 && cactusHurtable)
        {
            cactusHurtable = false;
            Invoke(nameof(SetCactusHurt), 0.5f);
            Damage(1);
        }
    }
    public void UpdateHeart()
    {
        for(int i = 0; i < 10; i++)
        {
            heart[i].sprite = hp * 0.5f >= i + 1? f : (hp * 0.5f > i ? h : n);
        }
    }
    public void Damage(int dam)
    {
        for(int i = 0; i < collidingCactuses.Count; i++)
        {
            float dis = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(collidingCactuses[i].transform.position.x, collidingCactuses[i].transform.position.z));
            if (dis > 1)
                collidingCactuses.RemoveAt(i);
        }

        airY = transform.position.y;
        hp -= dam;
        UpdateHeart();
        cam.SetTrigger("Shake");
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        if(hp <= 0 && !died)
        {
            died = true;
            StartCoroutine(Die());
        }
    }
    public IEnumerator Die()
    {
        dieUI.SetActive(true);
        fpc.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSeconds(2);
        Time.timeScale = 0;
    }
    public void CactusEnter(GameObject c)
    {
        if (!collidingCactuses.Contains(c))
        { 
            collidingCactuses.Add(c);
        }
    }

    public void CactusExit(GameObject c)
    {
        collidingCactuses.Remove(c);
    }
    public void SetCactusHurt() => cactusHurtable = true;
}
