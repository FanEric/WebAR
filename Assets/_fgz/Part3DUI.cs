using UnityEngine;
using UnityEngine.UI;

public class Part3DUI : MonoBehaviour
{
    private Transform kTarget;
    public float height;
    public float desire = 10f;
    public Text kTxt;
    private Transform mRoot;

    public string pname = "peiyeguan";

    //private void Start()
    //{
    //    GameObject obj = GameObject.Find(pname);
    //    SetData("hello", obj.transform);
    //}
    public void SetData(string pname, Transform target)
    {
        kTxt.text = pname;
        kTarget = target;
        Bounds bounds = kTarget.GetComponent<MeshCollider>().bounds;
        Vector3 size = bounds.size;
        float big = size.x > size.y ? size.x : size.y;
        height = big / desire;
        mRoot = kTarget.root;
    }

    private void Update()
    {
        transform.position = kTarget.position + Vector3.up * height * mRoot.localScale.x;
        transform.forward = Camera.main.transform.forward;
    }

    public void Show() { gameObject.SetActive(true); }

    public void Hide() {  gameObject.SetActive(false); }
}
