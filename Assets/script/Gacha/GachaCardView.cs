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
    public void Init(CardEntity card,bool geted)
    {
        iconImage.sprite = card.icon;
        numText.text = card.num.ToString();
        var rare = SetPanel(card.rare);
        rareImage.sprite = rare;
        rarePanel.sprite = rare;
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
    public float StartAnimation()
    {
        if (animator == null)
            return 0;
        animator.enabled = true;
        animator.Play(0);
        return animator.GetCurrentAnimatorClipInfo(0).Length+1.3f;
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
}
