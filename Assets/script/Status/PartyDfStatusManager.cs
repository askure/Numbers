using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyDfStatusManager : MonoBehaviour
{
    // Start is called before the first frame update
    int FinishTurn;
    double effect;
    int index;
    static public int statusNum = 0;
    string mode;
    bool flag = false;
    Transform parent;
    [SerializeField] GameObject UpDfImage,DownDfImage;
    GameObject ImageGameObject;
    [SerializeField]PartyDfStatus partyDf;


    // Update is called once per frame
    void Update()
    {

        if (GameManger.TurnNum >= FinishTurn && flag)
        {
            StatusReset();
        }
        /*if (parent.childCount > (index+statusNum) && flag)
        {
            var addCount = parent.childCount - statusNum - index;
            for(int i = parent.childCount-addCount; i< parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.gameObject.CompareTag("status")) continue;
                Instantiate(partyDf).SetStatus(effect, FinishTurn, child.gameObject,mode);
            }
        }*/
        if (GameManger.handchange)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.gameObject.CompareTag("status")) continue;
                if (child.Find("partydf") == null)
                {
                    var g = Instantiate(partyDf);
                    g.SetStatus(effect, FinishTurn, child.gameObject, mode);
                    g.name = "partydf";
                }
            }
            
        }
       
    }


    public void SetStatusDf(double effect, int turn,string mode)
    {   
        this.effect = effect;
        this.mode = mode;
        this.FinishTurn = GameManger.TurnNum + turn;
        parent = GameObject.Find("Hand").transform;
        transform.parent = parent;
        var childCount = parent.childCount;
        for(int i=0; i<childCount; i++)
        {
            var child = parent.GetChild(i);
            if (child.gameObject.CompareTag("status")) {
                continue;
            }

            var g = Instantiate(partyDf);
            g.SetStatus(effect, FinishTurn, child.gameObject, mode);
            g.name = "partydf";
        }
        
   
        var pos = GameObject.Find("StatusList").transform;
        if(mode == "Multi" && effect >1)
        {
            ImageGameObject = Instantiate(UpDfImage, pos);
        }
        if(mode == "Multi" && effect < 1)
        {
            ImageGameObject = Instantiate(DownDfImage, pos);
        }
        if(mode == "Add" &&effect > 0)
        {
            ImageGameObject = Instantiate(UpDfImage, pos);
        }
        if(mode == "Add" && effect < 0)
        {
            ImageGameObject = Instantiate(DownDfImage, pos);
        }
        flag = true;
        statusNum++;
        index = parent.childCount - statusNum;
      
    }
    public void StatusReset()
    {
        /*for (int i = 0; i < parent.childCount; i++)
        {
        
            var child = parent.GetChild(i);
            if (child.gameObject.CompareTag("status")) continue;
            var model = child.GetComponent<CardController>().model;
        
            if (mode == "D")
                model.df -= effect;
            else if (mode == "A")
                model.at -= effect;
        }*/
        statusNum--;
        Destroy(ImageGameObject);
        Destroy(gameObject);
        
    }
}
