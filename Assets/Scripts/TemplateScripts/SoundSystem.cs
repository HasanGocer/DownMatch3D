using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoSingleton<SoundSystem>
{
    [SerializeField] private AudioSource mainSource;
    [SerializeField] private AudioClip mainMusic, star, finish, bomb;

    public void MainMusicPlay()
    {
        mainSource.clip = mainMusic;
        mainSource.Play();
        mainSource.volume = 70;
    }

    public void MainMusicStop()
    {
        mainSource.Stop();
        mainSource.volume = 0;
    }

    /* public void CallStarSound()
     {
         mainSource.PlayOneShot(star);
     }
     public void CallFinishSound()
     {
         mainSource.PlayOneShot(finish);
     }
     public void CallBombSound()
     {
         mainSource.PlayOneShot(bomb);
     }*/
}
