using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuScene : MonoBehaviour
{

    public GameObject panelSettings;
    void Start()
    {
        Slider musicSlider = GameObject.Find("Musica")?.GetComponent<Slider>();
        Slider sfxSlider = GameObject.Find("SFX")?.GetComponent<Slider>(); 
    }

    void Update()
    {

    }

    public void LoadMap()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        Application.Quit();

    }

    public void Settings()
    {
        panelSettings.SetActive(true);
    }

    public void closeSettings()
    {
        panelSettings.SetActive(false);
    }
}
