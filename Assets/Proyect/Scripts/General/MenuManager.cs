using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private bool menuIsOpen; 
    void Start()
    {
        pauseMenu.SetActive(false);
        menuIsOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        OpenMenu();
       
    }

    public void OpenMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
         
        }    
    }
    public void Return()
    {
         pauseMenu.SetActive(false);
    }
    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
