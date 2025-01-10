using UnityEngine;
using UnityEngine.UI;

public class StructPanel : MonoBehaviour
{
    private Animator mPartAnim;
    public Toggle kAssemTog;
    public Toggle kDisassTog;
    public Animator kArrowAnim;
    public Toggle kArrowTog;

    public Transform kPartContent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
                if(i < itemCount)
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
    }

    //private void OnDisable()
    //{
    //    mPartAnim?.Play("Assembled");
    //}

    private void OnEnable()
    {
        kAssemTog.isOn = true;
    }

    int layer = 1 << 6;
    Ray mRay;
    RaycastHit mHit;
    GameObject mSelectedObj;
    PartEntity mSelectedPart;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mRay, out mHit, 1000, layer))
            {
                if (mSelectedPart != null)
                {
                    Debug.Log(mSelectedPart.mPartName + " 取消选中");
                    mSelectedPart.DoSelect(false);
                }

                mSelectedObj = mHit.collider.gameObject;
                mSelectedPart = mSelectedObj.GetComponent<PartEntity>();


                Debug.Log("mSelectedObj: " + mSelectedObj.name);
                mSelectedPart.DoSelect(true, true);
            }
            else
            {
                //if (mSelectedPart != null)
                //{
                //    mSelectedPart.DoSelect(false);
                //    mSelectedObj = null;
                //    mSelectedPart = null;
                //}
            }
        }
    }
}
