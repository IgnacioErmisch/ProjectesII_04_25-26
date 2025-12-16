using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SwitchInterface : MonoBehaviour
{
    [SerializeField] public bool IsBigCloneSelected;
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
        BigClone.color = IsBigCloneSelected ? bigSelectedColor : darkColor;
        SmallClone.color = IsBigCloneSelected ? darkColor : smallSelectedColor;
    }
    private void TabSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            if (BigClone != null && SmallClone != null)
            {
                IsBigCloneSelected = !IsBigCloneSelected;
                BigClone.color = IsBigCloneSelected ? bigSelectedColor : darkColor;
                SmallClone.color = IsBigCloneSelected ? darkColor : smallSelectedColor;
            }
           
        }
    }
}