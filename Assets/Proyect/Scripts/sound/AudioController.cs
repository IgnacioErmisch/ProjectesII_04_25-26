using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        float musicValue = PlayerPrefs.GetFloat("Music", 0.5f);
        float sfxValue = PlayerPrefs.GetFloat("SFX", 0.5f);


        musicSlider.value = musicValue;
        sfxSlider.value = sfxValue;


        SetMusicVolume(musicValue);
        SetSFXVolume(sfxValue);


        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float value)
    {

        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1)) * 20;
        audioMixer.SetFloat("Music", volume);
        PlayerPrefs.SetFloat("Music", value);
        PlayerPrefs.Save();

    }

    public void SetSFXVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1)) * 20;
        audioMixer.SetFloat("SFX", volume);
        PlayerPrefs.SetFloat("SFX", value);
        PlayerPrefs.Save();
    }
}
