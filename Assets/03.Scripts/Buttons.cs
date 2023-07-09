using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    public GameObject helpText;
    public int NextandBack = 0;
    public int currentPage = 0;
    public GameObject help1;
    public GameObject help2;
    public GameObject help3;
    public GameObject help4;
    public GameObject help5;
    public Text currentPageText;
    private GameObject audioManager;
    private void Start()
    {
        //if (helpText == null)
        //    return;
        //else
        //    helpText.SetActive(false);
        audioManager = GameObject.FindGameObjectWithTag("Menu_AM");


    }
    public void onClickButtonGameStart()
    {        
        GameManager.isGameScene = true;
        GameManager.isMulti = false;
        NextScene.currentScene = 0;
        GameManager.isPlay = true;
        PlayerControllor.playerHp = 6;
        AudioManager.isClick = true;
        SceneManager.LoadScene(1);
        DontDestroyOnLoad(audioManager);
    }
    public void onClickButtonHelp()
    {
        currentPageText.text = "<1/5>";
        helpText.SetActive(true);
        AudioManager.isClick = true;
        help1.SetActive(true);

    }
    public void onClickButtonBack()
    {
        helpText.SetActive(false);
        AudioManager.isClick = true;
        help1.SetActive(false);
        help2.SetActive(false);
        help3.SetActive(false);
        help4.SetActive(false);
        help5.SetActive(false);
    }
    public void RestartGame()
    {
        GameManager.isGameScene = true;
        NextScene.currentScene = 0;
        PlayerControllor.playerHp = 6;
        GameManager.isPlay = true;       
        SceneManager.LoadScene(1);
    }

    public void MoveScene_Menu()
    {
        GameManager.isGameScene = false;
        AudioManager.isClick = true;
        SceneManager.LoadScene(1);
    }
    public void Click()
    {
        AudioManager.isClick = true;
    }
    public void NextBackPage()
    {
        AudioManager.isNextPage = true;

        if (NextandBack == 1 && currentPage == 1)
        {
            help1.SetActive(false);
            help2.SetActive(true);
            TextChange(2);
        }
        else if (NextandBack == 1 && currentPage == 2)
        {
            help2.SetActive(false);
            help3.SetActive(true);

            TextChange(3);

        }
        else if (NextandBack == 1 && currentPage == 3)
        {
            help3.SetActive(false);
            help4.SetActive(true);

            TextChange(4);
        }
        else if (NextandBack == 1 && currentPage == 4)
        {
            help4.SetActive(false);
            help5.SetActive(true);

            TextChange(5);
        }

        //뒤로
        if (NextandBack == 0 && currentPage == 2)
        {
            help2.SetActive(false);
            help1.SetActive(true);
            TextChange(1);
        }
        else if (NextandBack == 0 && currentPage == 3)
        {
            help3.SetActive(false);
            help2.SetActive(true);
            TextChange(2);

        }
        else if(NextandBack == 0 && currentPage == 4)
        {
            help4.SetActive(false);
            help3.SetActive(true);
            TextChange(3);

        }
        else if (NextandBack == 0 && currentPage == 5)
        {
            help5.SetActive(false);
            help4.SetActive(true);
            TextChange(4);

        }

    }
    private void TextChange(int a)
    {

        if (currentPageText == null)
            return;
        else
            currentPageText.text = "<" + a + "/5>";

    }

    
}
