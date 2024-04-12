using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyInfomatonView : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI enemyName, attackStatus, defenceStatus;
    [SerializeField] GameObject infomationPanel;
    [SerializeField] EnemyContoller contoller;


    public void InfomationUp()
    {

        infomationPanel.SetActive(true);
        var name = contoller.GetName();
        enemyName.SetText(name);
        if(name.Length > 10)
        {
            enemyName.fontSize -= (name.Length - 9) * 3;
        }
        attackStatus.SetText("AttackÅ~" +contoller.SumAttackStatus().ToString("F2"));
        defenceStatus.SetText("DefenceÅ~" + contoller.SumDefenceStatus().ToString("F2"));
    }

    public void InfomationDown()
    {


        infomationPanel.SetActive(false);
        enemyName.fontSize = 40;
    }
}
