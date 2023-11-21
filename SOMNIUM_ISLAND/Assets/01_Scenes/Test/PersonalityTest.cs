using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PersonalityTest : MonoBehaviour
{
    public List<Dictionary<string, object>> text;
    public CSVReader CSVReader;
    
    public int QuestionNum; //���� ������ ���°��

    public TMP_Text Question;
    public TMP_Text[] Answer;

    public GameObject[] Btns;
    public GameObject Result_UI;
    public GameObject PersonalTest_UI;

    public GameObject[] ResultNpcSlots;

    public Sprite[] NPC_list; // inspector â���� �־������

    void Start()
    {
        text = CSVReader.Read("PersonalityTest");

        Question = GameObject.Find("Question").GetComponent<TMP_Text>();
        Result_UI = GameObject.Find("Result");
        PersonalTest_UI = GameObject.Find("PersonalTest");

        Answer = new TMP_Text[4];
        Btns = new GameObject[4];
        ResultNpcSlots = new GameObject[6];

        QuestionNum = 0;
        for (int i = 0; i < Btns.Length; i++)
        {
            Btns[i] = GameObject.Find("Button_" + i);
            Answer[i] = Btns[i].transform.GetChild(0).GetComponent<TMP_Text>();
        }

        BtnSet(QuestionNum); //ó�� ����

        for (int i = 0; i < ResultNpcSlots.Length; i++) // �̷��� �ص� ������
        {
            ResultNpcSlots[i] = Result_UI.transform.GetChild(0).GetChild(7).GetChild(i).gameObject;
        }

    }

    public void BtnSet(int idx_H) //�ؽ�Ʈ ����
    {
        for (int i = idx_H; i < idx_H + 5; i++)
        {
            if ((string)text[i]["Type"] == "Question")
            {
                Question.text = (string)text[i]["Content"];
            }
            Debug.Log(text[i]["Content"]);
        }
        for (int i = idx_H; i < idx_H + 4; i++)
        {
            Answer[i - idx_H].text = (string)text[i + 1]["Content"];
        }
    }

    public void ButtonClick(int BtnIdx) //��ư Ŭ�� �� �ؽ�Ʈ ����
    {
        Debug.Log("textCount" + text.Count); //30��
        string selcetType = null;
        if (QuestionNum <= text.Count - 5) { //������ ��������
            selcetType = (string)text[BtnIdx + QuestionNum]["Type"];
            if (QuestionNum == text.Count - 5) //������ ������ ��
            {
                //BtnSet(QuestionNum); // ������ ���� ���� -> �׽�Ʈ ��� �����ִ� �Լ��� ü���� 
                QuestionNum += 5; // �������� -> �����
                PersonalStat.instance.PlusStat(selcetType); //��ġ ����
                TestResult();
            }
            else
            {
                PersonalStat.instance.PlusStat(selcetType); // ��ġ����
                QuestionNum += 5; // �����ϰ� ���� ���� ����
                BtnSet(QuestionNum);
            }
            Debug.Log("currType : " + selcetType);
        }
        else Debug.Log("���� ��");
        Debug.Log("QuestionNum : " + QuestionNum);
    }

    public void TestReset()
    {
        Result_UI.transform.GetChild(0).gameObject.SetActive(false);
        PersonalTest_UI.SetActive(true);

        PersonalStat.instance.Spring = 0;
        PersonalStat.instance.Summer = 0;
        PersonalStat.instance.Autumn = 0;
        PersonalStat.instance.Winter = 0;
        QuestionNum = 0;

        BtnSet(QuestionNum);
    }

    public void TestResult()
    {
        Result_UI.transform.GetChild(0).gameObject.SetActive(true);
        PersonalTest_UI.SetActive(false);
        int[] resultArr = PersonalStat.instance.Sort(); // ���� ����
        bool[] NPCres = new bool[4] { false, false, false, false }; // �ߺ� üũ...

        for (int i = 0; i < resultArr.Length; i++)
        {
            Debug.Log(i + " = " + resultArr[i] + ",");
            if (resultArr[i] == PersonalStat.instance.Spring && !NPCres[0]) // CSV ���� �� NPC����Ʈ ���� ���� ����...
            {
                ResultNpcSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("���");
                ResultNpcSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = NPC_list[0];
                NPCres[0] = true;

            }
            else if (resultArr[i] == PersonalStat.instance.Summer && !NPCres[1])
            {
                ResultNpcSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("�Ҹ�");
                ResultNpcSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = NPC_list[1];
                NPCres[1] = true;
            }
            else if (resultArr[i] == PersonalStat.instance.Autumn && !NPCres[2])
            {
                ResultNpcSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("�ߺ�");
                ResultNpcSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = NPC_list[2];
                NPCres[2] = true;

            }
            else if (resultArr[i] == PersonalStat.instance.Winter && !NPCres[3])
            {
                ResultNpcSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("�Ҽ�");
                ResultNpcSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = NPC_list[3];
                NPCres[3] = true;
            }

        }

    }
    
}
