using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyEntiry", menuName = "Create EnemyEnitiy")]
public class EnemyEntity :ScriptableObject
{
    public int EnemyId;
    public new string name;
    public int numba;
    public int Hp;
    public int at;
    public int df;
    public int _exp; 
    public Sprite icon;
    public List<Skill_origin> skilllist;
    public List<int> SkillTable;


}
