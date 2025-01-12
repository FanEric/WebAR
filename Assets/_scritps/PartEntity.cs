using UnityEngine;
using HighlightingSystem;
using NUnit.Framework;
using System.Collections.Generic;

public class PartEntity : MonoBehaviour
{
    public string mPartName;
    public bool mIsSelected;
    public bool mIsTransparent;
    public bool mIsHided;

    public Material kTransMat;
    Material[] mTransMat = new Material[1];
    Highlighter mHighlighter;
    Renderer[] mAllRenders;
    Dictionary<Renderer, Material[]> mRenderMats = new Dictionary<Renderer, Material[]>();

    private PartItem mItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mHighlighter = GetComponent<Highlighter>();
        GetInitMat();
    }

    public void SetItem(PartItem item) { mItem = item; }

    void GetInitMat()
    {
        mTransMat[0] = kTransMat;
        mAllRenders = GetComponentsInChildren<Renderer>();
        //Debug.Log(mPartName + " :" + mAllRenders.Length);
        foreach (var item in mAllRenders)
        {
            mRenderMats.Add(item, item.materials);
        }
    }

    public void DoSelect(bool isSelected, bool expandAction = false)
    {
        mIsSelected = isSelected;
        EventDispatcher<EventDef, PartEntity>.DispatchEvent(EventDef.PartSelect, this);

        if (isSelected)
        {
            ItemTipsMng.Instance.Show(gameObject, mPartName);
        }
        else
        {
            ItemTipsMng.Instance.Hide();
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
            Material[] materials = { kTransMat };
            foreach (var item in mAllRenders)
            {
                item.materials = materials;
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
