using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
    public GameManager gm;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        gm.startAudio = false;
    }
}
