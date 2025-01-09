using UnityEngine;

public class StructPanel : MonoBehaviour
{
    public Transform kElemTips;
    private KeepModelScreenSize mKeepModelScreenSize;
    int layer = 1 << 6;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mKeepModelScreenSize = kElemTips.GetComponent<KeepModelScreenSize>();
    }

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
                mKeepModelScreenSize.SetPartName(mSelectedPart.mPartName);
                kElemTips.transform.parent.SetParent(mSelectedObj.transform, true);
                Bounds bounds = mSelectedObj.GetComponent<MeshCollider>().bounds;
                Vector3 size = bounds.size;
                kElemTips.position = bounds.center + new Vector3(0, size.y / 2f + .1f, 0);

                Debug.Log("mSelectedObj: " + mSelectedObj.name);
                mSelectedPart.DoSelect(true);
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
