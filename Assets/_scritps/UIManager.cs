using Imagine.WebAR;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    string[] structIDs = { "水粉混合器" };
    string[] assemblyIDs = { "减速机构" };

    public Toggle kBrowseTog;
    public Toggle kInterTog;
    public Toggle kVoiceTog;

    public Button kScanBtn;
    public Button kHomeBtn;
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
    public GameObject kTestObj1;
    public GameObject kTestObj2;

    public AudioSource kAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        //DataSet.instance.GetAssembleData("减速机构");
        kBrowseTog.onValueChanged.AddListener(ToBrowse);
        kInterTog.onValueChanged.AddListener(ToInter);
        kVoiceTog.onValueChanged.AddListener(ToVoice);

        kScanBtn.onClick.AddListener(BeginScan);
        kHomeBtn.onClick.AddListener(DoHome);
        kTracker.OnImageFound.AddListener(str => { 
            DoImageFound(); 
            DoReset();
            kTitle.text = GlobalData.CurTrackedId = str;
            kAudioSource.clip = DataSet.instance.audios[GlobalData.CurTrackedId][0];
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

        BeginScan();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TestStruct();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TestAssemble();
        }
    }

    void TestStruct()
    {
        GlobalData.CurTrackedId = Const.SFHEQ;
        kTestObj1.SetActive(true );
        //kInterTog.isOn = true;
        kTestObj1.transform.GetChild(0).gameObject.SetActive(false);
        kTestObj1.transform.GetChild(1).gameObject.SetActive(true);
        kStructPanel.SetActive(true);
        kMenuPanel.SetActive(true);
        kTransPanel.SetActive(true);
        kScanPanel.SetActive(false);
    }

    void TestAssemble()
    {
        GlobalData.CurTrackedId = Const.JSJG;
        Debug.Log("kTestObj2 == null: " + kTestObj2 == null);
        kTestObj2.SetActive(true);
        //kInterTog.isOn = true;
        kTestObj2.transform.GetChild(0).gameObject.SetActive(false);
        kTestObj2.transform.GetChild(1).gameObject.SetActive(true);
        kAssemblyPanel.SetActive(true);
        kMenuPanel.SetActive(true);
        kTransPanel.SetActive(true);
        kScanPanel.SetActive(false);
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
        kMainPanel.SetActive(false);
        kScanPanel.SetActive(true);
        kMenuPanel.SetActive(false);
        kTransPanel.SetActive(false);

        DoReset();
        kTracker.DoReset();

        BeginScan();

    }

    void ToBrowse(bool isOn)
    {
        if (string.IsNullOrEmpty(GlobalData.CurTrackedId)) return;
        if(isOn)
        {
            kManiScript.DoReset();
            kTracker.SetTargetChildEnable(0);
            kVoiceTog.isOn = false;
            kAudioSource.clip = DataSet.instance.audios[GlobalData.CurTrackedId][0];
        }
    }

    void ToInter(bool isOn)
    {
        if (string.IsNullOrEmpty(GlobalData.CurTrackedId)) return;
        if (isOn)
        {
            kTracker.SetTargetChildEnable(1);
            kManiScript.DoReset();

            if (structIDs.Contains(GlobalData.CurTrackedId))
            {
                kStructPanel.SetActive(true);
                Debug.Log("结构展示： " + GlobalData.CurTrackedId);
            }
            else if (assemblyIDs.Contains(GlobalData.CurTrackedId))
            {
                Debug.Log("拆装： " + GlobalData.CurTrackedId);
                kAssemblyPanel.SetActive(true);
            }
            kVoiceTog.isOn = false;
            kAudioSource.clip = DataSet.instance.audios[GlobalData.CurTrackedId][1];
        }
        else
        { 
            kStructPanel.SetActive(false);
            kAssemblyPanel.SetActive(false);
        }
    }

    void ToVoice(bool isOn)
    {
        if (string.IsNullOrEmpty(GlobalData.CurTrackedId)) return;
        if (isOn)
        {
            kAudioSource.Play();
            //kTracker.SetTargetChildEnable(2);
            //kManiScript.DoReset();

            //kManiScript.doTranslate = false;
            //kManiScript.doRotate = false;
            //kManiScript.doScale = false;
        }
        else
        {
            kAudioSource.Stop();
        }
    }

    void DoReset()
    {
        Debug.Log("DoReset");
        GlobalData.CurTrackedId = "";
        kStructPanel.SetActive(false);
        kAssemblyPanel.SetActive(false);
        kBGPlane.SetActive(false);
        kBrowseTog.isOn = true;
        kManiScript.DoReset();

        kVoiceTog.isOn = false;
    }
}
