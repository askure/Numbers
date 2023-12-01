using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInformation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject InfomationObject;
    
    public void InfomationUp()
    {

        InfomationObject.SetActive(true);
        var model = GetComponent<CardController>().model;
        var name = model.name;
        var nameText = InfomationObject.gameObject.transform.GetChild(0).GetComponent<Text>();
        nameText.text = name;

        var HpText = InfomationObject.gameObject.transform.GetChild(1).GetComponent<Text>();
        HpText.text = "Hp:" +  model.Hp.ToString();

        var AtText = InfomationObject.gameObject.transform.GetChild(2).GetComponent<Text>();
        AtText.text =  "çUåÇóÕ:" + model.at.ToString();

        var DfText = InfomationObject.gameObject.transform.GetChild(3).GetComponent<Text>();
        DfText.text = "ñhå‰óÕ:" + model.df.ToString();

    }

    public void InfomationDown()
    {


        InfomationObject.SetActive(false);
    }


}
