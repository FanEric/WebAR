using UnityEngine;
using UnityEngine.UI;

public class Part2DUI : MonoBehaviour
{
    private Transform kTarget;
    public Text kTxt;
    private Transform mLabelTrans;
    public RectTransform ParentTran;
    public Vector2 Offset = Vector2.zero;

    public void SetData(string pname, Transform target)
    {
        kTxt.text = pname;
        kTarget = target;
        mLabelTrans = kTarget.GetChild(0);
        ParentTran = ManipulateObject.instance.kRectParent;
    }

    private void Update()
    {
        if(mLabelTrans)
        {
            Vector2 mScreenPos = Camera.main.WorldToScreenPoint(mLabelTrans.position);
            Vector2 mRectPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(ParentTran, mScreenPos, null, out mRectPos);
            transform.localPosition = mRectPos + Offset;
        }
    }

    public void Show() { gameObject.SetActive(true); }

    public void Hide() {  gameObject.SetActive(false); }
}
