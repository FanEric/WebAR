using UnityEngine;
using HighlightingSystem;
using System.Collections.Generic;
using UnityEditor;

public class ElementObj : MonoBehaviour
{
    public string shortName;
    public string fullName;
    public bool hasAnimation;

    HighlightingSystem.Highlighter[] mHighlighters;
    private Collider mCollider;
    Renderer[] mAllRenders;
    Dictionary<Renderer, Material[]> mRenderMats = new Dictionary<Renderer, Material[]>();
    void Awake()
    {
        mHighlighters = GetComponentsInChildren<HighlightingSystem.Highlighter>();
        mCollider = GetComponentInChildren<Collider>();

        GetInitMat();
        DisableCollider();
    }
    void GetInitMat()
    {
        mAllRenders = GetComponentsInChildren<Renderer>();
        //Debug.Log(mPartName + " :" + mAllRenders.Length);
        foreach (var item in mAllRenders)
        {
            mRenderMats.Add(item, item.materials);
        }
    }

    public void DisableCollider()
    {
        mCollider.enabled = false;
    }

    public void PreDisassemble()
    {
        Debug.Log("PreDisassemble: " + shortName);
        foreach (var item in mHighlighters)
        {
            item.ConstantOn(DataSet.instance.kHightlightColor);
        }
        mCollider.enabled = true;
    }

    public void DoDisassemble()
    {
        mCollider.enabled = false;
        foreach (Renderer renderer in mAllRenders) {
            renderer.enabled = false;
        }
        DoHighlightOff();
    }

    public void PreAssemble()
    {
        Debug.Log("PreAssemble: " + shortName);
        foreach (var item in mHighlighters)
        {
            item.ConstantOn(DataSet.instance.kHightlightColor);
        }
        mCollider.enabled = true;
    }

    public void DoHighlightOff()
    {
        foreach (var item in mHighlighters)
        {
            item.ConstantOff();
        }
    }

    public void DoAssemble()
    {
        mCollider.enabled = false;
        DoTrans(false);
        DoHighlightOff();
    }

    public void DoTrans(bool isTrans)
    {
        if (isTrans)
        {
            foreach (var item in mAllRenders)
            {
                item.enabled = true;
                item.materials = DataSet.instance.kTransMats;
            }
        }
        else
        {
            foreach (var item in mAllRenders)
            {
                item.enabled = true;
                item.materials = mRenderMats[item];
            }
        }
    }

}
