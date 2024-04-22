using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    // Start is called before the first frame update
    Slider volumeslider;
    static float volume;
    static BGMManager BGMManager;
    [SerializeField] GameObject TutorialPanel,strength, skill, status;
    private void Update()
    {
        volume = volumeslider.value;
        BGMManager.ChangeVolume(volume);
        DataManager.volume = volume;
    }
    public void SetUpPanel()
    {
        if (BGMManager == null) BGMManager = GameObject.Find("BGM").GetComponent<BGMManager>();
        if (volumeslider == null) volumeslider = GameObject.Find("BGMSlider").GetComponent<Slider>();
        volumeslider.value = DataManager.volume;
    }

   public void ClosePanel()
    {
        DataManager.DataSave();
        Destroy(gameObject);
    }

    public void OpenTutorialPanel()
    {
        TutorialPanel.SetActive(true);
        if (!DataManager.status_tutorial)
        {
            status.SetActive(false);
        }
        if (!DataManager.enemystatus_tutorial)
        {
            strength.SetActive(false);
        }
        if (!DataManager.charactor_tutorial)
        {
            skill.SetActive(false);
        }
    }
    public void CloseTutorialPanel()
    {
        TutorialPanel.SetActive(false);
    }


}
