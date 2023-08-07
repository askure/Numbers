using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create Data Chaneger")]

[SerializeField]
public class DataChanger : ScriptableObject
{
    public int Rank;
    public int Stone;
    public int Exp;
    public int Multi;
    public int Diviser;
    public int Prime;
    public int CardLv;
    public int Hpbuf;
    public int Atbuf;
    public int Dfbuf;
    public int convex;
    public int allCharactor;
    public int allStage;
    public bool clear;
    public bool pos;
}
