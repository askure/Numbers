using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeManeger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI rankeText,expText,stoneText,rankExpText;
    [SerializeField] AudioClip _intro, _loop;
    [SerializeField] SettingPanel SoundSettingPanel;
    Slider volumeslider;
    BGMManager BGMManager;
    void Start()
    {

        DataManager.DataLoad();
        rankeText.text = DataManager.rank.ToString();
        expText.text = DataManager.Exp.ToString("N0");
        stoneText.text = DataManager.Stone.ToString();
        int rankExp = (DataManager.rank + 1) * (DataManager.rank + 1) * 100 - DataManager.rankExp;
        rankExpText.text = "ŽŸ‚Ìƒ‰ƒ“ƒN‚Ü‚Å" + rankExp.ToString("N0");
        var bgmobj = GameObject.Find("BGM");
        if(bgmobj!= null)
        {
            BGMManager =   bgmobj.GetComponent<BGMManager>();
            BGMManager.SetBGM(_intro, _loop, DataManager.volume);
           // bgmobj.GetComponent<BGMManager>().Play();

        }


    }


    public void OpenSoundeSetting()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        var g = Instantiate(SoundSettingPanel,canvas);
        g.SetUpPanel();
        
    }

    public void CloseSoundSetting()
    {
        DataManager.DataSave();
    }

}
