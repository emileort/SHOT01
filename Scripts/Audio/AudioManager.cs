using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] AudioSource sFXPlayer;

    // [SerializeField] float minPitch = 0.9f;

    // [SerializeField] float maxPitch = 1.1f;

    const float MIN_PITCH = 0.9f;
    const float MAX_PITCH = 1.1f;


    // 用於UI穩定播放居多
    public void PlaySFX(AudioData audioData)
    {
        // sFXPlayer.clip = audioClip;
        // sFXPlayer.volume = volume;
        sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
    }

    // 用於連續播放音效居多
    public void PlayRandomSFX(AudioData audioData)
    {
        sFXPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
        PlaySFX(audioData);
    }

    public void PlayRandomSFX(AudioData[] audioData)
    {
        PlayRandomSFX(audioData[Random.Range(0, audioData.Length)]);
    }
}

[System.Serializable] public class AudioData
{
    public AudioClip audioClip;

    public float volume;
}
