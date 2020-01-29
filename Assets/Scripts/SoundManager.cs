using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public static SoundManager instance;

    public AudioClip audioClip_Click_btn;
    public AudioClip audioClip_Back_Btn;
    public AudioClip audioClip_win;
    public AudioClip audioClip_lose;
    public AudioClip audioClip_basecamp;
    public AudioClip audioClip_sendMSG;
    public AudioClip audioClip_swordSound;
    public AudioClip audioClip_tokenSound;
    public AudioClip audioClip_ArrowSound;

    void Awake()
    {
        if (SoundManager.instance == null)
            SoundManager.instance = this;
    }

    public void PlayArrowSound()
    {audioSource.PlayOneShot(audioClip_ArrowSound);}

    public void ClickBtnSound()
    {audioSource.PlayOneShot(audioClip_Click_btn); }

    public void BackBtnSound()
    { audioSource.PlayOneShot(audioClip_Back_Btn); }

    public void PlayWinSound()
    { audioSource.PlayOneShot(audioClip_win); }

    public void PlayLoseSound()
    {audioSource.PlayOneShot(audioClip_lose); }

    public void PlayBasecampSound()
    { audioSource.PlayOneShot(audioClip_basecamp); }

    public void PlaysendMSGSound()
    { audioSource.PlayOneShot(audioClip_sendMSG);  }

    public void PlayswordSound()
    {audioSource.PlayOneShot(audioClip_swordSound); }

    public void PlaytokenSound()
    {audioSource.PlayOneShot(audioClip_tokenSound); }
    
}
