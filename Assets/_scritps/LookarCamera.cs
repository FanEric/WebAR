using Imagine.WebAR;
using UnityEngine;

public class LookarCamera : MonoBehaviour
{
    Transform mCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mCamera.position);
    }
}
