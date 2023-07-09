using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private Animator stoneAnim;
    private bool isStop;
    public static bool isStart;
    public int rightOrLeft=0;
    // Update is called once per frame
    void Start(){
        stoneAnim = GetComponent<Animator>();
        stoneAnim.SetBool("IsBreak", false);
        isStop = true;
    }
    void Update()
    {

            if(rightOrLeft==1 && !isStop)
            {
                transform.Rotate(0, 0, -8f);
                transform.position += new Vector3(2f, 0, 0);
            }
            else if(rightOrLeft==0 && !isStop)
            {
            transform.Rotate(0, 0, 8f);
                transform.position += new Vector3(-2, 0, 0);
            }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isStart = true;
        isStop = false;
        if (collision.gameObject.tag == "DestroyStonePos")
        {
            isStop = true;
            stoneAnim.SetBool("IsBreak",true);
            Destroy(gameObject, 0.4f);
        }
    }
}

