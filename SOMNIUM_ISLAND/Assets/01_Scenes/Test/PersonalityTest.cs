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

    public GameObject[] ResultNpcSlots;

    public Sprite[] NPC_list; // inspector 창에서 넣어줘야함

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

        BtnSet(QuestionNum); //처음 세팅

        for (int i = 0; i < ResultNpcSlots.Length; i++) // 이렇게 해도 괜찮나
        {
            ResultNpcSlots[i] = Result_UI.transform.GetChild(0).GetChild(7).GetChild(i).gameObject;
        }

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
        int[] resultArr = PersonalStat.instance.Sort(); // 순서 정렬
        bool[] NPCres = new bool[4] { false, false, false, false }; // 중복 체크...

        for (int i = 0; i < resultArr.Length; i++)
        {
            Debug.Log(i + " = " + resultArr[i] + ",");
            if (resultArr[i] == PersonalStat.instance.Spring && !NPCres[0]) // CSV 연동 및 NPC리스트 만들어서 추후 수정...
            {
                ResultNpcSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("곡우");
                ResultNpcSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = NPC_list[0];
                NPCres[0] = true;

            }
            else if (resultArr[i] == PersonalStat.instance.Summer && !NPCres[1])
            {
                ResultNpcSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("소만");
                ResultNpcSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = NPC_list[1];
                NPCres[1] = true;
            }
            else if (resultArr[i] == PersonalStat.instance.Autumn && !NPCres[2])
            {
                ResultNpcSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("추분");
                ResultNpcSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = NPC_list[2];
                NPCres[2] = true;

            }
            else if (resultArr[i] == PersonalStat.instance.Winter && !NPCres[3])
            {
                ResultNpcSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("소설");
                ResultNpcSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = NPC_list[3];
                NPCres[3] = true;
            }

        }

    }
    
}
