using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCard : MonoBehaviour
{

    CardEditManager cardEdit;
    [SerializeField] GameObject Selected;
    public bool isTodeck;

    
    public void CardSelect()
    {   

        var x = GetComponent<CardController>();
        CharacterManager.SerctedCard = x;
        CharacterManager characterManager = new CharacterManager();
        characterManager.SetText();

    }

    public void PopUpSelect()
    {
        if(cardEdit == null)
            cardEdit = GameObject.Find("CardEditManager").GetComponent<CardEditManager>();
        var x = GetComponent<CardController>();
        CardEditManager.popupCard = x;
        cardEdit.SetPopUpcard();
    }

    public void NewDeck()
    {
        if (cardEdit == null)
            cardEdit = GameObject.Find("CardEditManager").GetComponent<CardEditManager>();
        cardEdit.EditButton();
        CardEditManager.Edit = true;
        Destroy(gameObject);
    }
    public void ToDeck()
    {
       
        if (CardEditManager.decklistTemp.Count == 12 && !CardEditManager.decklistTemp.Contains(-1)) return;
        if (!CardEditManager.Edit) return;
        if (!Input.GetMouseButtonUp(0))
            return;
        if (!isTodeck)
            return;
        var x = GetComponent<CardController>();
        if (CardEditManager.decklistTemp.IndexOf(x.model.cardID) != -1)
        {

            return;
        }
        
       
        if (cardEdit == null)
            cardEdit = GameObject.Find("CardEditManager").GetComponent<CardEditManager>();
        
        Selected.SetActive(true);
        
        cardEdit.SetDeckCard(x.model.cardID);
        StartCoroutine(Delay(false));

    }

    public void DeleteDeckCard()
    {
        if (!CardEditManager.Edit) return;
        if (!Input.GetMouseButtonUp(0))
            return;
        if (isTodeck)
            return;
        var x = GetComponent<CardController>();
        if (CardEditManager.decklistTemp.IndexOf(x.model.cardID) == -1)
        {
            return;
        }   
        if (cardEdit == null)
            cardEdit = GameObject.Find("CardEditManager").GetComponent<CardEditManager>();       
        cardEdit.DeckCardDelete(x.model.cardID);
        StartCoroutine(Delay(true));
    }

    private IEnumerator Delay(bool isTodeck)
    {
        yield return new WaitForSeconds(0.01f);
        this.isTodeck = isTodeck;
    }

  
    
    

    
    
}