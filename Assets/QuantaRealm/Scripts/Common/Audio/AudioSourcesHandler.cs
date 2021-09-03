using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType {BGM, UI, ENV_SFX, MAIN_SFX, SUB_SFX, AOE_SFX, ITEM_SFX, ENEMY_SFX, BOSS_SFX}

public class AudioSourcesHandler : MonoBehaviour
{
    public static AudioSourcesHandler instance;

    public SubAudioSource[] subAudioSources;

    public void Start()
    {
        instance = this;
    }

    public static void PlaySFX(int subAudioSourceIndex, int subAudioClipIndex)
    {
        instance.subAudioSources[subAudioSourceIndex].Play(subAudioClipIndex);
    }
}
