using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    private void Awake()
    {
        Hide();
    }
    public AudioMixer mixer;
    public void SetVolume(float volume)
    {
        mixer.SetFloat("Volume", volume);
        Debug.Log(volume);

    }

    public void FullScreen(bool toggleFullSrceen)
    {
        Screen.fullScreen = toggleFullSrceen;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
