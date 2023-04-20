using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource SFXSource;
    public AudioSource MusicSource;
    [SerializeField]
    private AudioClip BGM;

    public void Start()
    {
        PlayMusic(BGM);
    }

    public void PlaySound(AudioClip sound)
    {
        SFXSource.PlayOneShot(sound);
    }

    public void PlayMusic(AudioClip music)
    {
        MusicSource.PlayOneShot(music);
    }
}
