using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructPanel : MonoBehaviour
{
    public static StructPanel Instance;
    private Animator mPartAnim;
    public Toggle kAssemTog;
    public Toggle kDisassTog;
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
    public Transform kPartParent;

    private List<PartEntity> mParts = new List<PartEntity>();

    public bool IsMono { get { return kMonoTog.isOn; } }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameObject structObj = GameObject.FindGameObjectWithTag("Struct");
        if (structObj != null)
        { 
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
        kDisassTog.onValueChanged.AddListener(isOn =>
        {
            kManiScript.DoReset();
            if (isOn) mPartAnim?.SetInteger("DoAssem", 1);
        });
        kAssemTog.onValueChanged.AddListener(isOn =>
        {
            if (isOn) mPartAnim?.SetInteger("DoAssem", 2);
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
                UndoMono();
            }
        });

        EventDispatcher<EventDef, PartEntity>.AddListener(EventDef.PartSelect, (entity) => {
            if (entity.mIsSelected)
            {
                if (kMonoTog.isOn)
                    DoMono(entity);
                kTransTog.isOn = entity.mIsTransparent;
                kHideTog.isOn = entity.mIsHided;
            }
            else
            {
                kTransTog.isOn = false;
                kHideTog.isOn = false;
            }
        });

        kResetBtn.onClick.AddListener(DoReset);

    }

    void SetGroupInter(bool toInter)
    {
        kAssemTogCG.alpha = toInter ? 1 : 0.5f;
        kAssemTogCG.interactable = toInter;
        kAssemTogCG.blocksRaycasts = toInter;
    }

    void DoMono(PartEntity selected)
    {
        selected.OnMono();
        Vector3 localPos = selected.transform.localPosition;
        kPartParent = selected.transform.parent;
        kPartParent.localPosition = new Vector3(-localPos.x, -selected.mPosY, selected.mPosZ);
        kManiScript.DoReset();
        foreach (var entity in mParts)
        { 
            entity.DoHide(entity != selected);
        }
    }

    void UndoMono()
    {
        DoReset();
    }

    public void DoReset()
    {
        kAssemTog.isOn = true;
        kToggleGroup.allowSwitchOff = true;
        kManiScript.DoReset();
        kMonoTog.isOn = false;

        foreach (var entity in mParts)
        {
            entity.DoSelect(false, true);
            entity.DoHide(false);
            entity.DoTrans(false);
        }
        if(kPartParent != null)
            kPartParent.localPosition = Vector3.zero;
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
    PartEntity mSelectedPart;

    private void Update()
    {
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
                    if (mSelectedPart != null)
                    {
                        Debug.Log(mSelectedPart.mPartName + " 取消选中");
                        mSelectedPart.DoSelect(false);
                    }

                    mHitObj = mHit.collider.gameObject;
                    mSelectedPart = mHitObj.GetComponent<PartEntity>();


                    Debug.Log("mSelectedObj: " + mHitObj.name);
                    mSelectedPart.DoSelect(true, true);
                }
            }
        }
    }
}
