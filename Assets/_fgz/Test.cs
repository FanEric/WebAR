using UnityEngine;
using HighlightingSystem;

public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Highlighter>().FlashingOn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
