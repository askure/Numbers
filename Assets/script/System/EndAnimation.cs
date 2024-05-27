using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnimation : MonoBehaviour
{
    public void ButtleAnimatoinEnd()
    {
        GameManger x = GameObject.Find("GameManager").GetComponent<GameManger>();
        x.EnemyEntryAnimation();
        Destroy(gameObject);
    }
}
