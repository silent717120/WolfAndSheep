using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    [Header("�n���}��")]
    [SerializeField] Toggle AudioToggle;
    [Header("���ļ���")]
    [SerializeField] AudioSource sFXPlayer;

    //���q
    float SFX_Volume = 1f;

    public void PlaySFX(AudioData audioDat)
    {
        if (!AudioToggle.isOn) return;
        sFXPlayer.pitch = 1;
        sFXPlayer.PlayOneShot(audioDat.audioClip, audioDat.volume * SFX_Volume);
    }

    //�H�����q�ռ�
    public void PlayRandomSFX(AudioData[] audioData)
    {
        PlaySFX(audioData[Random.Range(0, audioData.Length)]);
    }
}

//�x�s���W���
[System.Serializable]
public class AudioData
{
    public AudioClip audioClip;

    public float volume;
}
