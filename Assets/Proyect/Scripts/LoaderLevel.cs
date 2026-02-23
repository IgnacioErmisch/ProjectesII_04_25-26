using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderLevel : MonoBehaviour
{
    [SerializeField] public string levelNumber;
    [SerializeField] private TextMeshProUGUI textlevel;
    void Start()
    {
        textlevel.text = levelNumber;
    }
    public void LoadLevel()
    {
        string sceneName = "Level " + levelNumber;
        SceneManager.LoadScene(sceneName);
    }

}
