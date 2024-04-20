using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyView : MonoBehaviour
{
    [SerializeField] Slider Hp;
    [SerializeField] TextMeshProUGUI numbaText;
    [SerializeField] Image iconImage;

    public void Show(EnemyModel enemyModel)
    {
       
        Hp.minValue = 0;
        numbaText.text = enemyModel.numba.ToString();
        iconImage.sprite = enemyModel.icon;

    }

    public void  Show_update(string s,int hp,int maxHp)
    {
        
        numbaText.text = s;
        Hp.maxValue = maxHp;
        Hp.value = hp;
        
    }
}
