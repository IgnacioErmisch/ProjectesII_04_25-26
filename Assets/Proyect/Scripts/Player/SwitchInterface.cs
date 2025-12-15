using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SwitchInterface : MonoBehaviour
{
    public bool IsBigCloneSelected { get; private set; } = true;
    [SerializeField] private Image BigClone;
    [SerializeField] private Image SmallClone;

    [SerializeField] private Color bigSelectedColor;
    [SerializeField] private Color smallSelectedColor;
    Color darkColor = new Color32(41, 39, 39, 255);

    void Update()
    {
        TabSwitch();
    }

    private void Start()
    {
        SmallClone.color = darkColor;
    }
    private void TabSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            IsBigCloneSelected = !IsBigCloneSelected;

            BigClone.color = IsBigCloneSelected ? bigSelectedColor : darkColor;
            SmallClone.color = IsBigCloneSelected ? darkColor : smallSelectedColor;
        }
    }
}