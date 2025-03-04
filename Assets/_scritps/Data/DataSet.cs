using System;
using System.Collections.Generic;
using UnityEngine;

public class DataSet : MonoBehaviour
{
    public static DataSet instance;

    public AudioClip k11;
    public AudioClip k12;
    public AudioClip k21;
    public AudioClip k22;
    public AudioClip k31;
    public AudioClip k32;
    public AudioClip k41;
    public AudioClip k42;
    public AudioClip k51;
    public AudioClip k52;
    public AudioClip k61;
    public AudioClip k62;
    public AudioClip k71;
    public AudioClip k72;
    public AudioClip k81;
    public AudioClip k82;
    public AudioClip k91;
    public AudioClip k92;
    public Color kHightlightColor;
    public Dictionary<string, List<AudioClip>> audios = new Dictionary<string, List<AudioClip>>();
    public Material[] kTransMats;

    //public List<Element> elements = new List<Element>();
    void Awake()
    {
        instance = this;

        audios.Add(Const.SFHEQ, new List<AudioClip>() { k11, k12});
        audios.Add(Const.JSJG, new List<AudioClip>() { k21, k22});
        audios.Add(Const.DQXT, new List<AudioClip>() { k31, k32});
        audios.Add(Const.DCRGLXT, new List<AudioClip>() { k41, k42});
        audios.Add(Const.KTYSJ, new List<AudioClip>() { k51, k52});
        audios.Add(Const.YCTBDDJ, new List<AudioClip>() { k61, k62});
        audios.Add(Const.ZLYSDJ, new List<AudioClip>() { k71, k72});
        audios.Add(Const.LXJRJ, new List<AudioClip>() { k81, k82});
        audios.Add(Const.JCQ, new List<AudioClip>() { k91, k92});
    }

    public List<Element> GetAssembleData(string id)
    {
        TextAsset ta = Resources.Load<TextAsset>("configs/" + id);
        if(ta == null)
            Debug.LogError("拆装数据不存在， id："  + id);
        string txt = ta.text;
        Debug.Log("json: " + txt);
        Elements elems = JsonUtility.FromJson<Elements>(txt);
        List<Element>  elements = elems.elements;
        int count = elements.Count;
        Debug.Log(id + " 零件个数：" + count);
        return elements;
    }
    
}

[Serializable]
public class Elements
{
    public List<Element> elements;
}

[Serializable]
public class Element
{
    public string shortName;
    public string fullName;
    public bool hasAnimation;
}