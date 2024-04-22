using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GachaListEntity", menuName = "Create GachaListEntity")]
public class GachaListEntity : ScriptableObject
{
    public string GachaName;
    public List<CardEntity> PickUpCards;
    public int ProbabilityA;
    public CardListEntiry A;
    public int ProbabilityS;
    public CardListEntiry S;
    public int ProbabilitySS;
    public CardListEntiry SS;
}
