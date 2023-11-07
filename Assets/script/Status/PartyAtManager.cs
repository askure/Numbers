using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyAtManager : MonoBehaviour
{
    int FinishTurn;
    double effect;
    int index;
    static public int statusNum = 0;
    string mode;
    bool flag = false;
    Transform parent;
    [SerializeField] GameObject UpAtImage, DownAtImage;
    GameObject ImageGameObject;
    [SerializeField] PartyAtStatus partyat;


    // Update is called once per frame
    void Update()
    {

        if (GameManger.TurnNum >= FinishTurn && flag)
        {
            StatusReset();
        }
        if (parent.childCount > (index + statusNum) && flag)
        {
            var addCount = parent.childCount - statusNum - index;
            for (int i = parent.childCount - addCount; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.gameObject.CompareTag("status")) continue;
                Instantiate(partyat).SetStatus(effect, FinishTurn, child.gameObject, mode);
            }
        }


    }


    public void SetStatusAt(double effect, int turn, string mode)
    {
        this.effect = effect;
        this.mode = mode;
        this.FinishTurn = GameManger.TurnNum + turn;
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
            Instantiate(partyat).SetStatus(effect, FinishTurn, child.gameObject, mode);
        }


        var pos = GameObject.Find("StatusList").transform;
        if (mode == "Multi" && effect > 1)
        {
            ImageGameObject = Instantiate(UpAtImage, pos);
        }
        if (mode == "Multi" && effect < 1)
        {
            ImageGameObject = Instantiate(DownAtImage, pos);
        }
        if (mode == "Add" && effect > 0)
        {
            ImageGameObject = Instantiate(UpAtImage, pos);
        }
        if (mode == "Add" && effect < 0)
        {
            ImageGameObject = Instantiate(DownAtImage, pos);
        }
        flag = true;
        statusNum++;
        index = parent.childCount - statusNum;

    }
    public void StatusReset()
    {   
        statusNum--;
        Destroy(ImageGameObject);
        Destroy(gameObject);

    }
}
