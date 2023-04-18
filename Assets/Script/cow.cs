using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cow : MonoBehaviour
{
    public AudioClip humanMoo;
    public AudioClip CowMoo;

    public AudioSource audioSource;

    // Update is called once per frame
    public void Playsound()
    {
        var rand = Random.Range(0, 1);
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
        Color c = GetComponent<Renderer>().material.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            GetComponent<Renderer>().material.color = c;
            yield return null;
        }
    }
}
