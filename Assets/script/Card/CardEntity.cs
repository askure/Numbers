using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEntiry", menuName = "Create CardEnitiy")]
public class CardEntity : ScriptableObject
{   
    public int cardID;
    public bool advent;
    public int Hp;
    public int at;
    public int df;
    public int num;
    public int firstExp;
    public int stageNum;
    public new string name;
    public  string rare;
    public Sprite  icon;
    public Skill_origin[] AutoSkill = new Skill_origin[6];
    public Skill_origin[] ReaderSkill = new Skill_origin[6];
   

}
