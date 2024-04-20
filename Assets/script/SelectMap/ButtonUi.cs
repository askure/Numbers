using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonUi : MonoBehaviour
{
    TextMeshProUGUI ButtonText;
    StageEntity stage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitButton(int stageNum)
    {
        ButtonText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        stage = Resources.Load<StageEntity>("stage_prehub/Stage/" + stageNum.ToString());
        ButtonText.text = stage.stageName;
        
    }

   public  StageEntity GetStage()
    {
        return stage;
    }

  
}
