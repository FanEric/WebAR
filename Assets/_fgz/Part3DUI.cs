using UnityEngine;
using UnityEngine.UI;

public class Part3DUI : MonoBehaviour
{
    private Transform kTarget;
    public float height;
    public float desire = 10f;
    public Text kTxt;
    private Transform mRoot;
    private Transform mLabelTrans;

    public void SetData(string pname, Transform target)
    {
        kTxt.text = pname;
        kTarget = target;
        MeshCollider collider = target.GetComponent<MeshCollider>();
        if (collider == null)
            collider = target.GetComponentsInChildren<MeshCollider>()[0];
        Bounds bounds = collider.bounds;
        Vector3 size = bounds.size;
        float big = size.x > size.y ? size.x : size.y;
        height = big / desire;
        mRoot = kTarget.root;

        mLabelTrans = kTarget.GetChild(0);
    }

    private void Update()
    {
        //transform.position = kTarget.position + Vector3.up * height * mRoot.localScale.x;
        transform.position = mLabelTrans.position;
        transform.forward = Camera.main.transform.forward;
    }

    public void Show() { gameObject.SetActive(true); }

    public void Hide() {  gameObject.SetActive(false); }
}
