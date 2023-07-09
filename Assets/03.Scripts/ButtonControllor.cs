using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonControllor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isButtonNum == 0)
            isLeftButton = true;
        else if (isButtonNum == 1)
            isRightButton = true;
        else if (isButtonNum == 2)
        {
            if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllor>().isJump)
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllor>().DownJumpingPlayer();
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
            if(!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllor>().isJump){
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllor>().myR.constraints = RigidbodyConstraints2D.FreezeRotation;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllor>().JumpingPlayer();
            }
            isJumpButton = false;
        }
    }
}
