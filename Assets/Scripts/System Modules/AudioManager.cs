using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    [Header("聲音開關")]
    [SerializeField] Toggle AudioToggle;
    [Header("音效播放器")]
    [SerializeField] AudioSource sFXPlayer;

    //音量
    float SFX_Volume = 1f;

    public void PlaySFX(AudioData audioDat)
    {
        if (!AudioToggle.isOn) return;
        sFXPlayer.pitch = 1;
        sFXPlayer.PlayOneShot(audioDat.audioClip, audioDat.volume * SFX_Volume);
    }

    //隨機音量組數
    public void PlayRandomSFX(AudioData[] audioData)
    {
        PlaySFX(audioData[Random.Range(0, audioData.Length)]);
    }
}

//儲存音頻資料
[System.Serializable]
public class AudioData
{
    public AudioClip audioClip;

    public float volume;
}
