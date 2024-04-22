using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusManager : MonoBehaviour
{
    public TextMeshProUGUI divisorText,divisorNeedText ,multiText,multiNeedText, primeText,primeNeedText,expText,bounusInfo;
    private int divisor, multi, prime;

    private void Start()
    {
        DataManager.DataLoad();
        divisor = DataManager.divisor_lv;
        multi = DataManager.multi_lv;
        prime = DataManager.prime_lv;
        SetText();
        
        
    }

    void SetText()
    {   
        
        divisorText.text = divisor.ToString()  + "/" + ((int)Mathf.Ceil(DataManager.rank * 1.2f)).ToString("F0");
        divisorNeedText.text = "必要経験値:" + GetExp(divisor).ToString();
        multiText.text = multi.ToString() + "/" + ((int)Mathf.Ceil(DataManager.rank * 1.2f)).ToString("F0");
        multiNeedText.text = "必要経験値:" + GetExp(multi).ToString();
        primeText.text = prime.ToString() + "/" + ((int)Mathf.Ceil(DataManager.rank * 1.2f)).ToString("F0");
        primeNeedText.text = "必要経験値:" +  GetExp(prime).ToString();
        if (DataManager.Exp > 999999999)
        {
            expText.text = "999999999+";
        }

        else
        {
            expText.text = DataManager.Exp.ToString();
        }
        SetBounus();
    }

    public void DivisorLvUp()
    {
        
        
        if ((divisor+1) > (int) Mathf.Ceil(DataManager.rank * 1.2f))
        {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "上限に達しました。\n上限を上げるにはランクを上げてください";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;

        }
        if (DataManager.Exp < GetExp(divisor++))
        {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "EXPが足りません";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;
            
        }
       
        DataManager.Exp -= GetExp(divisor);
        DataManager.divisor_lv = divisor;
        DataManager.DataSave();
        SetText();


    }

    public void MultiLvUp()
    {
        if ((multi+1) > Mathf.Ceil(DataManager.rank * 1.2f))
        {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "上限に達しました。\n上限を上げるにはランクを上げてください";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;

        }
        if (DataManager.Exp < GetExp(multi++)) {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "EXPが足りません";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;
        }

        DataManager.Exp -= GetExp(multi);
        DataManager.multi_lv = multi;
        DataManager.DataSave();
        SetText();

    }

    public void PrimeLvUp()
    {
        if ((prime+1) > Mathf.Ceil(DataManager.rank * 1.2f))
        {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "上限に達しました。\n上限を上げるにはランクを上げてください";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;

        }
        if (DataManager.Exp < GetExp(prime++))
        {

            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "EXPが足りません";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;
        }

        DataManager.Exp -= GetExp(prime);
        DataManager.prime_lv = prime;
        DataManager.DataSave();
        SetText();

    }

    void SetBounus()
    {   
        Bounus bounus  = new Bounus();
        bounusInfo.text = "";
        /*約数*/
        bounusInfo.text += "約数 ";
        for(int i=1; i<5; i++)
        {
            int divbounus = (int)bounus.Divisor_bounus(i, divisor);
            bounusInfo.text += divbounus;
            if (i != 4) bounusInfo.text += " →";
        }

        /*倍数*/
        bounusInfo.text += " (順に1,2,3,4体)\n\n倍数 ";

        for (int i = 1; i < 5; i++)
        {
            int multibounus = (int)bounus.Multi_bounus(i, multi);
            bounusInfo.text += multibounus;
            if (i != 4) bounusInfo.text += " →";
        }
        /*素数*/
        int primebounus = (int)bounus.Prime_bounus(prime);
        bounusInfo.text += " (順に1,2,3,4体)\n\n素数 " + primebounus + " (4体で発動)";
        
    }

   
    int GetExp(int lv)
    {
        return lv * lv * 200 + ( lv + 10) * 300;
    }
}
