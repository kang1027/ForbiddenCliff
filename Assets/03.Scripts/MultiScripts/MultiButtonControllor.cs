using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class MultiButtonControllor : MonoBehaviourPun, IPointerDownHandler, IPointerUpHandler
{
    public static bool isLeftButton;
    public static bool isRightButton;
    public static bool isJumpButton;
    public int isButtonNum; // 0 : Left 1 : Right 2 : Jump 3 : minimap
    private bool isAnimationAnim;
    public void MaximizeMinimap()
    {

        if (isButtonNum == 3)
        {
            if (isAnimationAnim) isAnimationAnim = false;
            else isAnimationAnim = true;

            Animator minimap = GameObject.FindGameObjectWithTag("Minimap").GetComponent<Animator>();
            minimap.SetBool("isAnim", isAnimationAnim);
        }
    }
    bool isOpen;
    public void ScoreToggle()
    {
        if (isOpen)
        {
            GameObject.Find("ScorePanel").GetComponent<Animator>().SetTrigger("ScoreOpen");
            isOpen = false;
        }
        else
        {
            GameObject.Find("ScorePanel").GetComponent<Animator>().SetTrigger("ScoreClose");

            isOpen = true;
        }
    }

    public void MultiRestartGame()
    {
        MultiGameManager.isGameScene = true;
        NextScene.currentScene = 0;
        MultiPlayerControllor.playerHp = 6;
        MultiGameManager.isPlay = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MultiPlayerControllor>().RestartUISetting();
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(73, -68, 0);
    }

    public void MultiMoveScene_Menu()
    {
        MultiGameManager.isGameScene = false;
        MultiAudioManager.isClick = true;
        SceneManager.LoadScene(1);
        PhotonNetwork.Disconnect();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isButtonNum == 0)
            isLeftButton = true;
        else if (isButtonNum == 1)
            isRightButton = true;
        else if (isButtonNum == 2)
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<MultiPlayerControllor>().isJump)
                GameObject.FindGameObjectWithTag("Player").GetComponent<MultiPlayerControllor>().DownJumpingPlayer();
            isJumpButton = true;
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (isButtonNum == 0)
            isLeftButton = false;
        else if (isButtonNum == 1)
            isRightButton = false;
        else if (isButtonNum == 2)
        {
            if(!GameObject.FindGameObjectWithTag("Player").GetComponent<MultiPlayerControllor>().isJump){

                GameObject.FindGameObjectWithTag("Player").GetComponent<MultiPlayerControllor>().JumpingPlayer();
                GameObject.FindGameObjectWithTag("Player").GetComponent<MultiPlayerControllor>().myR.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            isJumpButton = false;
        }
    }
}
