using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class cow : MonoBehaviour
{
    public AudioClip humanMoo;
    public AudioClip CowMoo;

    public AudioSource audioSource;

    public void Start()
    {
        StartCoroutine(SoundMoo());
    }

    // Update is called once per frame
    public void PlaySound()
    {
        var rand = Random.Range(0, 2);
        Debug.Log(rand);
        if(rand == 0)
        {
            audioSource.PlayOneShot(humanMoo);
        }
        else
        {
            audioSource.PlayOneShot(CowMoo);
        }
    }

    IEnumerator SoundMoo()
    {
        while(true)
        {
            PlaySound();
            yield return new WaitForSeconds(20.0f);
        }
    }
}
