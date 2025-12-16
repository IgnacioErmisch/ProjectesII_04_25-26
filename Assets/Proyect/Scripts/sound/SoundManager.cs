using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource loopSource; 

    [Header("Music")]
    public AudioClip background;

    [Header("SFX")]
    public AudioClip death;
    public AudioClip dash;
    public AudioClip jumpOnEnemies;
    public AudioClip jump;
    public AudioClip spawnClon;

    [Header("Movement Loops")]
    public AudioClip movementSC;
    public AudioClip movementBC;
    public AudioClip movementP;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();
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