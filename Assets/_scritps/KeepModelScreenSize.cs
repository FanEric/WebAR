using UnityEngine;
using UnityEngine.UI;

public class KeepModelScreenSize : MonoBehaviour
{
    public float desiredScreenHeight = 200f; // ��������Ļ�߶�

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
        //ŷ������� �������
        mtr.eulerAngles = mainCamera.transform.eulerAngles;

        // ��ȡ3Dģ������Ļ�ϵ�λ��
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // ����ģ������Ļ�ϵĸ߶�
        float modelScreenHeight = Mathf.Abs((screenPos.y - Camera.main.WorldToScreenPoint(transform.position + transform.up).y));

        // �������ű���
        float scaleRatio = desiredScreenHeight / modelScreenHeight;

        // ��������
        transform.localScale = new Vector3(scaleRatio, scaleRatio, scaleRatio);
    }
}

