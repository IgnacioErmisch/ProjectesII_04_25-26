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
    void Update()
    {
        TabSwitch();
    }
    private void TabSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            IsBigCloneSelected = !IsBigCloneSelected;
            
            Debug.Log(IsBigCloneSelected ? "ClonGrande" : "ClonPequeño");
            BigClone.color = IsBigCloneSelected ? bigSelectedColor : Color.white;
            SmallClone.color = IsBigCloneSelected ? Color.white : smallSelectedColor;
        }
    }
}
