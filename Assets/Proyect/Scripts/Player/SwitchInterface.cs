using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SwitchInterface : MonoBehaviour
{
    [SerializeField] public bool IsBigCloneSelected;
    [SerializeField] private Image BigClone;
    [SerializeField] private Image SmallClone;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Color bigSelectedColor;
    [SerializeField] private Color smallSelectedColor;
    Color darkColor = new Color32(41, 39, 39, 255);

    private Controller inputActions;

    private void Awake()
    {
        inputActions = new Controller();
    }

    private void OnEnable()
    {
        inputActions.Gameplay.Enable();
    }

    private void OnDisable()
    {
        inputActions.Gameplay.Disable();
    }

    void Update()
    {
      
        if (inputActions.Gameplay.SelectSmallClone.triggered && !gameManager.GetCloneActive())
        {
            SelectSmallClone();
        }

        if (inputActions.Gameplay.SelectBigClone.triggered && !gameManager.GetCloneActive())
        {
            SelectBigClone();
        }
    }

    private void Start()
    {
        BigClone.color = IsBigCloneSelected ? bigSelectedColor : darkColor;
        SmallClone.color = IsBigCloneSelected ? darkColor : smallSelectedColor;
        gameManager = GameManager.Instance;
    }

    private void SelectBigClone()
    {
        if (!IsBigCloneSelected)
        {
            IsBigCloneSelected = true;
            UpdateUI();
        }
    }

    private void SelectSmallClone()
    {
        if (IsBigCloneSelected)
        {
            IsBigCloneSelected = false;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        BigClone.color = IsBigCloneSelected ? bigSelectedColor : darkColor;
        SmallClone.color = IsBigCloneSelected ? darkColor : smallSelectedColor;
    }
}