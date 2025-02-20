using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AssemblyPanel : MonoBehaviour
{
    public Toggle kJobToggle;
    public Button kResetBtn;

    public GameObject kJobPanel;
    public GameObject kPartPanel;
    public Transform kJobParent;
    public Transform kPartParent;
    public GameObject kJobItem;
    public GameObject kPartItem;
    public Sprite kJobBg1;
    public Sprite kJobBg2;
    public Button kJobClose;

    public GameObject kTipObj1;
    public Button kTipOk1;

    public GameObject kTipObj2;
    public Button kTipOk2;

    public GameObject kResetObj;
    public Button kResetCancel;
    public Button kResetOk;
    public ManipulateObject kManipulateObject;
    private Animator mAnimator;
    private AnimatorCallbacks mAnimationCallback;

    List<ElementObj> mAllElements = new List<ElementObj>();
    int mCurIndex = 0;
    public static int CurIconIndex = -1;
    private bool mIsDisassemble = true;
    private PartIcon mCurPartIcon;
    public static string mAnimArgName;
    private int mAnimArgIndex = 0;
    private List<string> mAnimList = new List<string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventDispatcher<EventDef, PartIcon>.AddListener(
            EventDef.IconClicked, icon => { mCurPartIcon = icon; }
        );
        GameObject assembleObj = GameObject.FindGameObjectWithTag("Assemble");
        if (assembleObj == null) Debug.LogError("拆装模型未找到");
        mAnimArgName = assembleObj.name;
        mAnimator = assembleObj.GetComponent<Animator>();
        mAnimationCallback = mAnimator.GetComponent<AnimatorCallbacks>();
        RegisterAnimatorCallback();
        Transform trans = assembleObj.transform;
        List<Element> elements = DataSet.instance.GetAssembleData(GlobalData.CurTrackedId);
        int count = elements.Count;
        for (int i = 0; i < count; i++)
        {
            Element element = elements[i];
            Transform elemTrans = trans.Find(element.shortName);
            elemTrans.SetSiblingIndex(i);
            ElementObj elementObj = elemTrans.GetComponent<ElementObj>();
            elementObj.fullName = element.fullName;
            elementObj.shortName = element.shortName;
            elementObj.hasAnimation = element.hasAnimation;
            mAllElements.Add(elementObj);
        }

        mAllElements[mCurIndex].PreDisassemble();

        kTipOk1.onClick.AddListener(() => { 
            kTipObj1.SetActive(false);
            foreach (var item in mAllElements)
            {
                item.DoTrans(true);
            }
            mIsDisassemble = false;
            mAllElements[mCurIndex].PreAssemble();
        });

        kTipOk2.onClick.AddListener(() =>
        {
            kTipObj2.SetActive(false);
        });
        kResetBtn.onClick.AddListener(() => { kResetObj.SetActive(true); });
        kResetCancel.onClick.AddListener(() => {kResetObj.SetActive(false); });
        kResetOk.onClick.AddListener(() => {
            kResetObj.SetActive(false);
            DoReset();
            kManipulateObject.DoReset();
        });
        mAnimationCallback.OnAnimationComplete += DoAnimationComplete;
        mAnimationCallback.OnAnimationStart += DoAnimationStart;

        kJobToggle.onValueChanged.AddListener(isOn => { 
            kJobPanel.SetActive(isOn);
            kManipulateObject.canOperate = !isOn;
        });
        kJobClose.onClick.AddListener(() => { kJobToggle.isOn = false; });
    }

    private void OnEnable()
    {
        kJobPanel.SetActive(false);
    }

    void DoReset()
    {
        mIsDisassemble = true;
        mCurIndex = 0;

        foreach (var item in mAllElements)
        {
            item.DoTrans(false);
            item.DoHighlightOff();
            item.DisableCollider();
        }

        Transform trans;
        for(int i = 0; i < kJobParent.childCount; i++)
        {
            trans = kJobParent.GetChild(i);
            GameObject.Destroy(trans.gameObject);
        }
        for (int i = 0; i < kPartParent.childCount; i++)
        {
            trans = kPartParent.GetChild(i);
            GameObject.Destroy(trans.gameObject);
        }
        mAllElements[mCurIndex].PreDisassemble();
        mAnimArgIndex = 0;
        mAnimList.Clear();
    }

    public void SpawnJobItem(string jobName, string status = "")
    {
        GameObject item = Instantiate(kJobItem, kJobParent);
        Sprite sprite = kJobParent.childCount % 2 == 1 ? kJobBg1 : kJobBg2;
        item.GetComponent<JobItem>().SetData(jobName, sprite, status);
    }

    public void SpawnPartIcon(ElementObj element)
    {
        GameObject item = Instantiate(kPartItem, kPartParent);
        item.GetComponent<PartIcon>().SetData(element);
    }

    public void DoDisassemble(ElementObj element)
    {
        SpawnPartIcon(element);
        SpawnJobItem("拆卸" + element.fullName);
    }

    public void DoAssemble(string partName)
    {
        SpawnJobItem("安装" + partName);
    }

    string GetJobNameByPartName(string partName)
    {
        return partName + "任务";
    }

    private void OnDisable()
    {
        //for (int i = 0; i < kJobParent.childCount; i++)
        //{
        //    kJobParent.GetChild(i).gameObject.SetActive(false);
        //}
    }

    void RegisterAnimatorCallback()
    {
        for (int i = 0; i < mAnimator.runtimeAnimatorController.animationClips.Length; i++)
        {
            AnimationClip clip = mAnimator.runtimeAnimatorController.animationClips[i];

            AnimationEvent animationStartEvent = new AnimationEvent();
            animationStartEvent.time = 0;
            animationStartEvent.functionName = "AnimationStartHandler";
            animationStartEvent.stringParameter = clip.name;

            AnimationEvent animationEndEvent = new AnimationEvent();
            animationEndEvent.time = clip.length;
            animationEndEvent.functionName = "AnimationCompleteHandler";
            animationEndEvent.stringParameter = clip.name;

            clip.AddEvent(animationStartEvent);
            clip.AddEvent(animationEndEvent);
        }
    }

    void DoAnimationStart(string name)
    {
        if(!mIsDisassemble)
        {
            if (!mAnimList.Contains(name)) return;
            mAnimList.Remove(name);

            mCurElement.DoAssemble();
            DoAssemble(mCurElement.fullName);
            Destroy(mCurPartIcon.gameObject);
            mCurPartIcon = null;

            mCurIndex--;
            mAllElements[mCurIndex].PreAssemble();
        }
    }

    void DoAnimationComplete(string name)
    {
        
        if(mIsDisassemble)
        {
            if (mAnimList.Contains(name)) return;
            mAnimList.Add(name);

            mCurElement.DoDisassemble();
            DoDisassemble(mCurElement);
            mCurIndex++;
            mAllElements[mCurIndex].PreDisassemble();
        }
        else
        {
            
        }
    }

    int layer = 1 << 6;
    Ray mRay;
    RaycastHit mHit;
    GameObject mHitObj;
    ElementObj mCurElement;
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
                if (mHit.collider.gameObject == mHitObj)
                {
                    mCurElement = mHitObj.GetComponent<ElementObj>();
                    if(mCurElement == null) mCurElement = mHitObj.transform.parent.GetComponent<ElementObj>();
                    Transform eleTrans = mCurElement.transform;
                    if(mIsDisassemble)
                    {
                        if (mCurElement.hasAnimation)
                        {
                            //Debug.Log("播放动画： " + mCurElement.shortName);
                            //mAnimator.SetBool(mCurElement.shortName, true);
                            mAnimArgIndex++;
                            mAnimator.SetInteger(mAnimArgName, mAnimArgIndex);
                            mCurElement.DisableCollider();
                            mCurElement.DoHighlightOff();
                        }
                        else
                        {
                            mCurElement.DoDisassemble();
                            DoDisassemble(mCurElement);

                            if (mCurIndex < mAllElements.Count - 1)
                            {
                                mCurIndex++;
                                
                                mAllElements[mCurIndex].PreDisassemble();
                            }
                            else
                            {
                                kTipObj1.SetActive(true);
                            }
                        }
                    }
                    else
                    {
                        if(mCurPartIcon != null && mCurIndex == mCurPartIcon.gameObject.transform.GetSiblingIndex())
                        {
                            //Debug.Log("mCurIndex: " + mCurIndex + "mCurPartIcon：   " + mCurPartIcon.gameObject.transform.GetSiblingIndex());
                            if (mCurElement.hasAnimation)
                            {
                                mAnimArgIndex++;
                                mAnimator.SetInteger(mAnimArgName, mAnimArgIndex);
                                mCurElement.DisableCollider();
                                mCurElement.DoHighlightOff();
                            }
                            else
                            {
                                mCurElement.DoAssemble();
                                DoAssemble(mCurElement.fullName);
                                Destroy(mCurPartIcon.gameObject);
                                mCurPartIcon = null;

                                if (mCurIndex > 0)
                                {
                                    mCurIndex--;
                                    mAllElements[mCurIndex].PreAssemble();
                                }
                                else
                                {
                                    kTipObj2.SetActive(true);
                                }
                            }
                        }
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
