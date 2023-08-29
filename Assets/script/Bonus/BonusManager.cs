using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusManager : MonoBehaviour
{
    static DataManager dataManger;
    static string filepath;
    public Text divisorText,divisorNeedText ,multiText,multiNeedText, primeText,primeNeedText,expText;
    private int divisor, multi, prime;

    private void Start()
    {

        filepath = Application.persistentDataPath + "/" + ".savedata.json";
        dataManger = new DataManager(filepath);
        divisor = dataManger.divisor_lv;
        multi = dataManger.multi_lv;
        prime = dataManger.prime_lv;
        SetText();
        
        
    }

    void SetText()
    {   
        
        divisorText.text = divisor.ToString()  + "/" + (dataManger.rank * 1.2).ToString("F0");
        divisorNeedText.text = "必要経験値:" + GetExp(divisor).ToString();
        multiText.text = multi.ToString() + "/" + (dataManger.rank * 1.2).ToString("F0");
        multiNeedText.text = "必要経験値:" + GetExp(multi).ToString();
        primeText.text = prime.ToString() + "/" + (dataManger.rank * 1.2).ToString("F0");
        primeNeedText.text = "必要経験値:" +  GetExp(prime).ToString();
        if (dataManger.Exp > 999999999)
        {
            expText.text = "999999999+";
        }

        else
        {
            expText.text = dataManger.Exp.ToString();
        }
    }

    public void DivisorLvUp()
    {
        
        
        if ((divisor+1) > (int) Mathf.Ceil(dataManger.rank * 1.2f))
        {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<Text>().text = "上限に達しました。上限を上げるにはランクを上げてください";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;

        }
        if (dataManger.Exp < GetExp(divisor++))
        {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<Text>().text = "EXPが足りません";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;
            
        }
       
        dataManger.Exp -= GetExp(divisor);
        dataManger.divisor_lv = divisor;
        dataManger.multi_lv = multi;
        dataManger.prime_lv = prime;
        dataManger.DataSave(filepath);
        SetText();

    }

    public void MultiLvUp()
    {
        if ((multi+1) > Mathf.Ceil(dataManger.rank * 1.2f))
        {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<Text>().text = "上限に達しました。上限を上げるにはランクを上げてください";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;

        }
        if (dataManger.Exp < GetExp(multi++)) {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<Text>().text = "EXPが足りません";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;
        }

        dataManger.Exp -= GetExp(divisor);
        dataManger.divisor_lv = divisor;
        dataManger.multi_lv = multi;
        dataManger.prime_lv = prime;
        dataManger.DataSave(filepath);
        SetText();

    }

    public void PrimeLvUp()
    {
        if ((prime+1) > Mathf.Ceil(dataManger.rank * 1.2f))
        {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<Text>().text = "上限に達しました。上限を上げるにはランクを上げてください";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;

        }
        if (dataManger.Exp < GetExp(prime++))
        {

            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<Text>().text = "EXPが足りません";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;
        }

        dataManger.Exp -= GetExp(divisor);
        dataManger.divisor_lv = divisor;
        dataManger.multi_lv = multi;
        dataManger.prime_lv = prime;
        dataManger.DataSave(filepath);
        SetText();

    }
    int GetExp(int lv)
    {
        return lv * lv * 200 + ( lv + 10) * 300;
    }
}
