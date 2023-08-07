using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] Text maxHp, nowHp,TurnNum,logText,Numpower,BounusNum,NowDf;
    [SerializeField] Slider hpbar;
    public void Init(int max,int now,int df)
    {
        maxHp.text = max.ToString();
        nowHp.text = now.ToString();
        NowDf.text = "Now Df:" + df.ToString(); 
        hpbar.maxValue = max;
        hpbar.value = now;
        logText.text = "";
        int[] x = { 0, 0 };
        NumPowerText(x);
    }
    public void updateView(int now,int Turn,int df)
    {
        nowHp.text = now.ToString();
        hpbar.value = now;
        TurnNum.text = Turn.ToString();
        NowDf.text = "Now Df:" + df.ToString();
    }

    public void LogTextView(List<string> s)
    {
        logText.text = "";
        
        foreach(string log in s)
        {
            
            logText.text += log + "\n";
            
        }
       
    }

    public void NumPowerText(int[] x)
    {
        Numpower.text = x[0].ToString();
        BounusNum.text =  "(+" + x[1].ToString() +")";
    }
}
