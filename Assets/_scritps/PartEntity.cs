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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mPartName = transform.GetSiblingIndex().ToString();
        mHighlighter = GetComponent<Highlighter>();
        GetInitMat();
    }

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

    public void DoSelect(bool isSelected)
    {
        mIsSelected = isSelected;
        if(isSelected)
            mHighlighter.FlashingOn();
        else
            mHighlighter.FlashingOff();

        DoTrans(isSelected);
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
    }

    public void DoHide(bool isHided)
    {
        mIsHided = isHided;
        gameObject.SetActive(!mIsHided);
    }
}
