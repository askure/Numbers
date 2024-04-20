using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI maxHp, nowHp,Numpower;
    [SerializeField] Slider hpbar;
    static TextMeshProUGUI logText;
    private void Start()
    {
        
    }
    public void Init(int max,int now,int df)
    {
        maxHp.text = max.ToString();
        nowHp.text = now.ToString();
        hpbar.maxValue = max;
        hpbar.value = now;
        if(logText == null) logText = GameObject.Find("LogText").GetComponent<TextMeshProUGUI>();
        logText.text = "";
        int[] x = { 0, 0 };
        NumPowerText(x);
    }

    private void Update()
    {
        if (GameManger.hpSum < 0) GameManger.hpSum = 0;
        updateView(GameManger.hpSum,0, 0);
    }
    public void updateView(int now,int Turn,int df)
    {
        nowHp.text = now.ToString();
        hpbar.value = now;
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
    }
}
