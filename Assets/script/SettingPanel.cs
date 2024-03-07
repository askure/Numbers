using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    // Start is called before the first frame update
    DataManager dataManager;
    Slider volumeslider;
    static string dfilepath;
    static float volume;
    static BGMManager BGMManager;
    [SerializeField] GameObject TutorialPanel,strength, skill, status;
    private void Update()
    {
        volume = volumeslider.value;
        BGMManager.ChangeVolume(volume);
        dataManager.volume = volume;
    }
    public void SetUpPanel()
    {
        if (BGMManager == null) BGMManager = GameObject.Find("BGM").GetComponent<BGMManager>();
        if (dfilepath == null) dfilepath = Application.persistentDataPath + "/" + ".savedata.json"; ;
        if (volumeslider == null) volumeslider = GameObject.Find("BGMSlider").GetComponent<Slider>();
        dataManager = new DataManager();
        dataManager.DataLoad(dfilepath);
        volumeslider.value = dataManager.volume;
    }

   public void ClosePanel()
    {
        dataManager.DataSave(dfilepath);
        Destroy(gameObject);
    }

    public void OpenTutorialPanel()
    {
        TutorialPanel.SetActive(true);
        if (!dataManager.status_tutorial)
        {
            status.SetActive(false);
        }
        if (!dataManager.enemystatus_tutorial)
        {
            strength.SetActive(false);
        }
        if (!dataManager.charactor_tutorial)
        {
            skill.SetActive(false);
        }
    }
    public void CloseTutorialPanel()
    {
        TutorialPanel.SetActive(false);
    }


}
