using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartIcon : MonoBehaviour
{
    public Button kBtn;
    private Toggle mTog;
    public Image kImg;
    public TMP_Text kNameTxt;
    private ElementObj mElement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //kBtn.onClick.AddListener(() => {
        //EventDispatcher<EventDef, string>.DispatchEvent(EventDef.IconClicked, mPartName);
        //});
        mTog = GetComponent<Toggle>();
        mTog.group = transform.parent.GetComponent<ToggleGroup>();
        mTog.onValueChanged.AddListener(isOn => { 
            if(isOn)
            {
                //AssemblyPanel.CurIconIndex = transform.GetSiblingIndex();
                EventDispatcher<EventDef, PartIcon>.DispatchEvent(EventDef.IconClicked, this);
            }
        });
    }

    public void SetData(ElementObj elem)
    {
        
        mElement = elem;
        kNameTxt.text = elem.fullName;
        Sprite sprite = Resources.Load<Sprite>("sprites/" + GlobalData.CurTrackedId + "/" + elem.shortName);
        kImg.sprite = sprite;
    }

}
