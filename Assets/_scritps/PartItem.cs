using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartItem : MonoBehaviour
{
    public Toggle kSelectedTog;
    public TMP_Text kIDTxt;
    public TMP_Text kNameTxt;
    public GameObject kRedIcon;
    private string mName;
    private PartEntity mEntity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        kSelectedTog.onValueChanged.AddListener(isOn => { DoSelect(isOn, true); });
        kIDTxt.text = (transform.GetSiblingIndex() + 1).ToString();

    }

    public void SetEntity(PartEntity entiy) 
    { 
        mEntity = entiy; 
        mName = kNameTxt.text = entiy.mPartName;
    }

    public void DoSelect(bool isSelected, bool expandAction = false)
    {
        kSelectedTog.isOn = isSelected;
        if (expandAction)
            mEntity?.DoSelect(isSelected);
    }

    public void DoHideOrTrans(bool isHideOrTrans)
    {
        kRedIcon.SetActive(isHideOrTrans);
    }

}
