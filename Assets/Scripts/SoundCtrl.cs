using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundCtrl : MonoBehaviour {

    private static AudioSource audioSource;
    public static Action OnPlaySound;
    public static Action OnEndSound;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        OnPlaySound += () => { StartCoroutine(CheckEnd(onEnd: OnEndSound)); };
    }

    public static void PlayOneShot(AudioClip audioClip,Action onEnd=null)
    {
        audioSource.PlayOneShot(audioClip);
        OnPlaySound?.Invoke();
    }

    private static IEnumerator CheckEnd(Action onEnd)
    {
        var frameWait = new WaitForEndOfFrame();
        while(true)
        {
            yield return frameWait;
            if(audioSource.isPlaying == false)
            {
                onEnd?.Invoke();
                break;
            }
        }
    }

}
