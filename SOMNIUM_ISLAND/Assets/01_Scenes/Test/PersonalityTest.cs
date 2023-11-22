using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PersonalityTest : MonoBehaviour
{
    public List<Dictionary<string, object>> text;
    public CSVReader CSVReader;
    
    public int QuestionNum; //지금 질문이 몇번째니

    public TMP_Text Question;
    public TMP_Text[] Answer;
    public GameObject[] Btns;

    public GameObject Result_UI;
    public GameObject PersonalTest_UI;
    public GameObject TypeResult_UI;

    public GameObject[] ResultNpcSlots;

    public Sprite[] NPC_listH; // inspector 창에서 넣어줘야함
    public Sprite[] NPC_list;
    public Sprite[] Deco_Imgs;

    //------------------------------------------------------------[컬러]
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

        BtnSet(QuestionNum); //처음 세팅

        for (int i = 0; i < ResultNpcSlots.Length; i++)
        {
            ResultNpcSlots[i] = Result_UI.transform.GetChild(0).GetChild(6).GetChild(i).gameObject;
        }
        //-------------------------------------------------------------[컬러 설정]
        ColorUtility.TryParseHtmlString("#D7A347", out Sp_color);
        ColorUtility.TryParseHtmlString("#84C365", out Sum_color);
        ColorUtility.TryParseHtmlString("#C38B64", out Aut_color);
        ColorUtility.TryParseHtmlString("#64A7C3", out Win_color);
    }

    public void BtnSet(int idx_H) //텍스트 세팅
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

    public void ButtonClick(int BtnIdx) //버튼 클릭 시 텍스트 세팅
    {
        Debug.Log("textCount" + text.Count); //30개
        string selcetType = null;
        if (QuestionNum <= text.Count - 5) { //마지막 질문까지
            selcetType = (string)text[BtnIdx + QuestionNum]["Type"];
            if (QuestionNum == text.Count - 5) //마지막 질문일 때
            {
                //BtnSet(QuestionNum); // 마지막 질문 세팅 -> 테스트 결과 보여주는 함수로 체인지 
                QuestionNum += 5; // 다음질문 -> 없어용
                PersonalStat.instance.PlusStat(selcetType); //수치 증가
                TestResult();
            }
            else
            {
                PersonalStat.instance.PlusStat(selcetType); // 수치증가
                QuestionNum += 5; // 증가하고 다음 질문 세팅
                BtnSet(QuestionNum);
            }
            Debug.Log("currType : " + selcetType);
        }
        else Debug.Log("질문 끝");
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
        int[] resultArr = PersonalStat.instance.Sort(); // 순서 정렬
        int[] PersonalArr = new int[4]
        {
            PersonalStat.instance.Spring, 
            PersonalStat.instance.Summer, 
            PersonalStat.instance.Autumn, 
            PersonalStat.instance.Winter
        };
        bool[] NPCres = new bool[4] {false, false, false, false }; // 중복 체크...
        //잘맞는 타입의 친구들
        
        for (int i = 0; i < resultArr.Length; i++)
        {
            if (resultArr[i] == PersonalStat.instance.Spring && !NPCres[0]) // CSV 연동 및 NPC리스트 만들어서 추후 수정...
            {
                NPCres[0] = true;
                ResultNpcSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("곡우");
                ResultNpcSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = NPC_listH[0];

            }
            else if (resultArr[i] == PersonalStat.instance.Summer && !NPCres[1])
            {
                NPCres[1] = true;
                ResultNpcSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("소만");
                ResultNpcSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = NPC_listH[1];


            }
            else if (resultArr[i] == PersonalStat.instance.Autumn && !NPCres[2])
            {
                NPCres[2] = true;
                ResultNpcSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("추분");
                ResultNpcSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = NPC_listH[2];

            }
            else if (resultArr[i] == PersonalStat.instance.Winter && !NPCres[3])
            {
                NPCres[3] = true;
                ResultNpcSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("소설");
                ResultNpcSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = NPC_listH[3];


            }

        }
        //플레이어 타입 및 가장 잘맞는 NPC 소개
        PersonalTestResult(resultArr, PersonalArr);

    }
    void PersonalTestResult(int[] PersonalResultArr, int[] PersonalTypeStat)
    {
        for (int i = 0; i < PersonalResultArr.Length; i++)
        {
            //플레이어 타입 및 가장 잘맞는 NPC 소개
            if (PersonalResultArr[0] == PersonalTypeStat[i])
            {
                
                if (PersonalTypeStat[i] == PersonalStat.instance.Spring)
                {
                    TypeResult_UI.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = NPC_list[0];

                    TypeResult_UI.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().SetText("곡우");
                    TypeResult_UI.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().SetText("마을 토박이 청년");
                    TypeResult_UI.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>().SetText("당신은 꽃봉우리가 피어나는 계절, 봄을 닮은 사람입니다.");

                    TypeResult_UI.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().SetText("봄");
                    TypeResult_UI.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().color = Sp_color;

                    TypeResult_UI.transform.GetChild(0).GetChild(6).GetComponent<Image>().sprite = Deco_Imgs[0];
                    TypeResult_UI.transform.GetChild(0).GetChild(7).GetComponent<Image>().sprite = Deco_Imgs[0];
                }
                else if (PersonalTypeStat[i] == PersonalStat.instance.Summer)
                {
                    TypeResult_UI.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = NPC_list[1];

                    TypeResult_UI.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().SetText("소만");
                    TypeResult_UI.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().SetText("촌장의 손녀 딸");
                    TypeResult_UI.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>().SetText("당신은 따뜻한 햇볕이 감싸오는 계절, 여름을 닮은 사람입니다.");

                    TypeResult_UI.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().SetText("여름");
                    TypeResult_UI.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().color = Sum_color;

                    TypeResult_UI.transform.GetChild(0).GetChild(6).GetComponent<Image>().sprite = Deco_Imgs[1];
                    TypeResult_UI.transform.GetChild(0).GetChild(7).GetComponent<Image>().sprite = Deco_Imgs[1];
                }
                else if (PersonalTypeStat[i] == PersonalStat.instance.Autumn)
                {
                    TypeResult_UI.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = NPC_list[2];

                    TypeResult_UI.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().SetText("추분");
                    TypeResult_UI.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().SetText("잡화점의 주인");
                    TypeResult_UI.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>().SetText("당신은 곡식들이 알록달록 물들어가는 계절, 가을을 닮은 사람입니다.");

                    TypeResult_UI.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().SetText("가을");
                    TypeResult_UI.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().color = Aut_color;

                    TypeResult_UI.transform.GetChild(0).GetChild(6).GetComponent<Image>().sprite = Deco_Imgs[2];
                    TypeResult_UI.transform.GetChild(0).GetChild(7).GetComponent<Image>().sprite = Deco_Imgs[2];
                }
                else if (PersonalTypeStat[i] == PersonalStat.instance.Winter)
                {
                    TypeResult_UI.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = NPC_list[3];

                    TypeResult_UI.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().SetText("소설");
                    TypeResult_UI.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().SetText("마을 병원의 의사");
                    TypeResult_UI.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>().SetText("당신은 나무들이 새하얗게 단장하는 계절, 겨울을 닮은 사람입니다.");

                    TypeResult_UI.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().SetText("겨울");
                    TypeResult_UI.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().color = Win_color;

                    TypeResult_UI.transform.GetChild(0).GetChild(6).GetComponent<Image>().sprite = Deco_Imgs[3];
                    TypeResult_UI.transform.GetChild(0).GetChild(7).GetComponent<Image>().sprite = Deco_Imgs[3];
                }

                
            }
        }
        

    }
    
    
}
