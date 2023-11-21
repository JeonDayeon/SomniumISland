using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalStat : MonoBehaviour
{
    public static PersonalStat instance;

    public int Spring;
    public int Summer;
    public int Autumn;
    public int Winter;
    public string currPersonalType;
    int plusStat = 10;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currPersonalType = null;
    }

    public void PlusStat(string PersonalType)
    {
        if (PersonalType == "Spring")
        {
            Spring += plusStat;
            Summer += plusStat / 4;
            Autumn -= plusStat / 3;
            Winter += plusStat / 4;

        }
        else if (PersonalType == "Summer")
        {
            Spring += plusStat / 4;
            Summer += plusStat;
            Autumn += plusStat / 4;
            Winter -= plusStat / 3;
        }
        else if (PersonalType == "Autumn")
        {
            Spring -= plusStat / 3;
            Summer += plusStat / 4;
            Autumn += plusStat;
            Winter += plusStat / 4;
        }
        else if (PersonalType == "Winter")
        {
            Spring += plusStat / 4;
            Summer -= plusStat / 3;
            Autumn += plusStat / 4;
            Winter += plusStat;
        }
        else
        {
            Debug.Log("Type : NULL");
        }
    }

    public int[] Sort()
    {
        int[] arr = new int[4]{ Spring, Summer, Autumn, Winter }; // ¿Œµ¶Ω∫
        int temp;
        Debug.Log("arr.Length" + arr.Length);
        for(int i = 0; i < arr.Length; i++)
        {
            for(int j = i + 1; j < arr.Length; j++)
            {
                if (arr[i] < arr[j])
                {
                    temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }
            
        }
        return arr;
    }

}
