using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view;
    public CardModel model;
    public GameObject gbj;

    private void Awake()
    {
        view = GetComponent<CardView>();
        gbj = this.gameObject;
        
       

    }
    public CardController(int cardId,int lv)
    {
        
        model = new CardModel(cardId, lv);
        
    }
    public void Init(int cardID,int lv)
    {
        model = new CardModel(cardID,lv);
        view.Show(model);
    }
    public void CahacterInit(int cardID,int lv)
    {
        model = new CardModel(cardID,lv);
        view.ChacterView(model);
    }
    public void DeckEdiInit(int cardID,int lv)
    {
        model = new CardModel(cardID, lv);
        view.DeckEditView(model);
    }
    public void CardlistInit(int cardID,int lv)
    {
        model = new CardModel(cardID,lv);
        view.CardListView(model);
    }
}
