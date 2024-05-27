using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SSAnimation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PopUpNum,CardNum,CardName;
    [SerializeField] Image icon;
    [SerializeField] Animator animator;
    GachaCardView gachaCardView;

    public Animator StartAnimation(CardEntity card,GachaCardView gachaCardView)
    {   

        PopUpNum.SetText(card.num.ToString());
        CardName.SetText(card.name.ToString());
        CardNum.SetText(card.num.ToString());
        icon.sprite = card.icon;
        this.gachaCardView = gachaCardView;
        return animator;
    }

    public void EndAnimation()
    {   
        var status = animator.GetCurrentAnimatorStateInfo(0);
        if (status.normalizedTime < 1)
        {
            AnimationSkip.Skip(animator);
            return;
        }  
        GachaManager.stop = false;
        gachaCardView.Skip();
        Destroy(gameObject);
    }
}
