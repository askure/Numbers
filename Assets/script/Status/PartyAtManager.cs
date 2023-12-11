using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyAtManager : MonoBehaviour
{
    int FinishTurn;
    double effect;
    static public int statusNum = 0;
    static public int statusSum = 0;
    static string path;
    int MystatusNum;
    string mode,cname,effecttext;
    bool flag = false;
    Transform parent;
    [SerializeField] GameObject UpAtImage, DownAtImage;
    GameObject ImageGameObject,node;
    [SerializeField] PartyAtStatus partyat;
    StatusListManager StatusList;


    // Update is called once per frame
    void Update()
    {

        if (GameManger.TurnNum >= FinishTurn && flag)
        {
            StatusReset();
        }
        /*if (parent.childCount > (index + statusNum) && flag)
        {
            var addCount = parent.childCount - statusNum - index;
            for (int i = parent.childCount - addCount; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.gameObject.CompareTag("status")) continue;
                Instantiate(partyat).SetStatus(effect, FinishTurn, child.gameObject, mode);
            }
        }*/
      
            
            for(int i=0; i<parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.gameObject.CompareTag("status")) continue;
                if (child.Find("partyat" + MystatusNum.ToString()) == null){
                    var g = Instantiate(partyat);
                    //Debug.Log("--ステータス番号" +MystatusNum +  "付与開始--");
                    g.SetStatus(effect, FinishTurn, child.gameObject, mode);
                    g.name = "partyat" + MystatusNum.ToString();
                    //Debug.Log("--ステータス番号" + MystatusNum + "付与完了--");

            }
            }
           
        
        


    }


    public void SetStatusAt(double effect, int turn, string mode,string cname)
    {   
        if(StatusList == null)
        {
            StatusList = GameObject.Find("StatusList").GetComponent<StatusListManager>();
        }
        

        this.effect = effect;
        this.mode = mode;
        this.cname = cname;
        this.FinishTurn = GameManger.TurnNum + turn;
        MystatusNum = statusSum;
        parent = GameObject.Find("Hand").transform;
        transform.parent = parent;
        var childCount = parent.childCount;
        
        for (int i = 0; i < childCount; i++)
        {
            var child = parent.GetChild(i);
            if (child.gameObject.CompareTag("status"))
            {
                continue;
            }
            var g = Instantiate(partyat);
            g.SetStatus(effect, FinishTurn, child.gameObject, mode);
            g.name = "partyat" + MystatusNum.ToString();
        }
        if (mode == "Multi" && effect > 1)
        {
            ImageGameObject =UpAtImage;

            effecttext =  turn.ToString() + "ターンの間"+ effect.ToString() + "倍する。";
        }
        if (mode == "Multi" && effect < 1)
        {
            ImageGameObject = DownAtImage;
            effecttext = turn.ToString() + "ターンの間" + effect.ToString() + "倍する。";
        }
        if (mode == "Add" && effect > 0)
        {
            ImageGameObject = UpAtImage;
            effecttext = turn.ToString() + "ターンの間" + effect.ToString() + "分増やす。";
        }
        if (mode == "Add" && effect < 0)
        {
            ImageGameObject = DownAtImage;
            effecttext = turn.ToString() + "ターンの間" + effect.ToString() + "分減らす。";
        }
        flag = true;
        
        statusNum++;
        statusSum++;
        StatusList.SetGameObjct(ImageGameObject);
    }

    public void SetStatusList()
    {
        StatusList.SetGameObject();
    }

    public string SkillInfo()
    {
        return "-" + cname + "-\n攻撃力を" + effecttext  ;
    }
    public void GetNode(GameObject node)
    {
        this.node = node;
    }
    public void StatusReset()
    {
        statusNum--;
        StatusList.Remove(ImageGameObject);
        Destroy(node);
        //Destroy(ImageGameObject);
        Destroy(gameObject);

    }
    
}
