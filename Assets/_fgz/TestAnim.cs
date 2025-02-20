using UnityEngine;
using UnityEngine.UI;
using HighlightingSystem;
using UnityEditor;

public class TestAnim : MonoBehaviour
{
    public Animator mPartAnim;
    public Toggle kAssemTog;
    public Toggle kDisassTog;
    public Transform kRoot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        kDisassTog.onValueChanged.AddListener(isOn =>
        {
            if (isOn) mPartAnim?.SetInteger("DoAssem", 1);
        });
        kAssemTog.onValueChanged.AddListener(isOn =>
        {
            if (isOn) mPartAnim?.SetInteger("DoAssem", 2);
        });

        HighlightingSystem.Highlighter[] hs = kRoot.GetComponentsInChildren<HighlightingSystem.Highlighter>();
        foreach (var item in hs)
        {
            item.ConstantOn();
        }
    }

    // ÷ÿ÷√Animator
    public void ResetAnimator()
    {
        Debug.Log("1111111111");
        mPartAnim.Play("Assembled");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            ResetAnimator();
    }
}
