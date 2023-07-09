using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiChangeBGM : MonoBehaviour
{
    public MultiGameManager gm;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<MultiGameManager>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        gm.startAudio = false;
    }
}
