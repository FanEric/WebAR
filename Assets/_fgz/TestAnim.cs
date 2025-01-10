using UnityEngine;
using UnityEngine.UI;

public class TestAnim : MonoBehaviour
{
    public Animator mPartAnim;
    public Toggle kAssemTog;
    public Toggle kDisassTog;
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
