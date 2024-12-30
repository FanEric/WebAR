using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITransformPanel : MonoBehaviour
{
    public Toggle kTranslateTog;
    public Toggle kRotateTog;
    public Toggle kScaleTog;
    public ManipulateObject kManiScript;

    // Start is called before the first frame update
    void Awake()
    {
        kTranslateTog.onValueChanged.AddListener(isOn => { kManiScript.doTranslate = isOn; });
        kRotateTog.onValueChanged.AddListener(isOn => { kManiScript.doRotate = isOn; });
        kScaleTog.onValueChanged.AddListener(isOn => { kManiScript.doScale = isOn; });
    }

    private void OnEnable()
    {
        kTranslateTog.isOn = true;
    }
}
