using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private PerspectiveSwitch perspectiveSwitch;
    private GameObject energyBigClone;
    private GameObject energySmallClone;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Instance._player = _player;
            Instance.perspectiveSwitch = perspectiveSwitch;
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

    public void SetBigCloneEnergy(GameObject energyBigCloneImage)
    {
        energyBigClone = energyBigCloneImage;
    }
    public void SetSmallCloneEnergy(GameObject energySmallCloneImage)
    {
        energySmallClone = energySmallCloneImage;

    }
    public GameObject GetBigCloneEnergy()
    {
        return energyBigClone;
    }

    public GameObject GetSmallCloneEnergy()
    {
        return energySmallClone;
    }
}
