using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaCardView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI numText;
    [SerializeField] Image iconImage, rareImage,rarePanel;
    [SerializeField] GameObject newText;
    [SerializeField] Animator animator;
    [SerializeField] SSAnimation ssAnimation;
    [SerializeField] AudioClip SE_S,SE_A;
    CardEntity card;
    public void Init(CardEntity card,bool geted)
    {
        iconImage.sprite = card.icon;
        numText.text = card.num.ToString();
        this.card = card;
        var rareImagetmp = SetPanel(card.rare);
        var SE = GetAudioClip(card.rare);
        GetComponent<AudioSource>().clip = SE;
        rareImage.sprite = rareImagetmp;
        rarePanel.sprite = rareImagetmp;
        if (!geted)
        {
            newText.SetActive(true);
        }
        else
        {
            newText.SetActive(false);
        }
    }
    public void Skip()
    {
        rarePanel.gameObject.SetActive(false);
    }
    public Animator StartAnimation()
    {
        Animator anim = animator;
        GachaManager.rare = card.rare;
        if (card.rare.Equals("SS")){
            if (ssAnimation == null)
                return null;
            var canvas = GameObject.Find("Canvas");
            var animation = Instantiate(ssAnimation, canvas.transform);
            iconImage.gameObject.SetActive(true);
            anim = animation.StartAnimation(card, this);
            GachaManager.stop = true;
        }
        return anim;
        
    }
    Sprite SetPanel(string rare)
    {
        return rare switch
        {
            "A" => Resources.Load<Sprite>("A"),
            "S" => Resources.Load<Sprite>("S"),
            "SS" => Resources.Load<Sprite>("SS"),
            _ => null,
        };
    }

    AudioClip GetAudioClip(string rare)
    {

        return rare switch
        {
            "A" => SE_A,
            "S" => SE_S,
            _ => null,
        };
    }
}
