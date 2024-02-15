using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI : MonoBehaviourPunCallbacks
{
    Camera MainCamera;
    PlayerController playerController;
    [Header("시작 카운트")]
    public TMP_Text Count;
    public Image hasWeapon;
    public GameObject HpBar;
    public GameObject DamagedUI;

    public bool isExit = false;
    public GameObject ExitUI;

    void Start()
    {
        MainCamera = Camera.main;
    }

    void Update()
    {

        CountUI();

        bool DidFind = false;

        foreach (PlayerController pc in FindObjectsOfType<PlayerController>())
        { 
            if(PhotonView.Get(pc).IsMine)
            {
                playerController = pc;
                DidFind = true;
                break;
            }
        }
        if (DidFind == false)
        {
            for (int i = 10; i > 0; i--)
            {
                HpBar.transform.GetChild(i - 1).gameObject.SetActive(false);
            }
            return;
        }
        if (playerController.HP == 1 || playerController.isDamaged)
        {
            DamagedUI.SetActive(true);
        }
        else if(!playerController.isDamaged)
        {
            DamagedUI.SetActive(false);
        }
        WeaponUI();
        HpFIll();
        GetExit();
        ExitUI.SetActive(isExit);
    }
    public void Exit ()
    {
        Application.Quit();
    }
    public void CountUI()
    {
        
        Count.text = "Stage" + "\n" + ((int)MainCamera.GetComponent<CameraMove>().StartTime);
        if(((int)MainCamera.GetComponent<CameraMove>().StartTime) <=0)
        {
            Count.transform.parent.gameObject.SetActive(false);
        }
    }
    public void GetExit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isExit = !isExit;
        }
    }
    public void WeaponUI()
    {
        hasWeapon.sprite = playerController.HandObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite;
    }
    public void HpFIll()
    {
        if (playerController.photonView.IsMine)
        {
            if(playerController.HP == 10)
                for (int i = 10; i > 0; i--)
                {
                    HpBar.transform.GetChild(i - 1).gameObject.SetActive(true);
                }


            if (playerController.HP != 0)
            {
                for (int i = 10; i >playerController.HP; i--)
                {
                    HpBar.transform.GetChild(i-1).gameObject.SetActive(false);
                }
            }
        }
    }


}
