using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyInfomatonView : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject infomationPanel;
   
    public void InfomationUp()
    {

        infomationPanel.SetActive(true);
        var model = GetComponent<EnemyContoller>().model;
        var name = model.name;
        var beforeAt = model.initAt;
        var beforeDf = model.initDf;
        var At = model.at;
        var Df = model.df;
        infomationPanel.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
        infomationPanel.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text  =  model.Hp + "/"+ model.initHp +  " \nAttackÅ~" + ((double)At / beforeAt).ToString("F2") + "\nDefenceÅ~" + ((double)Df / beforeDf).ToString("F2");
    }

    public void InfomationDown()
    {


        infomationPanel.SetActive(false);
    }
}
