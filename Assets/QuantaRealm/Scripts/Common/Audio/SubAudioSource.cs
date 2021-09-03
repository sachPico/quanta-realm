using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubAudioSource : MonoBehaviour
{
    public AudioClip[] subAudioClips;

    AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(int subAudioClipIndex)
    {
        audioSource.clip = subAudioClips[subAudioClipIndex];
        audioSource.Stop();
        audioSource.Play();
    }
}
