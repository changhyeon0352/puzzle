using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : Singleton<SoundPlayer>
{
    AudioSource audioSource;

    override protected void Awake()
    {
        base.Awake();
        audioSource=GetComponent<AudioSource>();
    }

    public void PlayPieceSound()
    {
        if(GameManager.Instance.isSoundOn)
            audioSource.Play();
    }
}
