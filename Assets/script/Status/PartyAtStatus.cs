using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyAtStatus : MonoBehaviour
{

    int FinishTurn;
    double effect;
    string mode;
    CardModel model;

    // Update is called once per frame
    void Update()
    {

        if (GameManger.TurnNum >= FinishTurn)
        {
            StatusReset();
        }

    }

    //çUåÇóÕÉAÉbÉv
    public void SetStatus(double effect, int turn, GameObject card, string mode)
    {
        if (card == null) return;
        transform.parent = card.transform;

        model = card.GetComponent<CardController>().model;
        this.effect = effect;
        this.mode = mode;
        if (mode == "Multi")
        {
            double temp = model.at * effect;
            model.at *= (int)temp;
        }
        else if (mode == "Add")
        {
            double temp = model.at + effect;
            model.at += (int)temp;
        }
        FinishTurn = turn;
    }
    public void StatusReset()
    {
        Debug.Log("end");
        if (mode == "Multi")
        {
            double temp = model.at / effect;
            model.at = (int)temp;
        }
        else if (mode == "Add")
        {
            double temp = model.at - effect;
            model.at = (int)temp;
        }
        Destroy(gameObject);
    }
}
