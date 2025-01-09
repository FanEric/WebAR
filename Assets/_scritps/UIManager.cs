using Imagine.WebAR;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    string[] structIDs = { "Ë®·Û»ìºÏÆ÷" };
    string[] assemblyIDs = { "»ìºÏÆ÷" };

    public Toggle kBrowseTog;
    public Toggle kInterTog;
    public Toggle kVoiceTog;

    public Button kScanBtn;
    public Button kHomeBtn;
    public Button kCloseBtn;
    public Toggle kCamTog;
    public TMP_Text kTitle;

    public GameObject kMainPanel;
    public GameObject kScanPanel;
    public GameObject kMenuPanel;
    public GameObject kTransPanel;

    public ManipulateObject kManiScript;
    public ImageTracker kTracker;
    public ARCamera kARCamera;

    public GameObject kBGPlane;

    public GameObject kStructPanel;
    public GameObject kAssemblyPanel;
    string mCurTrackedId;
    // Start is called before the first frame update
    void Start()
    {
        kBrowseTog.onValueChanged.AddListener(ToBrowse);
        kInterTog.onValueChanged.AddListener(ToInter);
        kVoiceTog.onValueChanged.AddListener(ToVoice);

        kScanBtn.onClick.AddListener(BeginScan);
        kHomeBtn.onClick.AddListener(DoHome);
        kCloseBtn.onClick.AddListener(DoHome);
        kTracker.OnImageFound.AddListener(str => { 
            DoImageFound(); 
            DoReset();
            Debug.Log("OnImageFound: " + str);
            kTitle.text = mCurTrackedId = str;
        });
        kCamTog.onValueChanged.AddListener(isOn =>
        {
            kBGPlane.SetActive(!isOn);
            //if(isOn) kARCamera.UnpauseCamera();
            //else kARCamera.PauseCamera();
        });
        DoReset();

        kMainPanel.SetActive(true);
        kScanPanel.SetActive(false);
        kMenuPanel.SetActive(false);
        kTransPanel.SetActive(false);
    }

    void BeginScan()
    {
        kMainPanel.SetActive(false);
        kScanPanel.SetActive(true);
        kMenuPanel.SetActive(false);
        kTransPanel.SetActive(false);

        kTracker.StartTracker();
    }

    void DoImageFound()
    {
        kMainPanel.SetActive(false);
        kScanPanel.SetActive(false);
        kMenuPanel.SetActive(true);
        kTransPanel.SetActive(true);
    }

    void DoHome()
    {
        kMainPanel.SetActive(true);
        kScanPanel.SetActive(false);
        kMenuPanel.SetActive(false);
        kTransPanel.SetActive(false);

        DoReset();
        kTracker.DoReset();
    }

    void ToBrowse(bool isOn)
    {
        kTracker.SetTargetChildEnable(0);

        //kTransPanel.SetActive(isOn);
    }

    void ToInter(bool isOn)
    {
        kTracker.SetTargetChildEnable(1);
        kManiScript.DoReset();

        kManiScript.doTranslate = false;
        kManiScript.doRotate = true;
        kManiScript.doScale = false;
        Debug.Log("structIDs.Contains(mCurTrackedId): " + mCurTrackedId + "--" + structIDs.Contains(mCurTrackedId));
        if (structIDs.Contains(mCurTrackedId))
        {
            kStructPanel.SetActive(true);
        }
        else if (assemblyIDs.Contains(mCurTrackedId))
        {
            kAssemblyPanel.SetActive(true);
        }
    }

    void ToVoice(bool isOn)
    {
        kTracker.SetTargetChildEnable(2);
        kManiScript.DoReset();

        kManiScript.doTranslate = false;
        kManiScript.doRotate = false;
        kManiScript.doScale = false;

    }

    void DoReset()
    {
        mCurTrackedId = "";
        kStructPanel.SetActive(false);
        kAssemblyPanel.SetActive(false);
        kBGPlane.SetActive(false);
        kBrowseTog.isOn = true;
        kManiScript.DoReset();
    }
}
