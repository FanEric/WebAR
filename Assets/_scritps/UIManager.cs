using Imagine.WebAR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Toggle kBrowseTog;
    public Toggle kInterTog;
    public Toggle kVoiceTog;

    #region content of browse
    public GameObject kBrowseContent;
    #endregion

    public ManipulateObject kManiScript;
    public ImageTracker kTracker;

    // Start is called before the first frame update
    void Start()
    {
        kBrowseTog.onValueChanged.AddListener(ToBrowse);
        kInterTog.onValueChanged.AddListener(ToInter);
        kVoiceTog.onValueChanged.AddListener(ToVoice);

        kTracker.OnImageFound.AddListener(str => { DoReset(); });
        DoReset();
    }

    void ToBrowse(bool isOn)
    {
        kTracker.SetTargetChildEnable(0);

        kBrowseContent.SetActive(isOn);
    }

    void ToInter(bool isOn)
    {
        kTracker.SetTargetChildEnable(1);
        kManiScript.DoReset();

        kManiScript.doTranslate = false;
        kManiScript.doRotate = true;
        kManiScript.doScale = false;

    }

    void ToVoice(bool isOn)
    {
        kTracker.SetTargetChildEnable(2);
        kManiScript.DoReset();

        kManiScript.doTranslate = false;
        kManiScript.doRotate = false;
        kManiScript.doScale = false;

    }

    // Update is called once per frame
    void DoReset()
    {
        kBrowseTog.isOn = true;
        kManiScript.DoReset();
    }
}
