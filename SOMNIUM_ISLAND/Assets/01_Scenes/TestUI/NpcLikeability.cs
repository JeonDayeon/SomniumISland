using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcLikeability : MonoBehaviour
{
    public List<Dictionary<string, object>> NpcStat_csv; //csv 기본값 설정...
    public CSVReader CSVReader;

    
    NpcStat[] npcStat;
    //PersonalityTest personalityTest;
    
    int npcNum;
    public GameObject[] NpcStatSlot;

    public Sprite[] NpcList_H; //중복...
    public TMP_Text[] NpcName;
    private void Awake()
    {
        NpcStat_csv = CSVReader.Read("NpcStat");
        npcNum = 4;
        
        npcStat = new NpcStat[npcNum];
        NpcStatSlot = new GameObject[npcNum];
        NpcName = new TMP_Text[npcNum];

        for (int i = 0; i < npcNum; i++)
        {
            npcStat[i] = new NpcStat(   (string)NpcStat_csv[i]["NPCName"],
                                        (string)NpcStat_csv[i]["Type"],
                                        (string)NpcStat_csv[i]["Color"],
                                        (int)NpcStat_csv[i]["Like"],
                                        (int)NpcStat_csv[i]["Hate"],
                                        (int)NpcStat_csv[i]["Passion"],
                                        (int)NpcStat_csv[i]["Devotion"]);

            NpcStatSlot[i] = GameObject.Find("NpcSlot_Likeability" + i);
            NpcName[i] = NpcStatSlot[i].transform.GetChild(3).GetComponent<TMP_Text>();
            NpcName[i].SetText(npcStat[i].Name);
        }
    }
    void Start()
    {

        for (int i = 0; i < npcNum; i++)
        {
           

            Debug.Log("NpcStatSlot["+ i +"] : " + NpcStatSlot[i]);
            Debug.Log("Type : " + npcStat[i].Type);
            Debug.Log("Color : " + npcStat[i].color);
            Debug.Log("LikeStat : " + npcStat[i].LikeStat);
            Debug.Log("HateStat : " + npcStat[i].HateStat);
            Debug.Log("PassionStat : " + npcStat[i].PassionStat);
            Debug.Log("DevotionStat : " + npcStat[i].DevotionStat);
            
        }
        NpcSlotSet_UI(npcStat, 0, 0, 0, 0);
    }

    void NpcSlotSet_UI(NpcStat[] NpcStat, int LikeValue, int HateValue, int PassionValue, int DevotionValue)
    {
        for(int i = 0; i < NpcStat.Length; i++)
        {
            NpcStatSlot[i].transform.GetChild(0).GetChild(0).GetComponent<Slider>().value = NpcStat[i].LikeStat + LikeValue;
            NpcStatSlot[i].transform.GetChild(0).GetChild(1).GetComponent<Slider>().value = NpcStat[i].HateStat + HateValue;
            NpcStatSlot[i].transform.GetChild(0).GetChild(2).GetComponent<Slider>().value = NpcStat[i].PassionStat + PassionValue;
            NpcStatSlot[i].transform.GetChild(0).GetChild(3).GetComponent<Slider>().value = NpcStat[i].DevotionStat + DevotionValue;
        }
    }

}
