using UnityEngine;
using UnityEngine.UI;

public class ItemTipsMng : MonoBehaviour
{
    public static ItemTipsMng Instance;
    public float desiredScreenHeight = 200f; // ��������Ļ�߶�

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

