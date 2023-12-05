using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNameAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Animator animator;
    [SerializeField] Text text;
    [SerializeField] AudioClip se;
    static BGMManager bgm;
    
    public int StartAniamtion(string skillname)
    {
        text.text = skillname;
        if (skillname.Length > 15)
        {
            int revision = skillname.Length - 15;
            text.fontSize = 60 - revision * 3;
        }
        else text.fontSize= 60;
        animator.Play(0);
        if (bgm == null) bgm = GameObject.Find("BGM").GetComponent<BGMManager>();
        return animator.GetCurrentAnimatorClipInfo(0).Length;
    }

    public void PlaySe()
    {
        if(bgm!=null){
            bgm.PlaySE(se);
        }

    }
}
