using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI MaxHp, NowHp,NumPower;
    [SerializeField] Slider hpbar;
    static TextMeshProUGUI logText;
    public void Init()
    {
        var maxHp = GameManger.MAX_HP;
        var nowHp = GameManger.PartyHp;
        MaxHp.text = maxHp.ToString();
        NowHp.text = nowHp.ToString();
        hpbar.maxValue = maxHp;
        hpbar.value = nowHp;
        if(logText == null) logText = GameObject.Find("LogText").GetComponent<TextMeshProUGUI>();
        logText.text = "";
        int[] x = { 0, 0 };
        NumPowerText(x);
    }

    private void Update()
    {
        if (GameManger.PartyHp < 0) GameManger.PartyHp = 0;
        UpdateView(GameManger.PartyHp, 0, 0);
    }
    public void UpdateView(int now,int Turn,int df)
    {
        NowHp.text = now.ToString();
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
        NumPower.text = x[0].ToString();
    }
}
