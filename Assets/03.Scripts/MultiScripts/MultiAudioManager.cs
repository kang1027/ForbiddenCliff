using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class MultiAudioManager : MonoBehaviourPun
{
    AudioSource audioSource;
    private bool isClip;
    private float currentTime;
    public AudioClip clickSounds;
    public AudioClip nextPageSounds;
    public static bool isClick;
    public static bool isNextPage;
    public bool isMine;
    public bool isJump;
    // Start is callyed before the first frame update
    
    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
    }
    public void SelectMine()
    {
        if (photonView.IsMine)
        {
            gameObject.name = "isMineAudioManager";
            isMine = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isGameScene)
        {
            if (isClick)
            {
                PlayAudio(clickSounds);
                isClick = false;
            }
            if (isNextPage)
            {
                PlayAudio(nextPageSounds, 0.25f);
                isNextPage = false;
            }
        }
    }

    public void PlayAudio(AudioClip audioclip, float delay = 0, float volumn = 0.6f)
    {
        if (!photonView.IsMine)
            return;

        audioSource.volume = volumn;
        if (!isJump)
            audioSource.Stop();
        if (Time.time > currentTime + delay)
        {
            currentTime = Time.time;
            audioSource.PlayOneShot(audioclip);
            if (audioclip.name == "Chaging")
                isJump = false;
        }
    }
}
