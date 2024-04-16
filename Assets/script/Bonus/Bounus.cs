using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

/// <summary>
/// ボーナスの計算
/// </summary>
public class Bounus 
{
    public double Divisor_bounus(int divisors, int lv)
   {
        switch (divisors)
        {
            case 1: return lv * 0.54 + 1;
            case 2: return lv * 0.59 + 2;
            case 3: return lv * 0.67 + 3;
            case 4: return lv * 0.9 + 4;

            default: return 0;
        }
    }
    public double Multi_bounus(int multis, int lv)
   {
        switch (multis)
        {
            case 1: return lv * 0.64 + 1;
            case 2: return lv * 0.73 + 2;
            case 3: return lv * 0.82 + 3;
            case 4: return lv * 0.93 + 4;

            default: return 0;
        }
    }
    public int Prime_bounus(double primeLv)
    {
      return (int)(1 + primeLv * 0.57);
    }

}
