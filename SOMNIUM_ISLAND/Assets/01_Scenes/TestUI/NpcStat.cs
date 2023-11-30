using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStat
{
    public int LikeStat;
    public int HateStat;
    public int PassionStat;
    public int DevotionStat;

    public string Type;
    public string Name;
    public Color color;
     
    public NpcStat(string name, string Type, string color, int LikeStat, int HateStat, int PassionStat, int DevotionStat)
    {
        this.Name = name;
        this.Type = Type;
        ColorUtility.TryParseHtmlString(color, out this.color);

        this.LikeStat = LikeStat;
        this.HateStat = HateStat;
        this.PassionStat = PassionStat;
        this.DevotionStat = DevotionStat;
    }
}
