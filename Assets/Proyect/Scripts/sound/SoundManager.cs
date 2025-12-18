using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

   
    public AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource loopSource;  
    public AudioClip background;  
    public AudioClip death;
    public AudioClip dash;
    public AudioClip jumpOnEnemies;
    public AudioClip jump;
    public AudioClip spawnClon;
    public AudioClip movementSC;
    public AudioClip movementBC;
    public AudioClip movementP;    
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Awake()
    {
      
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();

        float musicValue = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxValue = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        SetMusicVolume(musicValue);
        SetSFXVolume(sfxValue);     
        ConfigureSliders(musicValue, sfxValue);
    }

    private void ConfigureSliders(float musicValue, float sfxValue)
    {
        if (musicSlider != null)
        {
            musicSlider.value = musicValue;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxValue;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

   
    public void AssignSliders(Slider music, Slider sfx)
    {
        musicSlider = music;
        sfxSlider = sfx;

        float musicValue = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxValue = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        ConfigureSliders(musicValue, sfxValue);
    }

   
    public void SetMusicVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1)) * 20;
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1)) * 20;
        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

   
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayLoop(AudioClip clip)
    {
        if (loopSource.clip == clip && loopSource.isPlaying)
            return;

        loopSource.clip = clip;
        loopSource.loop = true;
        loopSource.Play();
    }

    public void StopLoop()
    {
        loopSource.Stop();
        loopSource.clip = null;
    }

}