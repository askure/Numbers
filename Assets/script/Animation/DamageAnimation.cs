using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioClip se;
    int damage;
    public void changeHp()
    {
        GameManger.PartyHp -= damage;
    }

    public int startAnimation(int damage)
    {
        this.damage = damage;
        animator.Play(0);
        return animator.GetCurrentAnimatorClipInfo(0).Length;
    }
    public void PlaySe()
    {
        BGMManager bGM = GameObject.Find("BGM").GetComponent<BGMManager>();
        bGM.PlaySE(se, 2f);
    }
}
