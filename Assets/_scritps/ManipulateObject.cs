using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ManipulateObject : MonoBehaviour
{
    private float previousDistance;
    private Vector2 previousPosition;
    private Vector2[] previousTouchPositions = new Vector2[2];
    private Vector3 previousTranslatePosition;
    //public TMP_InputField kTransRate;
    public TMP_Text kSldVale;
    public Slider kSld;
    public float kTransRate = 0.001f;

    public float rotateSpeedX = 4f;
    public float rotateSpeedY = 2f;
    public float rotateDelta = 5f;
    public float mouseX = 0f;
    public float mouseY = 60f;
    public float angleMax = 90;
    public float angleMin = -90;

    public float panSpeed = 5f;
    public bool doTranslate = true;
    public bool doRotate = false;
    public bool doScale = false;

    public float scaleRate = 5f;
    public float scaleMin = 0.1f;
    public float scaleMax = 5;

    Transform mTrans;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    public EventSystem eventSystem;
    public GraphicRaycaster graphicRaycaster;

    private void Awake()
    {
        mTrans = transform;
    }

    bool CheckMouseOnUI()
    {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, list);
        return list.Count > 0;
    }


    public void DoReset()
    {
        if (mTrans != null)
        {
            mTrans.position = Vector3.zero;
            mTrans.rotation = Quaternion.identity;
            mTrans.localScale = Vector3.one;
        }
    }

    void HandleMouse()
    {
        if (CheckMouseOnUI()) return;
        if (doTranslate && Input.GetMouseButton(0))
        {
            mTrans.Translate(Vector3.right * Input.GetAxis("Mouse X") * Time.fixedDeltaTime * panSpeed, Space.World);
            mTrans.Translate(Vector3.up * Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * panSpeed, Space.World);
        }

        if (doRotate && Input.GetMouseButton(0))
        {
            //mouseX -= Input.GetAxis("Mouse X") * rotateSpeedX;
            //mouseY += Input.GetAxis("Mouse Y") * rotateSpeedY;

            //mouseY = ClampAngle(mouseY, angleMin, angleMax);

            //currentRotation = mTrans.rotation;
            //desiredRotation = Quaternion.Euler(mouseY, mouseX, 0);
            //Quaternion rotation = Quaternion.Slerp(currentRotation, desiredRotation, Time.fixedDeltaTime * rotateDelta);
            //mTrans.rotation = rotation;
            mTrans.Rotate(Vector3.down * Input.GetAxis("Mouse X") * rotateSpeedX, Space.World);//绕Y轴进行旋转
            mTrans.Rotate(Vector3.right * Input.GetAxis("Mouse Y") * rotateSpeedY, Space.World);//绕X轴进行旋转
        }

        if (doScale)
        {
            if (Input.GetMouseButton(0))
            {
                float axis = Input.GetAxis("Mouse X");
                if(axis != 0)
                {
                    float factor = axis * Time.fixedDeltaTime * scaleRate;
                    mTrans.localScale += Vector3.one * factor;
                    if (mTrans.localScale.x <= scaleMin)
                        mTrans.localScale = new Vector3(scaleMin, scaleMin, scaleMin);
                    else if(mTrans.localScale.x >= scaleMax)
                        mTrans.localScale = new Vector3(scaleMax, scaleMax, scaleMax);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            DoReset();
        }
    }

    void HandleTouch()
    {
        //kSldVale.text = kSld.value.ToString();
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                if (previousPosition == Vector2.zero)
                {
                    previousPosition = touch.position;
                }
                else
                {
                    Vector2 deltaPosition = touch.position - previousPosition;
                    transform.Rotate(Vector3.up, -deltaPosition.x * 0.5f);
                    previousPosition = touch.position;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                previousPosition = Vector2.zero;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                previousTouchPositions[0] = touch1.position;
                previousTouchPositions[1] = touch2.position;
                previousDistance = Vector2.Distance(touch1.position, touch2.position);
                previousTranslatePosition = transform.position;
            }
            else if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                Vector2 currentTouchPosition1 = touch1.position;
                Vector2 currentTouchPosition2 = touch2.position;
                float currentDistance = Vector2.Distance(currentTouchPosition1, currentTouchPosition2);
                float scaleFactor = currentDistance / previousDistance;
                transform.localScale *= scaleFactor;
                previousDistance = currentDistance;

                // 计算双指平移
                Vector2 midPoint = (currentTouchPosition1 + currentTouchPosition2) / 2;
                Vector2 previousMidPoint = (previousTouchPositions[0] + previousTouchPositions[1]) / 2;
                Vector3 translation = new Vector3(midPoint.x - previousMidPoint.x, 0, midPoint.y - previousMidPoint.y);
                transform.position = previousTranslatePosition + translation * kTransRate;
                //transform.position = previousTranslatePosition + translation * kTransRate * kSld.value;
            }
        }
    }
   
    void Update() 
    {

        HandleMouse();

        //HandleTouch();
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

}