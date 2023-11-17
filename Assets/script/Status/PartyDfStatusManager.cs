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
    static public int statusSum = 0;
    int mystatusNum;
    string mode,cname, effecttext;
    bool flag = false;
    Transform parent;
    [SerializeField] GameObject UpDfImage,DownDfImage;
    GameObject ImageGameObject,node;
    [SerializeField]PartyDfStatus partyDf;
    StatusListManager StatusList;


    
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
       
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.gameObject.CompareTag("status")) continue;
                if (child.Find("partydf" + mystatusNum.ToString()) == null)
                {
                    var g = Instantiate(partyDf);
                    g.SetStatus(effect, FinishTurn, child.gameObject, mode);
                    g.name = "partydf" + mystatusNum.ToString();
                }
            }
            
        
       
    }


    public void SetStatusDf(double effect, int turn,string mode,string cname)
    {

        if (StatusList == null)
        {
            StatusList = GameObject.Find("StatusList").GetComponent<StatusListManager>();
        }
        this.effect = effect;
        this.mode = mode;
        this.cname = cname;
        this.FinishTurn = GameManger.TurnNum + turn;
        mystatusNum = statusSum;
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
            g.name = "partydf" + mystatusNum.ToString();
        }
        
   
        var pos = GameObject.Find("StatusList").transform;
        if(mode == "Multi" && effect >1)
        {
            ImageGameObject = UpDfImage;
            effecttext = turn.ToString() + "�^�[���̊�" + effect.ToString() + "�{����B";

        }
        if(mode == "Multi" && effect < 1)
        {
            ImageGameObject = DownDfImage;
            effecttext = turn.ToString() + "�^�[���̊�" + effect.ToString() + "�{����B";
        }
        if(mode == "Add" &&effect > 0)
        {
            ImageGameObject = UpDfImage;
            effecttext = turn.ToString() + "�^�[���̊�" + effect.ToString() + "�����₷�B";

        }
        if (mode == "Add" && effect < 0)
        {
            ImageGameObject = DownDfImage;
            effecttext = turn.ToString() + "�^�[���̊�" + effect.ToString() + "�����炷�B";
        }
        flag = true;
        statusNum++;
        statusSum++;
        index = parent.childCount - statusNum;
        StatusList.SetGameObjct(ImageGameObject);
    }

    public void SetStatusList()
    {
        StatusList.SetGameObject();
    }

    public string SkillInfo()
    {
        return "-" + cname + "-\n�h��͂�" + effecttext;
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
        Destroy(gameObject);
        
    }
}
