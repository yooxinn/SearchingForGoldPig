using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseUI;
    Inventory i;
    HpManager hm;
    public GameManager gm;
    public bool paused;
    private void Awake()
    {
        i = GetComponent<Inventory>();
        hm = GetComponent<HpManager>();
    }

    private void Update()
    {
        if (hm.died)
            return;


        pauseUI.SetActive(paused);
        Time.timeScale = paused ? 0 : 1;
        Cursor.lockState = (paused || i.inE) ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused || i.inE;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }
    }
}
