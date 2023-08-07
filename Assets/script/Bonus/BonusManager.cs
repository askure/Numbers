using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusManager : MonoBehaviour
{
    static GameManger gameManger;
    static string filepath;
    public Text divisorText,divisorNeedText ,multiText,multiNeedText, primeText,primeNeedText,expText;
    private int divisor, multi, prime;
    private int lvMax;

    private void Start()
    {

        filepath = Application.persistentDataPath + "/" + ".savedata.json";
        gameManger = new GameManger();
        gameManger.Dataload(filepath);
        var x = gameManger.GetBonusLv();
        divisor = x[0];
        multi = x[1];
        prime = x[2];
        SetText();
        
        
    }

    void SetText()
    {   
        
        divisorText.text = divisor.ToString()  + "/" + (gameManger.rank * 1.2).ToString("F0");
        divisorNeedText.text = "必要経験値:" + GetExp(divisor).ToString();
        multiText.text = multi.ToString() + "/" + (gameManger.rank * 1.2).ToString("F0");
        multiNeedText.text = "必要経験値:" + GetExp(multi).ToString();
        primeText.text = prime.ToString() + "/" + (gameManger.rank * 1.2).ToString("F0");
        primeNeedText.text = "必要経験値:" +  GetExp(prime).ToString();
        if (GameManger.Exp > 999999999)
        {
            expText.text = "999999999+";
        }

        else
        {
            expText.text = GameManger.Exp.ToString();
        }
    }

    public void DivisorLvUp()
    {
        
        
        if ((divisor+1) > (int) Mathf.Ceil(gameManger.rank * 1.2f))
        {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<Text>().text = "上限に達しました。上限を上げるにはランクを上げてください";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;

        }
        if (GameManger.Exp < GetExp(divisor++))
        {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<Text>().text = "EXPが足りません";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;
            
        }
       
        GameManger.Exp -= GetExp(divisor);
        gameManger.SetBonusLv(divisor, multi, prime);
        gameManger.Datasave(filepath);
        SetText();

    }

    public void MultiLvUp()
    {
        if ((multi+1) > Mathf.Ceil(gameManger.rank * 1.2f))
        {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<Text>().text = "上限に達しました。上限を上げるにはランクを上げてください";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;

        }
        if (GameManger.Exp < GetExp(multi++)) {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<Text>().text = "EXPが足りません";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;
        }
       
        GameManger.Exp -= GetExp(multi);
        gameManger.SetBonusLv(divisor, multi, prime);
        gameManger.Datasave(filepath);
        SetText();

    }

    public void PrimeLvUp()
    {
        if ((prime+1) > Mathf.Ceil(gameManger.rank * 1.2f))
        {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<Text>().text = "上限に達しました。上限を上げるにはランクを上げてください";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;

        }
        if (GameManger.Exp < GetExp(prime++))
        {

            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<Text>().text = "EXPが足りません";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;
        }
        
        GameManger.Exp -= GetExp(prime);
        gameManger.SetBonusLv(divisor, multi, prime);
        SetText();

    }
    int GetExp(int lv)
    {
        return lv * lv * 200 + ( lv + 10) * 300;
    }
}
