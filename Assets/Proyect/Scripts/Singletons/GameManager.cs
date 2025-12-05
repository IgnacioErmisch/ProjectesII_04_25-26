using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private PerspectiveSwitch perspectiveSwitch;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetPlayer()
    {
        return _player;
    }
    public void SetPlayer(GameObject player)
    {
        _player = player;
    }
    public void SetControlllingPlayer(PerspectiveSwitch perspective)
    {
       perspectiveSwitch = perspective;
    }
    public bool GetControlllingPlayer()
    {
        return perspectiveSwitch.controllingPlayer;
    }
}
