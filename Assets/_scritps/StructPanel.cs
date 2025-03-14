using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StructPanel : MonoBehaviour
{
    public static StructPanel Instance;
    private Animator mPartAnim;
    public Toggle kAssemTog;
    public Animator kArrowAnim;
    public Toggle kArrowTog;
    public Toggle kTransTog;
    public Toggle kHideTog;
    public Toggle kMonoTog;
    public Button kResetBtn;
    public ToggleGroup kToggleGroup;
    public CanvasGroup kAssemTogCG;

    public Transform kPartContent;
    private Transform kPartTransform;
    public GameObject k3DUIObj;

    private List<PartEntity> mParts = new List<PartEntity>();

    public bool IsMono { get { return kMonoTog.isOn; } }

    private GameObject mQDDJObj;
    private Animator mQDDJAnim;
    private List<PartEntity> mQDDJParts = new List<PartEntity>();
    private GameObject mBYDQ_CK;
    private GameObject mBYDQ_Other;
    private GameObject structObj;

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        kArrowAnim.SetBool("toPand", true);
    }

    void Start()
    {
        Debug.Log("StructPanel---Start");
        kArrowAnim.SetBool("toPand", true);
        structObj = GameObject.FindGameObjectWithTag("Struct");
        if (structObj != null)
        {
            kPartTransform = structObj.transform;
            mPartAnim = structObj.GetComponent<Animator>();
            int itemCount = kPartContent.childCount;
            Transform trans = structObj.transform;
            for (int i = 0; i < trans.childCount; i++)
            {
                PartEntity entity = trans.GetChild(i).GetComponent<PartEntity>();
                if (entity != null)
                {
                    mParts.Add(entity);
                    if (i < itemCount)
                    {
                        Transform itemTrans = kPartContent.GetChild(i);
                        itemTrans.gameObject.SetActive(true);
                        PartItem item = itemTrans.GetComponent<PartItem>();
                        if (item != null)
                        {
                            entity.SetItem(item);
                            item.SetEntity(entity);
                        }
                    }
                }
            }
                
            if(structObj.name == "S_ZCGJBJ")
            {
                mQDDJObj = structObj.transform.parent.GetChild(2).gameObject;
                mBYDQ_CK = GameObject.Find("BYDQ_CK");
                mBYDQ_Other = GameObject.Find("BYDQ_Other");
                if (mQDDJObj != null)
                {
                    mQDDJAnim = mQDDJObj.GetComponent<Animator>();
                    Transform trans2 = mQDDJObj.transform;
                    for (int i = 0; i < trans2.childCount; i++)
                    {
                        PartEntity en = trans2.GetChild(i).GetComponent<PartEntity>();
                        if (en != null)
                        {
                            mQDDJParts.Add(en);

                        }
                    }
                }
            }
        }
        
        kAssemTog.onValueChanged.AddListener(isOn =>
        {
            kArrowTog.isOn = false;
            if (isOn)
            {
                HideAll3DUI();
                mPartAnim?.SetInteger("DoAssem", 2);
            }
            else
            {
                PartEntity entity = GetSelectedEntity();
                entity?.DoSelect(false, true);
                ManipulateObject.instance.DoReset();
                mPartAnim?.SetInteger("DoAssem", 1);
                Invoke("ShowAll3DUI", 2);
            }
        });
        kArrowTog.onValueChanged.AddListener(isOn => 
        {
            kArrowAnim.SetBool("toPand", !isOn);
        });

        kTransTog.onValueChanged.AddListener(isOn => {
            PartEntity entity = GetSelectedEntity();
            if (entity == null)
                kTransTog.isOn = false;
            else
                entity.DoTrans(isOn);
        });

        kHideTog.onValueChanged.AddListener(isOn => {
            PartEntity entity = GetSelectedEntity();
            if (entity == null)
                kHideTog.isOn = false;
            else
                entity.DoHide(isOn);
        });

        kMonoTog.onValueChanged.AddListener(isOn => {
            kArrowTog.isOn = false;
            PartEntity entity = GetSelectedEntity();
            kToggleGroup.allowSwitchOff = !isOn;
            SetGroupInter(!isOn);
            if (isOn)
            {
                if (entity != null)
                    DoMono(entity);
            }
            else
            {
                UndoMono(entity);
            }
        });

        EventDispatcher<EventDef, PartEntity>.AddListener(EventDef.PartSelect, (entity) => {
            HideAll3DUI();
            if (entity.mIsSelected)
            {
                if (kMonoTog.isOn)
                    DoMono(entity);
                kTransTog.isOn = entity.mIsTransparent;
                kHideTog.isOn = entity.mIsHided;
            }
            else
            {
                entity.UndoMono();
                kTransTog.isOn = false;
                kHideTog.isOn = false;
            }
        });

        //EventDispatcher<EventDef, string>.AddListener(EventDef.DoFocus, (str) => { DoFocus(); });

        kResetBtn.onClick.AddListener(DoReset);
    }

    void ShowAll3DUI()
    {
        foreach (var item in mParts)
            item.Show3DUI();
    }

    void HideAll3DUI()
    {
        foreach (var item in mParts)
            item.Hide3DUI();
    }



    void SetGroupInter(bool toInter)
    {
        kAssemTogCG.alpha = toInter ? 1 : 0.5f;
        kAssemTogCG.interactable = toInter;
        kAssemTogCG.blocksRaycasts = toInter;
    }

    void DoMono(PartEntity selected)
    {
        mPartAnim.enabled = false;
        StartCoroutine(selected.DoMono());
        ManipulateObject.instance.DoReset(selected);
        foreach (var entity in mParts)
        { 
            entity.DoHide(entity != selected);
        }

        if (mQDDJObj != null && mQDDJAnim != null)
        {
            if (selected.mPartName == "驱动电机")
            {
                kPartTransform.gameObject.SetActive(false);
                mQDDJObj.SetActive(true);
                mQDDJAnim.SetInteger("DoAssem", 1);
                Invoke("ShowQDDJ3DUI", 2);
            }
            else
            {
                kPartTransform.gameObject.SetActive(true);
                mQDDJObj.SetActive(false);
                mQDDJAnim.SetInteger("DoAssem", 2);
                HideQDDJ3DUI();
            }
        }
        mBYDQ_CK?.SetActive(false);
        mBYDQ_Other?.SetActive(false);
    }

    void ShowQDDJ3DUI()
    {
        foreach (var item in mQDDJParts)
            item.Show3DUI();
    }

    void HideQDDJ3DUI()
    {
        foreach (var item in mQDDJParts)
            item.Hide3DUI();
    }

    void UndoMono(PartEntity selected)
    {
        DoReset();
        selected?.UndoMono();
        mPartAnim.enabled = true;
    }

    PartEntity mLastSelected;
    void DoFocus()
    { 
        PartEntity part = GetSelectedEntity();
        ManipulateObject.instance.ResetPos(part);
        if (part != null)
        {
            if(mLastSelected != part)
                structObj.transform.localPosition += part.GetInverseLocalPos();
            mLastSelected = part;
        }
        else
        {
            structObj.transform.position = Vector3.zero;
        }
    }

    public void DoReset()
    {
        kAssemTog.isOn = true;
        kToggleGroup.allowSwitchOff = true;
        ManipulateObject.instance.DoReset();
        kMonoTog.isOn = false;
        kArrowTog.isOn = false;

        foreach (var entity in mParts)
        {
            entity.DoSelect(false, true);
            entity.DoHide(false);
            entity.DoTrans(false);
        }
        
        mHitObj = null;
        mSelectedPart = null;

        if (mQDDJObj != null && mQDDJAnim != null)
        {
            kPartTransform.gameObject.SetActive(true);
            mQDDJObj.SetActive(false);
            mQDDJAnim.SetInteger("DoAssem", 2);
            HideQDDJ3DUI();
        }
        mBYDQ_CK?.SetActive(true);
        mBYDQ_Other?.SetActive(true);
    }

    PartEntity GetSelectedEntity()
    {
        foreach (var entity in mParts)
        {
            if (entity.mIsSelected)
                return entity;
        }
        return null;
    }


    private void OnDisable()
    {
        DoReset();
    }

    int layer = 1 << 6;
    Ray mRay;
    RaycastHit mHit;
    GameObject mHitObj;
    GameObject mLastHitObj;
    PartEntity mSelectedPart;

    private void Update()
    {
        if (IsMono) { return; }
        if (ManipulateObject.instance.CheckMouseOnUI()) return;
        if (Input.GetMouseButtonDown(0))
        {
            mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mRay, out mHit, 1000, layer))
            {
                mHitObj = mHit.collider.gameObject;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mRay, out mHit, 1000, layer))
            {
                if(mHit.collider.gameObject == mHitObj)
                {
                    HideAll3DUI();
                    if (mSelectedPart != null)
                    {
                        Debug.Log(mSelectedPart.mPartName + " 取消选中");
                        mSelectedPart.DoSelect(false, true);
                    }

                    if(mLastHitObj !=  mHitObj)
                    {
                        mHitObj = mHit.collider.gameObject;
                        mSelectedPart = mHitObj.GetComponent<PartEntity>();
                        if(mSelectedPart == null)
                            mSelectedPart = mHitObj.transform.parent.GetComponent<PartEntity>();

                        Debug.Log("mSelectedObj: " + mHitObj.name);
                        mSelectedPart.DoSelect(true, true);
                        mLastHitObj = mHitObj;
                    }
                    else
                    {
                        mSelectedPart = null;
                        mLastHitObj = null;
                        mHitObj = null;
                    }
                }
            }
        }
    }

}
