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
    public Color kHightlightColor;
    public Dictionary<string, List<AudioClip>> audios = new Dictionary<string, List<AudioClip>>();
    public Material[] kTransMats;

    //public List<Element> elements = new List<Element>();
    void Awake()
    {
        instance = this;

        audios.Add(Const.SFHEQ, new List<AudioClip>() { k11, k12});
        audios.Add(Const.JSJG, new List<AudioClip>() { k21, k22});
        audios.Add(Const.DQXT, new List<AudioClip>() { k21, k22});
        audios.Add(Const.DCRGLXT, new List<AudioClip>() { k21, k22});
        audios.Add(Const.KTYSJ, new List<AudioClip>() { k21, k22});
        audios.Add(Const.YCTBDDJ, new List<AudioClip>() { k21, k22});
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