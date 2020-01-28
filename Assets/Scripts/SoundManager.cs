using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource musicPlayer;
    public AudioClip btnClick;
    public AudioClip prefab_boom;

    public static void buttonClick(AudioClip btnClick, AudioSource audioPlayer)
    {
        audioPlayer.Stop();
        audioPlayer.clip = btnClick;
        audioPlayer.loop = false;
        audioPlayer.time = 0;
        audioPlayer.Play();
        
    }
}
