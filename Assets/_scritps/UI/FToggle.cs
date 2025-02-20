using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class FToggle : MonoBehaviour
{
    private Toggle toggle;
    public bool kInitOn = false;
    public GameObject kOnImg;
    public GameObject kOffImg;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        if(kOnImg != null && kOffImg != null)
        {
            toggle.onValueChanged.AddListener(isOn => {
                kOnImg?.SetActive(isOn);
                kOffImg?.SetActive(!isOn);
            });
        }
    }

    private void OnEnable()
    {
        toggle.isOn = kInitOn;
    }

}
