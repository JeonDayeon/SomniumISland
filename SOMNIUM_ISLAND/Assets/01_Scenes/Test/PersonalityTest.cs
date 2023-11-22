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
    public GameObject TypeResult_UI;

    public GameObject[] ResultNpcSlots;

    public Sprite[] NPC_listH; // inspector â���� �־������
    public Sprite[] NPC_list;
    public Sprite[] Deco_Imgs;

    //------------------------------------------------------------[�÷�]
    Color Sp_color;
    Color Sum_color;
    Color Aut_color;
    Color Win_color;

    void Start()
    {
        text = CSVReader.Read("PersonalityTest");

        Question = GameObject.Find("Question").GetComponent<TMP_Text>();
        PersonalTest_UI = GameObject.Find("PersonalTest");
        Result_UI = GameObject.Find("Result");
        TypeResult_UI = GameObject.Find("TypeResult");

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

        for (int i = 0; i < ResultNpcSlots.Length; i++)
        {
            ResultNpcSlots[i] = Result_UI.transform.GetChild(0).GetChild(6).GetChild(i).gameObject;
        }
        //-------------------------------------------------------------[�÷� ����]
        ColorUtility.TryParseHtmlString("#D7A347", out Sp_color);
        ColorUtility.TryParseHtmlString("#84C365", out Sum_color);
        ColorUtility.TryParseHtmlString("#C38B64", out Aut_color);
        ColorUtility.TryParseHtmlString("#64A7C3", out Win_color);
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
        TypeResult_UI.transform.GetChild(0).gameObject.SetActive(false);

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
        TypeResult_UI.transform.GetChild(0).gameObject.SetActive(true);
        int[] resultArr = PersonalStat.instance.Sort(); // ���� ����
        int[] PersonalArr = new int[4]
        {
            PersonalStat.instance.Spring, 
            PersonalStat.instance.Summer, 
            PersonalStat.instance.Autumn, 
            PersonalStat.instance.Winter
        };
        bool[] NPCres = new bool[4] {false, false, false, false }; // �ߺ� üũ...
        //�߸´� Ÿ���� ģ����
        
        for (int i = 0; i < resultArr.Length; i++)
        {
            if (resultArr[i] == PersonalStat.instance.Spring && !NPCres[0]) // CSV ���� �� NPC����Ʈ ���� ���� ����...
            {
                NPCres[0] = true;
                ResultNpcSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("���");
                ResultNpcSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = NPC_listH[0];

            }
            else if (resultArr[i] == PersonalStat.instance.Summer && !NPCres[1])
            {
                NPCres[1] = true;
                ResultNpcSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("�Ҹ�");
                ResultNpcSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = NPC_listH[1];


            }
            else if (resultArr[i] == PersonalStat.instance.Autumn && !NPCres[2])
            {
                NPCres[2] = true;
                ResultNpcSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("�ߺ�");
                ResultNpcSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = NPC_listH[2];

            }
            else if (resultArr[i] == PersonalStat.instance.Winter && !NPCres[3])
            {
                NPCres[3] = true;
                ResultNpcSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("�Ҽ�");
                ResultNpcSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = NPC_listH[3];


            }

        }
        //�÷��̾� Ÿ�� �� ���� �߸´� NPC �Ұ�
        PersonalTestResult(resultArr, PersonalArr);

    }
    void PersonalTestResult(int[] PersonalResultArr, int[] PersonalTypeStat)
    {
        for (int i = 0; i < PersonalResultArr.Length; i++)
        {
            //�÷��̾� Ÿ�� �� ���� �߸´� NPC �Ұ�
            if (PersonalResultArr[0] == PersonalTypeStat[i])
            {
                
                if (PersonalTypeStat[i] == PersonalStat.instance.Spring)
                {
                    TypeResult_UI.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = NPC_list[0];

                    TypeResult_UI.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().SetText("���");
                    TypeResult_UI.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().SetText("���� ����� û��");
                    TypeResult_UI.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>().SetText("����� �ɺ��츮�� �Ǿ�� ����, ���� ���� ����Դϴ�.");

                    TypeResult_UI.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().SetText("��");
                    TypeResult_UI.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().color = Sp_color;

                    TypeResult_UI.transform.GetChild(0).GetChild(6).GetComponent<Image>().sprite = Deco_Imgs[0];
                    TypeResult_UI.transform.GetChild(0).GetChild(7).GetComponent<Image>().sprite = Deco_Imgs[0];
                }
                else if (PersonalTypeStat[i] == PersonalStat.instance.Summer)
                {
                    TypeResult_UI.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = NPC_list[1];

                    TypeResult_UI.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().SetText("�Ҹ�");
                    TypeResult_UI.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().SetText("������ �ճ� ��");
                    TypeResult_UI.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>().SetText("����� ������ �޺��� ���ο��� ����, ������ ���� ����Դϴ�.");

                    TypeResult_UI.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().SetText("����");
                    TypeResult_UI.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().color = Sum_color;

                    TypeResult_UI.transform.GetChild(0).GetChild(6).GetComponent<Image>().sprite = Deco_Imgs[1];
                    TypeResult_UI.transform.GetChild(0).GetChild(7).GetComponent<Image>().sprite = Deco_Imgs[1];
                }
                else if (PersonalTypeStat[i] == PersonalStat.instance.Autumn)
                {
                    TypeResult_UI.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = NPC_list[2];

                    TypeResult_UI.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().SetText("�ߺ�");
                    TypeResult_UI.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().SetText("��ȭ���� ����");
                    TypeResult_UI.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>().SetText("����� ��ĵ��� �˷ϴ޷� ������ ����, ������ ���� ����Դϴ�.");

                    TypeResult_UI.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().SetText("����");
                    TypeResult_UI.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().color = Aut_color;

                    TypeResult_UI.transform.GetChild(0).GetChild(6).GetComponent<Image>().sprite = Deco_Imgs[2];
                    TypeResult_UI.transform.GetChild(0).GetChild(7).GetComponent<Image>().sprite = Deco_Imgs[2];
                }
                else if (PersonalTypeStat[i] == PersonalStat.instance.Winter)
                {
                    TypeResult_UI.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = NPC_list[3];

                    TypeResult_UI.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().SetText("�Ҽ�");
                    TypeResult_UI.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().SetText("���� ������ �ǻ�");
                    TypeResult_UI.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>().SetText("����� �������� ���Ͼ�� �����ϴ� ����, �ܿ��� ���� ����Դϴ�.");

                    TypeResult_UI.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().SetText("�ܿ�");
                    TypeResult_UI.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().color = Win_color;

                    TypeResult_UI.transform.GetChild(0).GetChild(6).GetComponent<Image>().sprite = Deco_Imgs[3];
                    TypeResult_UI.transform.GetChild(0).GetChild(7).GetComponent<Image>().sprite = Deco_Imgs[3];
                }

                
            }
        }
        

    }
    
    
}
