using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    List<Dictionary<string, object>> text;
    public CSVReader CSVReader;

    public int id;
    public string TalkType;
    // Start is called before the first frame update
    void Start()
    {
    }

    void GenerateData()
    {
        if (TalkType != null)
        {
            
        }

        else
        {
            text = CSVReader.Read("CharacterGreet");
        }

        Debug.Log("���׷���Ʈ ���������׷���Ʈ ���������׷���Ʈ ���������׷���Ʈ ������");

    }

    public string GetTalk(int Tid, int talkindex, string typeName, string talkType)//�Ѱ� �� �����͸� �̱� ���� ���ڿ��� ���ϴ� ���� ����
    {
        id = Tid;
        TalkType = talkType;

        Debug.Log("����������������������������������������������������������");

        GenerateData();


        if (talkType == null)
        {
            int i = 0;
            int NPC = 0;
        
            while (NPC != id)
            {
                NPC = ((int)text[i]["ID"]);
                talkindex = i;
                i++;
            }
            Debug.Log("//////////////////////////////i =" + i);
            return ((string)text[talkindex][typeName]);
        }

        else
        {
            if (talkindex == text.Count)
            {
                return null;
            }
        
            else
            {
                return ((string)text[talkindex][typeName]);
            }
        }
    }
}
