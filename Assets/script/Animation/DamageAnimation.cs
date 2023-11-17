using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    int damage;
    public void changeHp()
    {
        GameManger.hpSum -= damage;
    }

    public int startAnimation(int damage)
    {
        this.damage = damage;
        animator.Play(0);
        return animator.GetCurrentAnimatorClipInfo(0).Length;
    }
}
