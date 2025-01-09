using UnityEngine;
using UnityEngine.UI;

public class KeepModelScreenSize : MonoBehaviour
{
    public float desiredScreenHeight = 200f; // 期望的屏幕高度

    private Transform mtr;
    private Camera mainCamera;
    public Text kTxt;
    private void Awake()
    {
        mtr = this.transform;
        mainCamera = Camera.main;
    }

    public void SetPartName(string partName)
    {
        kTxt.text = partName;
    }

    void Update()
    {
        //欧拉角相等 面向相机
        mtr.eulerAngles = mainCamera.transform.eulerAngles;

        // 获取3D模型在屏幕上的位置
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // 计算模型在屏幕上的高度
        float modelScreenHeight = Mathf.Abs((screenPos.y - Camera.main.WorldToScreenPoint(transform.position + transform.up).y));

        // 计算缩放比例
        float scaleRatio = desiredScreenHeight / modelScreenHeight;

        // 设置缩放
        transform.localScale = new Vector3(scaleRatio, scaleRatio, scaleRatio);
    }
}

