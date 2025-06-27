using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] public AudioSource AudioSrc;
    [SerializeField] AudioClip DefaultClip;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider EffectsSlider;
    [SerializeField] AudioMixer Mixer;

    static MusicManager _instance;
    public static MusicManager instance { get { return _instance; } }

    private void Awake ()
    {
        if (_instance == null) _instance = this;
        else Debug.LogError ("More than one MusicManager");

        if (AudioSrc == null) AudioSrc = GetComponent<AudioSource> ();
    }

    private void Start ()
    {
        PlayDefault ();
        Mixer.SetFloat ("Music", Mathf.Log10 (PlayerPrefs.GetFloat ("Music", 1f)) * 20);
        MusicSlider.value = PlayerPrefs.GetFloat ("Music", 1f);
        float EffectsVolume = PlayerPrefs.GetFloat ("Effects", 1f);
        EffectsSlider.value = EffectsVolume;
        Mixer.SetFloat ("Effects", Mathf.Log10 (EffectsVolume) * 20);
    }

    public void NewMusic (AudioClip NewClip)
    {
        if (AudioSrc.clip == NewClip && AudioSrc.isPlaying) return;

        AudioSrc.clip = NewClip;
        AudioSrc.Play ();
        AudioSrc.loop = false;
    }

    public void Silence ()
    {
        AudioSrc.Stop ();
    }

    public void Continue ()
    {
        AudioSrc.Play ();
    }

    public void PlayDefault ()
    {
        AudioSrc.clip = DefaultClip;
        AudioSrc.Play ();
        AudioSrc.loop = true;
    }

    public void GamePaused ()
    {
        if (AudioSrc.clip != DefaultClip) {
            AudioSrc.Pause ();
        }
    }

    public void GameUnpaused ()
    {
        if (AudioSrc.clip != DefaultClip) {
            AudioSrc.UnPause ();
        }
    }

    public void ChangeVolume (float NewVolume)
    {
        PlayerPrefs.SetFloat ("Music", NewVolume);
        Mixer.SetFloat ("Music", Mathf.Log10 (NewVolume) * 20);
    }

    public void ChangeEffectsVolume (float NewVolume)
    {
        PlayerPrefs.SetFloat ("Effects", NewVolume);
        Mixer.SetFloat ("Effects", Mathf.Log10 (NewVolume) * 20);
    }
}
