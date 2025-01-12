using UnityEngine;
using UnityEngine.UI;

public class ItemTipsMng : MonoBehaviour
{
    public static ItemTipsMng Instance;
    public float desiredScreenHeight = 200f; // 期望的屏幕高度

    private Transform mtr;
    private Camera mainCamera;
    public Text kTxt;
    private void Awake()
    {
        Instance = this;
        mtr = this.transform;
        mainCamera = Camera.main;
    }

    public void Show(GameObject part, string pname)
    {
        kTxt.text = pname;
        transform.parent.SetParent(part.transform, true);
        Bounds bounds = part.GetComponent<MeshCollider>().bounds;
        Vector3 size = bounds.size;
        transform.position = bounds.center + new Vector3(0, size.y / 2f + .1f, 0);
    }

    public void Hide()
    {
        transform.position = new Vector3(0, 0, -1000);
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

