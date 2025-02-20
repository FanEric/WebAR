using UnityEngine;
using HighlightingSystem;
using System;

public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GetComponent<Highlighter>().FlashingOn();
        GetTime();
    }

    void GetTime()
    {
        DateTime nowTime = DateTime.Now.ToLocalTime();

        Debug.Log(nowTime.ToString("HH:mm:ss"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
