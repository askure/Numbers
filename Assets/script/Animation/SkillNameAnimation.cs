using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNameAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Animator animator;
    [SerializeField] Text text;
    public int StartAniamtion(string skillname)
    {
        text.text = skillname;
        if (skillname.Length > 15)
        {
            int revision = skillname.Length - 15;
            text.fontSize = 60 - revision * 3;
        }
        else text.fontSize = 60;
        animator.Play(0);
        return animator.GetCurrentAnimatorClipInfo(0).Length;
    }
}
