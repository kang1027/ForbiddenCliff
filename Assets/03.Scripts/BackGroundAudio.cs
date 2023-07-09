using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundAudio : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip audioclip, float volumn = 0.6f)
    {
        audioSource.volume = volumn;
        if (audioSource.isPlaying)
            audioSource.Stop();
        audioSource.clip = audioclip;
        audioSource.Play();

    }
}
