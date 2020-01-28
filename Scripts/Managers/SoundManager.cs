using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager> {

    [SerializeField] private AudioSource musicSource, sfxSource;
    [SerializeField] private Slider musicSlider, sfxSlider;

    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

	// Use this for initialization
	void Start ()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio") as AudioClip[];

        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);    // every audio clip is stored in the audioClips dictionary with its name paired with its value
        }

        LoadVolume();

        // must delegate functions that are passed in as events to listeners
        musicSlider.onValueChanged.AddListener(delegate { UpdateMusicVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdateSFXVolume(); });
    }

    public void PlaySFX(string name)
    {
        sfxSource.PlayOneShot(audioClips[name]);
    }

    public void UpdateMusicVolume()
    {
        musicSource.volume = musicSlider.value;

        PlayerPrefs.SetFloat("Music", musicSlider.value);
    }

    public void UpdateSFXVolume()
    {
        sfxSource.volume = sfxSlider.value;

        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
    }

    private void LoadVolume()
    {
        musicSource.volume = PlayerPrefs.GetFloat("Music", 0.5f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFX", 0.5f);

        musicSlider.value = musicSource.volume;
        sfxSlider.value = sfxSource.volume;
    }
}