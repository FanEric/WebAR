using UnityEngine;

public class OrbitControl : MonoBehaviour
{
    #region property
    public Transform target;
    public Transform tracker;

    public float xSpeed = 30.0f;
    public float ySpeed = 360.0f;
    public float zoomSpeed = 1.2f;
    public float yMinLimit = -90f;
    public float yMaxLimit = 90f;
    public float wheelSensitivity = 0.1f;

    private Camera MainCamera;
    private float distance = 0.0f;

    private float angleX = 0.0f;
    public float angleY = 0.0f;

    private bool isTouch = false;
    private bool active = false;

    private Vector3 beginTween =Vector3.zero;
    private Vector3 tweenToPos =Vector3.zero;

    private Vector3 beginCenter =Vector3.zero;
    private Vector3 centerTweenPos =Vector3.zero;

    private float tweenTotalTime = 0.5f;
    private float tweenTime = 0;
    private bool tweeningCamera = false;

    private Vector2 lastMouse = new Vector2();

    public static OrbitControl Instance;

    public bool Enable = true;
    public bool EnableZoom = true;
    public bool EnablePan = true;
    public bool EnableRotate = false;
    public bool EnableSelfRotate = true;

    private bool isPanning = false;

    private float MoveSpeedBase = 25;
    private float MoveSpeed = 25;
    private Vector3 MoveDir =Vector3.zero;

    public bool EnableGravity = false;

    public float ViewHeight = 4;

    public bool EnableCollision = false;
    public float FarestRate = 10f;

    private bool isSelfRotate = false;
    private float rotateSpeed = 1;
    private Bounds mBounds = new Bounds();
    #endregion

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        MainCamera = GetComponent<Camera>();
        tracker.position = transform.position;
        tracker.rotation = transform.rotation;

        UpdateAngleDistanceFromTracker();
    }

    void UpdateMainCameraRotate(float deltaX, float deltaY)
    {
        if (!Enable || !EnableRotate)
            return;
        angleX += deltaX;// * xSpeed * 360 / Screen.width * 0.1f;
        angleY -= deltaY;// * ySpeed * 360 / Screen.height * 0.1f;

        angleY = clampAngle(angleY, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(angleY, angleX, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;
        tracker.rotation = rotation;
        tracker.position = position;
    }

    void UpdateMainCameraSelfRotate(float deltaX, float deltaY)
    {
        if (!Enable || !EnableSelfRotate)
            return;
        angleX += deltaX;// * xSpeed * 360 / Screen.width * 0.1f;
        angleY -= deltaY;// * ySpeed * 360 / Screen.height * 0.1f;

        angleY = clampAngle(angleY, yMinLimit, yMaxLimit);
        Quaternion rotation = Quaternion.Euler(angleY, angleX, 0);

        tracker.rotation = rotation;
    }

    float lastClick = 0f;
    float interval = 0.3f;
    public void Click()
    {
        //if (Input.touchCount > 1 || Constant.IsMeasuring)
        //    return;

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //UBRaycastHit hit;
        //if (UBUtils.Raycast(ray, out hit))
        //{
        //    UBRoot.Instance.Controller.Pick.SetSelectElement(hit.element);
        //    Debug.Log(hit.element.ElementId);
        //}
        //else
        //{
        //    UBRoot.Instance.Controller.Pick.CancelSelectElement(true);
        //    UBRoot.Instance.Controller.Event.DispatchEvent(UBEventType.ON_EMPTY_SELECT);
        //}

        //if ((lastClick + interval) > Time.time)
        //{
        //    TweenToElement(hit.element);//双击
        //}
        lastClick = Time.time;
    }

    //public void TweenToElement(UBElement element)
    //{
    //    if (element == null)
    //    {
    //        var dir = Vector3.one;
    //        dir.Normalize();
    //        TweenToDir(dir);
    //    }
    //    else
    //    {
    //        Bounds bounds = element.Bounds;
    //        Vector3 center = bounds.center;
    //        float length = bounds.size.magnitude * 2f;

    //        beginCenter = target.position;
    //        centerTweenPos = center;

    //        Vector3 dir = tracker.position - center;
    //        dir.Normalize();
    //        tweenToPos = dir * length + center;
    //        beginTween = tracker.position;

    //        tweeningCamera = true;
    //        tweenTime = 0;
    //    }
    //}

    /// <summary>
    /// 相机缓动到dir这个角度
    /// </summary>
    /// <param name="dir"></param>
    public void TweenToDir(Vector3 dir)
    {
        //Bounds bounds = UBRoot.Instance.Controller.Model.GetBounds();
        //Vector3 center = bounds.center;
        //float length = bounds.size.magnitude * 2f;

        //if (target == null)
        //    target = GameObject.Find("ModelCenter").transform;
        //if (tracker == null)
        //    tracker = GameObject.Find("ModelTracker").transform;

        //beginCenter = target.position;
        //centerTweenPos = center;

        //tweenToPos = dir * length + center;
        //beginTween = tracker.position;

        //tweeningCamera = true;
        //tweenTime = 0;
    }

    public void SetMainCameraRotate(float deltaX, float deltaY)
    {
        var x = deltaX / Screen.width * 100 * Mathf.PI;
        var y = deltaY / Screen.height * 100 * Mathf.PI;

        UpdateMainCameraRotate(x, y);
        UpdateMainCameraSelfRotate(x, y);
    }

    public void SetCameraRotateSelf()
    {
        if (!Enable || !EnableSelfRotate)
            return;

        angleY = 0;
        Quaternion rotation = Quaternion.Euler(angleY, angleX, 0);
        tracker.rotation = rotation;
    }

    public void UpdateMainCamera()
    {
        isTouch = Input.touchCount > 0;

        if (isTouch)
        {
            active = true;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                active = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                active = false;
            }

            if (Input.GetMouseButtonDown(2))
            {
                isPanning = true;
            }

            if (Input.GetMouseButtonUp(2))
            {
                isPanning = false;
            }
        }

        if (target && active)
        {
            float x = 0, y = 0;
            if (!isTouch)
            {
                x = Input.GetAxis("Mouse X") * Time.deltaTime * 1000;
                y = Input.GetAxis("Mouse Y") * Time.deltaTime * 1000;
            }
            else if (Input.touchCount == 1)
            {
                x = Input.GetTouch(0).deltaPosition.x;
                y = Input.GetTouch(0).deltaPosition.y;
            }

            x = x / Screen.width * 100 * Mathf.PI;
            y = y / Screen.height * 100 * Mathf.PI;

            UpdateMainCameraRotate(x, y);
            UpdateMainCameraSelfRotate(x, y);
        }

        if (isPanning)
        {
            if (!isTouch)
            {
                float x = 0, y = 0;
                x = Input.GetAxis("Mouse X") * Time.deltaTime * 1000;
                y = Input.GetAxis("Mouse Y") * Time.deltaTime * 1000;
                UpdatePan(x, y);
            }
        }

        if (MainCamera)
        {
            if (!isTouch)
            {
                float zoom = Input.GetAxis("Mouse ScrollWheel");
                if (zoom != 0f)
                {
                    float scale = 1;

                    if (zoom > 0)
                    {
                        scale = 0.95f;
                    }
                    else
                    {
                        scale = 1.05f;
                    }

                    UpdateZoom(scale, Input.mousePosition.x, Input.mousePosition.y);
                }
            }
            else
            {
                //if (Input.touchCount == 2)
                //{
                //    Touch touch0 = Input.GetTouch(0);
                //    Touch touch1 = Input.GetTouch(1);
                //    Vector2 last0 = touch0.position + touch0.deltaPosition;
                //    Vector2 last1 = touch1.position + touch1.deltaPosition;

                //    if (Vector2.Dot(touch0.deltaPosition, touch1.deltaPosition) > 0f)
                //    {
                //        UpdatePan((touch0.deltaPosition.x + touch1.deltaPosition.x) / 2, (touch0.deltaPosition.y + touch1.deltaPosition.y) / 2);
                //    }
                //    else
                //    {
                //        float dis = Vector2.Distance(last0, last1) - Vector2.Distance(touch0.position, touch1.position);
                //        float scale = dis > 0 ? 0.99f : 1.01f;
                //        UpdateZoom(scale, (touch0.position.x + touch1.position.x) / 2, (touch0.position.y + touch1.position.y) / 2);
                //    }
                //}
            }
        }
    }

    /// <summary>
    /// 缩放
    /// </summary>
    /// <param name="scale"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void UpdateZoom(float scale, float x, float y)
    {
        if (!Enable || !EnableZoom)
            return;

        Vector3 zoomCenter = MainCamera.ScreenToWorldPoint(new Vector3(x, y, 1));

        Vector3 dir = (zoomCenter - tracker.position).normalized;

        var length = Vector3.Distance(tracker.position, target.position);

        var newLength = length * scale;

        var dis = length - newLength;

        var eyeDir = (target.position - tracker.position).normalized;

        var dot = Vector3.Dot(dir, eyeDir);

        var moveDis = dis / dot;

        var targetMove = dir * moveDis - eyeDir * dis;

        if (Vector3.Distance(tracker.position + dir * moveDis, mBounds.center) < mBounds.size.magnitude * FarestRate)
        {
            tracker.position = tracker.position + dir * moveDis;

            target.position = target.position + targetMove;

            distance = newLength;
        }
    }

    /// <summary>
    /// 平移
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void UpdatePan(float x, float y)
    {
        if (!Enable || !EnablePan)
            return;

        var dir = target.position - tracker.position;

        var targetDistance = dir.magnitude;

        targetDistance *= Mathf.Tan(MainCamera.fieldOfView / 2f * Mathf.PI / 180f);

        var xDis = 2 * targetDistance * x / Screen.height;
        var xAxis = MainCamera.transform.localToWorldMatrix.GetColumn(0) * -xDis;

        var yDis = 2 * targetDistance * y / Screen.height;
        var yAxis = MainCamera.transform.localToWorldMatrix.GetColumn(1) * -yDis;

        var offset = new Vector3(xAxis.x + yAxis.x, xAxis.y + yAxis.y, xAxis.z + yAxis.z);

        tracker.position = tracker.position + offset;
        target.position = target.position + offset;

    }

    public void SetCamera(Vector3 center, Vector3 pos)
    {
        target.position = center;
        tracker.position = pos;
        distance = Vector3.Distance(tracker.position, target.position);
        MainCamera.orthographicSize = distance + 1.0f;
        MainCamera.nearClipPlane = 1.0f;

        tracker.LookAt(target);
    }

    private bool isDispatched = false;

    private void Update()
    {
        UpdateMainCamera();
        UpdateTweeningCamera();

        if (isSelfRotate)
        {
            tracker.RotateAround(target.position, Vector3.up, Mathf.Rad2Deg * 2f * Mathf.PI * 10f / Screen.width * rotateSpeed);
        }

        if (tweeningCamera && !isDispatched)
        {
            isDispatched = true;
        }
        else if (isDispatched && !tweeningCamera)
        {
            isDispatched = false;
        }
    }

    public void UpdateTweeningCamera()
    {
        if (!tweeningCamera)
            return;
        var deltaTime = Time.deltaTime;
        tweenTime += deltaTime;
        float progress = tweenTime / tweenTotalTime;

        if (progress >= 1)
        {
            tweeningCamera = false;
            progress = 1;
        }

        SetCamera(Vector3.Lerp(beginCenter, centerTweenPos, progress), Vector3.Lerp(beginTween, tweenToPos, progress));
        UpdateAngleDistanceFromTracker();
    }

    public void UpdateAngleDistanceFromTracker()
    {
        Vector3 angles = tracker.eulerAngles;
        angleX = angles.y;
        angleY = angles.x;
        if (angleY >= 270)
            angleY = angleY - 360;

        distance = Vector3.Distance(tracker.position, target.position);
    }

    public void ApplyAngleFromCurrect()
    {
        target.position = tracker.position + tracker.forward * distance;
        UpdateAngleDistanceFromTracker();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private float clampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public void UpdateCameraMoveFromKeys()
    {
        /// 向前
        if (Input.GetKey(KeyCode.W))
        {
            MoveForCamera(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            MoveForCamera(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            MoveForCamera(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveForCamera(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.Q) && !EnableGravity)
        {
            MoveForCamera(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.E) && !EnableGravity)
        {
            MoveForCamera(0, -1, 0);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void MoveForCamera(float x, float y, float z)
    {
        MoveDir.Set(x, y, z);
        var length = MoveDir.magnitude;
        if (y == 0)
        {
            MoveDir = MainCamera.transform.TransformDirection(MoveDir);
            MoveDir.y = 0;
            MoveDir.Normalize();
            //if (CheckCollision(MoveDir, 2f))
            //{
            //    return;
            //}
        }
        else
        {
            MoveDir.x = 0;
            MoveDir.z = 0;
            MoveDir.Normalize();
        }
        MoveDir *= length;
        Move(MoveDir.x, MoveDir.y, MoveDir.z);

        //if (!isDispatched)
        //{
        //    isDispatched = true;
        //}
        //else
        //{

        //}
    }

    //public bool CheckCollision(Vector3 dir, float length)
    //{
        //if (!EnableSelfRotate || !EnableCollision)
        //    return false;

        //Ray ray = new Ray(tracker.position, dir);
        //UBRaycastHit hit;
        //if (UBUtils.Raycast(ray, out hit) && hit.distance < length)
        //    return true;
        //else
        //    return false;
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void Move(float x, float y, float z)
    {
        MoveDir.Set(x, y, z);

        tracker.position = tracker.position + MoveDir * MoveSpeed * Time.deltaTime;
        target.position = target.position + MoveDir * MoveSpeed * Time.deltaTime;
    }

   
    public void SetRoamAble(bool value)
    {
        if (value)
        {
            EnableRotate = false;
            EnableSelfRotate = true;

            EnablePan = false;
            EnableZoom = false;
        }
        else
        {
            EnableRotate = true;
            EnableSelfRotate = false;

            EnablePan = true;
            EnableZoom = true;
            ApplyAngleFromCurrect();
        }
    }

    public void SetGravityAble(bool value)
    {
        EnableGravity = value;
    }

    public void SetCollisionAble(bool value)
    {
        EnableCollision = value;
    }

    public void SetSpeedScale(int value)
    {
        MoveSpeed = MoveSpeedBase * value;
    }

    /// <summary>
    /// 同步相机视角（标注用）
    /// </summary>
    /// <param name="cameraPos"></param>
    /// <param name="targetPos"></param>
    public void UpdateView(Vector3 cameraPos, Vector3 targetPos)
    {
        tracker.position = cameraPos;
        target.position = targetPos;
        tracker.LookAt(target);
        UpdateAngleDistanceFromTracker();
    }

    public void StartSelfRotate(float speed)
    {
        rotateSpeed = speed;
        isSelfRotate = true;
    }

    public void StopSelfRotate()
    {
        isSelfRotate = false;
    }
}
