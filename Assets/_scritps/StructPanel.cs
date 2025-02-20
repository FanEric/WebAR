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
    public ManipulateObject kManiScript;
    public ToggleGroup kToggleGroup;
    public CanvasGroup kAssemTogCG;

    public Transform kPartContent;
    private Transform kPartTransform;
    public GameObject k3DUIObj;

    private List<PartEntity> mParts = new List<PartEntity>();

    public bool IsMono { get { return kMonoTog.isOn; } }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        kArrowAnim.SetBool("toPand", false);
        GameObject structObj = GameObject.FindGameObjectWithTag("Struct");
        if (structObj != null)
        {
            kPartTransform = structObj.transform;
            mPartAnim = structObj.GetComponent<Animator>();
            int itemCount = kPartContent.childCount;
            Transform trans = structObj.transform;
            for (int i = 0; i < trans.childCount; i++)
            {
                PartEntity entity = trans.GetChild(i).GetComponent<PartEntity>();
                mParts.Add(entity);
                if (i < itemCount)
                {
                    Transform itemTrans = kPartContent.GetChild(i);
                    itemTrans.gameObject.SetActive(true);
                    PartItem item = itemTrans.GetComponent<PartItem>();
                    entity.SetItem(item);
                    item.SetEntity(entity);
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
                kManiScript.DoReset();
                mPartAnim?.SetInteger("DoAssem", 1);
                Invoke("ShowAll3DUI", 2);
            }
        });
        kArrowTog.onValueChanged.AddListener(isOn => 
        {
            kArrowAnim.SetBool("toPand", isOn);
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
        kManiScript.DoReset(selected);
        foreach (var entity in mParts)
        { 
            entity.DoHide(entity != selected);
        }
    }

    void UndoMono(PartEntity selected)
    {
        DoReset();
        selected?.UndoMono();
        mPartAnim.enabled = true;
    }

    public void DoReset()
    {
        kAssemTog.isOn = true;
        kToggleGroup.allowSwitchOff = true;
        kManiScript.DoReset();
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

    private void OnEnable()
    {
        kAssemTog.isOn = true;
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
    public EventSystem eventSystem;
    public GraphicRaycaster graphicRaycaster;

    private void Update()
    {
        if (CheckMouseOnUI()) return;
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


    bool CheckMouseOnUI()
    {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, list);
        return list.Count > 0;
    }

}
