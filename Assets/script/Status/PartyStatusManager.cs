using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyStatusManager : MonoBehaviour
{
    // Start is called before the first frame update
    int EffectTurn = 3;
    int FinishTurn;
    int effect;
    int index;
    static public int statusNum = 0;
    string mode;
    bool flag = false;
    Transform parent;
    [SerializeField] GameObject UpAtImage, UpDfImage, DownAtImage,DownDfImage;
    GameObject ImageGameObject;


    // Update is called once per frame
    void Update()
    {

        if (GameManger.TurnNum >= FinishTurn && flag)
        {
            StatusReset();
        }
        if (parent.childCount > (index+statusNum) && flag)
        {
            var addCount = parent.childCount - statusNum - index;
            for(int i = parent.childCount-addCount; i< parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.gameObject.CompareTag("status")) continue;
                var model = child.GetComponent<CardController>().model;
               
                if (mode == "D")
                    model.df += effect;
                else if (mode == "A")
                    model.at += effect;
            }
        }
        
       
    }


    public void SetStatus(int effect, int turn,string mode)
    {   
        this.effect = effect;
        this.mode = mode;
        parent = GameObject.Find("Hand").transform;
        transform.parent = parent;
        var childCount = parent.childCount;
        for(int i=0; i<childCount; i++)
        {
            var child = parent.GetChild(i);
            if (child.gameObject.CompareTag("status")) {
                continue;
            } 
            var  model = child.GetComponent<CardController>().model;
            
            if (mode == "D")
                model.df += effect;
            else if (mode == "A")
                model.at += effect;
        }
        
        EffectTurn = turn;
        FinishTurn = GameManger.TurnNum + EffectTurn;
        var pos = GameObject.Find("StatusList").transform;
        if(mode == "D" && effect > 0)
        {
            ImageGameObject = Instantiate(UpDfImage, pos);
        }
        if(mode == "D" && effect < 0)
        {
            ImageGameObject = Instantiate(DownDfImage, pos);
        }
        if(mode == "A" &&effect > 0)
        {
            ImageGameObject = Instantiate(UpAtImage, pos);
        }
        if(mode == "A" && effect < 0)
        {
            ImageGameObject = Instantiate(DownAtImage, pos);
        }
        Debug.Log(effect);
        flag = true;
        statusNum++;
        index = parent.childCount - statusNum;
      
    }
    public void StatusReset()
    {
        for (int i = 0; i < parent.childCount; i++)
        {
        
            var child = parent.GetChild(i);
            if (child.gameObject.CompareTag("status")) continue;
            var model = child.GetComponent<CardController>().model;
        
            if (mode == "D")
                model.df -= effect;
            else if (mode == "A")
                model.at -= effect;
        }
        statusNum--;
        Destroy(ImageGameObject);
        Destroy(gameObject);
        
    }
}
