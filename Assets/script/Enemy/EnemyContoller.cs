using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyContoller : MonoBehaviour
{
    public  EnemyView view;
    public EnemyModel model;
    public GameObject gbj;




    private void Awake()
    {
        view = GetComponent<EnemyView>();
        gbj = this.gameObject;
       

    }

  

    public void Init(int EnemyID)
    {
        model = new EnemyModel(EnemyID);
        view.Show(model);
    }
    public void Show_update( string s,int hp,int maxHp)
    {
        view.Show_update(s,hp,maxHp);
    }

   
}
