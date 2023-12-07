using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource spiderHit;
    [SerializeField] private AudioSource fungusHit;
    [SerializeField] private AudioSource treeHit;
    [SerializeField] private AudioSource destructibleHit;
    [SerializeField] private AudioSource shipSource;
    [SerializeField] private AudioSource startSource;
    [SerializeField] private AudioSource mainMenuSource;
    [SerializeField] private AudioSource roomTransitionSource;
    [SerializeField] private AudioSource dungeonSource;
    [SerializeField] private AudioClip doorClip;
    [SerializeField] private AudioClip menuClip;

    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySpiderSound(AudioClip clip)
    {
        spiderHit.PlayOneShot(clip);
    }

    public void PlayDestructibleSound(AudioClip clip)
    {
        destructibleHit.PlayOneShot(clip);
    }

    public void PlayFungusSound(AudioClip clip)
    {
        fungusHit.PlayOneShot(clip);
    }

    public void PlayTreeSound(AudioClip clip)
    {
        treeHit.PlayOneShot(clip);
    }

    public void PlayDoorSound(AudioClip clip)
    {
        roomTransitionSource.PlayOneShot(doorClip);
    }

    public void PlayMenuMusic(AudioClip clip)
    {
        //mainMenuSource.PlayOneShot(clip);
        mainMenuSource.Play();
        mainMenuSource.playOnAwake = true;
    }

    public void StopMenu()
    {
        mainMenuSource.Stop();
    }
}
