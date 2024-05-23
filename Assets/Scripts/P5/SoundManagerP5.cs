using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerP5 : MonoBehaviour
{
    public static SoundManagerP5 instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(int index)
    {
        if (index >= 0 && index < bgmClips.Length)
        {
            bgmSource.clip = bgmClips[index];
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("BGM index out of range");
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(int index)
    {
        if (index >= 0 && index < sfxClips.Length)
        {
            sfxSource.PlayOneShot(sfxClips[index]);
        }
        else
        {
            Debug.LogWarning("SFX index out of range");
        }
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void PlayButtonSound()
    {
        PlaySFX(2);
    }

    public void PlayShieldSound()
    {
        PlaySFX(3);
    }

    public void PlaySwordSound()
    {
        PlaySFX(4);
    }

}
