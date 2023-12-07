using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] Text nameText, HpText ,atText, dfText, numText,rareText,lvText;
    [SerializeField] Image iconImage,rareImage;
    
    public void Show(CardModel cardModel)
    {
        iconImage.sprite = cardModel.icon;
        SetPanel(cardModel.rare);
     

       
    }
    public void GachaView(CardModel card)
    {
        iconImage.sprite = card.icon;
        rareText.text = card.rare;
        SetPanel(card.rare);
    }
    public void ChacterView(CardModel card)
    {
        iconImage.sprite = card.icon;
        lvText.text = "LV:" + card.Lv.ToString();
        numText.text = card.num.ToString();
        SetPanel(card.rare);
    }

    public void DeckEditView(CardModel card)
    {
        iconImage.sprite = card.icon;
        lvText.text = "LV:" + card.Lv.ToString();
        numText.text = card.num.ToString();
        SetPanel(card.rare);
    }
    public void CardListView(CardModel card)
    {
        iconImage.sprite = card.icon;
        numText.text = card.num.ToString();
        SetPanel(card.rare);


    }
    public void Show_Updte(CardModel cardModel)
    {

        
        numText.text = cardModel.num.ToString();
        SetPanel(cardModel.rare);

    }

    void SetPanel(string rare)
    {
        switch (rare)
        {
            case "A":
                rareImage.sprite = Resources.Load<Sprite>("A");
                
                break;
            case "S":
                rareImage.sprite = Resources.Load<Sprite>("S");
                break;
            case "SS":
                rareImage.sprite = Resources.Load<Sprite>("SS");
                break;
        }
    }
}
