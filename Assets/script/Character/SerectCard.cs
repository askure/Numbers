using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SerectCard : MonoBehaviour
{   
   
   public void CardSerect()
    {
        var x = GetComponent<CardController>();
        CharacterManager.SerctedCard = x;
        CharacterManager characterManager = new CharacterManager();
        characterManager.SetText();
        
    }

    public void PopUpSerect()
    {
        var x = GetComponent<CardController>();
        CardEditManager.popupCard = x;
        CardEditManager cardEditManager = new CardEditManager();
        cardEditManager.SetPopUpcard();
    }

    public void NewDeck()
    {
        CardEditManager CardEditManager = new CardEditManager();
        CardEditManager.EditButton();
        CardEditManager.Edit = true;
        Destroy(gameObject);
    }
    public void ToDeck()
    {
        if (CardEditManager.decklistTemp.Count == 12) return;
        if (!CardEditManager.Edit) return;
        CardEditManager cardEdit = new CardEditManager();
        var x = GetComponent<CardController>();
        var y = Resources.Load<CardController>("DeckEditPrehub/DeckCard");
        gameObject.transform.Find("Serected").gameObject.SetActive(true);
        if (CardEditManager.decklistTemp.IndexOf(x.model.cardID) != -1)
        {
           
            return;
        }
        
        cardEdit.SetDeckCard(x.model.cardID, y);
        
    }

    public void DeleteDeckCard()
    {
        if (!CardEditManager.Edit) return;
       
        var x = GetComponent<CardController>();
        CardEditManager cardEdit = new CardEditManager();
        if (CardEditManager.decklistTemp.IndexOf(x.model.cardID) == -1)
        {
            
            return;
        }

        cardEdit.DeckCardDelete(x.model.cardID, x);
        Destroy(gameObject);

    }
}
