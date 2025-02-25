using UnityEngine;
using HighlightingSystem;
using System.Collections.Generic;
using System.Collections;

public class PartEntity : MonoBehaviour
{
    public string mPartName;
    public bool mIsSelected;
    public bool mIsTransparent;
    public bool mIsHided;

    HighlightingSystem.Highlighter mHighlighter;
    Renderer[] mAllRenders;
    Dictionary<Renderer, Material[]> mRenderMats = new Dictionary<Renderer, Material[]>();

    private PartItem mItem;
    private Vector3 mLocalPos;
    public float mModelHight;

    private Part3DUI mPart3DUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mHighlighter = GetComponent<HighlightingSystem.Highlighter>();
        GetInitMat();
        Bounds bounds = GetComponent<MeshCollider>().bounds;
        Vector3 size = bounds.size;
        mModelHight = size.x > size.y ? size.x : size.y;

        mLocalPos = transform.localPosition;

        Init3DUI();
    }

    void Init3DUI()
    {
        mPart3DUI = Instantiate(StructPanel.Instance.k3DUIObj).GetComponent<Part3DUI>();
        mPart3DUI.SetData(mPartName, transform);
        mPart3DUI.Hide();
    }

    public void Show3DUI() { mPart3DUI.Show(); }
    public void Hide3DUI() { mPart3DUI?.Hide(); }

    public void SetItem(PartItem item) { mItem = item; }

    void GetInitMat()
    {
        mAllRenders = GetComponentsInChildren<Renderer>();
        //Debug.Log(mPartName + " :" + mAllRenders.Length);
        foreach (var item in mAllRenders)
        {
            mRenderMats.Add(item, item.materials);
        }
    }

    public IEnumerator DoMono()
    {
        yield return new WaitForEndOfFrame();
        transform.localPosition = Vector3.zero;
        yield return new WaitForEndOfFrame();
        transform.localPosition = Vector3.zero;
        DoFlashing(false);
        mPart3DUI.Hide();
    }

    public void UndoMono()
    {
        transform.localPosition = mLocalPos;
    }

    public void DoSelect(bool isSelected, bool expandAction = false)
    {
        mIsSelected = isSelected;
        EventDispatcher<EventDef, PartEntity>.DispatchEvent(EventDef.PartSelect, this);

        if(StructPanel.Instance.IsMono)
        {
            DoFlashing(false);
            mPart3DUI.Hide();
            return;
        }

        if (isSelected)
        {
            mPart3DUI.Show();
        }
        else
        {
            mPart3DUI.Hide();
        }

        DoFlashing(mIsSelected);

        if (expandAction)
            mItem?.DoSelect(isSelected);
    }

    void DoFlashing(bool isSelected)
    {
        if (isSelected)
            mHighlighter.FlashingOn();
        else
            mHighlighter.FlashingOff();
    }

    public void DoTrans(bool isTrans)
    {
        mIsTransparent = isTrans;
        if (mIsTransparent)
        {
            foreach (var item in mAllRenders)
            {
                item.materials = DataSet.instance.kTransMats;
            }
        }
        else
        {
            foreach (var item in mAllRenders)
            {
                item.materials = mRenderMats[item];
            }
        }
        mItem.DoHideOrTrans(mIsTransparent || mIsHided);
    }

    public void DoHide(bool isHided)
    {
        mIsHided = isHided;
        gameObject.SetActive(!mIsHided);
        mItem.DoHideOrTrans(mIsTransparent || mIsHided);
    }
}
