using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HomeManeger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text rankeText,expText,stoneText,rankExpText;
    [SerializeField] AudioClip _intro, _loop;
    [SerializeField] SettingPanel SoundSettingPanel;
    Slider volumeslider;
    DataManager dmanager;
    CharacterDataManager cmanager;
    BGMManager BGMManager;
    string filepath,mapfilepath,cfilepath;
    float volume;
    void Start()
    {
        filepath = Application.persistentDataPath + "/" + ".savedata.json";
        mapfilepath = Application.persistentDataPath + "/" + ".savemapdata.json";
        cfilepath = Application.persistentDataPath + "/" + ".charactersavedata.json";
        dmanager = new DataManager();
        cmanager = new CharacterDataManager(cfilepath);
        if (!File.Exists(mapfilepath) || !File.Exists(filepath) || !File.Exists(cfilepath))
        {
            dmanager.DataInit(filepath);
            dmanager.MapDataInit(mapfilepath, 40, false);
            cmanager.DataInit(cfilepath);
        }
        dmanager.DataLoad(filepath);
        cmanager.Dataload(cfilepath);
        rankeText.text = dmanager.rank.ToString();
        expText.text = dmanager.Exp.ToString("N0");
        stoneText.text = dmanager.Stone.ToString();
        int rankExp = (dmanager.rank + 1) * (dmanager.rank + 1) * 100 - dmanager.rankExp;
        rankExpText.text = "ŽŸ‚Ìƒ‰ƒ“ƒN‚Ü‚Å" + rankExp.ToString("N0");
        var bgmobj = GameObject.Find("BGM");
        if(bgmobj!= null)
        {
            BGMManager =   bgmobj.GetComponent<BGMManager>();
            volume = dmanager.volume;
            BGMManager.SetBGM(_intro, _loop, dmanager.volume);
           // bgmobj.GetComponent<BGMManager>().Play();

        }


    }

    private void Update()
    {
        /*if (SoundSettingPanel.activeInHierarchy)
        {
            volume = volumeslider.value;
            BGMManager.ChangeVolume(volume);
            dmanager.volume = volume;
        }*/
    }

    public void OpenSoundeSetting()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        var g = Instantiate(SoundSettingPanel,canvas);
        g.SetUpPanel();
        
    }

    public void CloseSoundSetting()
    {
        dmanager.DataSave(filepath);
        //SoundSettingPanel.SetActive(false);
    }

}
