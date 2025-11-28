using UnityEngine;

public class SwitchInterface : MonoBehaviour
{
    public GameObject bigCloneSelected;
    public GameObject smallCloneSelected;
    public GameObject EBigClone;
    public GameObject ESmallClone;
    public bool isBigCloneSelected;
    public bool isSmallCloneSelected;

    void Start()
    {
        bigCloneSelected.SetActive(true);
        EBigClone.SetActive(true);
        smallCloneSelected.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CloneSelected();
        TabSwitch();
    }

    private void CloneSelected()
    {
        if (bigCloneSelected.activeInHierarchy)
        {
            isBigCloneSelected = true;
            isSmallCloneSelected = false;
        }
        else if (smallCloneSelected.activeInHierarchy)
        {
            isBigCloneSelected = false;
            isSmallCloneSelected = true;
        }
    }

    private void TabSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && isBigCloneSelected)
        {
            smallCloneSelected.SetActive(true);
            bigCloneSelected.SetActive(false);
            ESmallClone.SetActive(true);
            EBigClone.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isSmallCloneSelected)
        {
            smallCloneSelected.SetActive(false);
            bigCloneSelected.SetActive(true);
            ESmallClone.SetActive(false);
            EBigClone.SetActive(true);
        }
    }

    public bool GetBigCloneSelected()
    {
        return isBigCloneSelected;
    }
    public bool GetSmallCloneSelected()
    {
        return isSmallCloneSelected;
    }
}
