using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class JobItem : MonoBehaviour
{
    public TMP_Text kCont;
    public TMP_Text kStatus;
    public TMP_Text kTime;
    public Image kBg;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SetData(string cont, Sprite sprite, string status = "")
    {
        kCont.text = cont;
        kBg.sprite = sprite;
        kStatus.text = string.IsNullOrEmpty(status) ? "ÒÑÍê³É" : status;
        kTime.text = DateTime.Now.ToLocalTime().ToString("HH:mm:ss");
    }

}
