using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    public GameObject OptionUI;
    public GameObject TutorialUI;
    public TMP_Text Screentxt;


    int ScreenNum = 0;
    public bool isoption;
    public bool isTutorial;


    void Start()
    {
        isoption = false;
        isTutorial = false;
    }

    void Update()
    {
        OptionUI.SetActive(isoption);
        TutorialUI.SetActive(isTutorial);
    }

    public void ClickOption()
    {
        isoption = !isoption;
    }
    public void ClickTutorial()
    {
        isTutorial = !isTutorial;
    }

    public void ClickScreen()
    {
        ScreenNum++;
        ScreenNum %= 2;
        switch (ScreenNum)
        {
            case 0:
                Screentxt.text = "��ü ȭ��";
                Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
                break;
            case 1:
                Screentxt.text = "â���";
                Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
                break;
        }
    }
    public void ExitBtn()
    {
            Application.Quit();
    }


}
