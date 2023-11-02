using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Arrow : MonoBehaviour
{
    //-----------------------------------------------------------------------------[ 변수 선언 ]
    public Sprite[] ArrImage_df; 
    public List<int> Answer; //현재 답
    public List<Image> ListImageSpace; //이미지, 자리
    public int TargetValue; //목표치 나무
    public int ArrowListCount; //몇개 할거니 //리스트개수
    int failCount; //틀린 횟수
    Sprite[] setImages;
    bool isSucc = true;
    int current = 0; //값 비교할 index 현재 값

    public TextMeshProUGUI Text; // 텍스트
    
    enum ArrowDt // 상 - 0, 하 - 1, 좌 - 2, 우 - 3
    {
        상 = 0,
        하 = 1,
        좌 = 2,
        우 = 3
    }
    int EnumLength = System.Enum.GetValues(typeof(ArrowDt)).Length;

    void Start()
    {
        //-------------------------------------------------------------------------[ 변수 초기화 ]
        GameObject ArrowList = GameObject.Find("ArrowList");

        Answer = new List<int>(new int[10]); //답
        ListImageSpace = new List<Image>(new Image[10]); //생성될 이미지 //변할 수 있을 만한 건 list
        TargetValue = 10;
        ArrowListCount = ArrowList.transform.childCount; //있는 값 만큼 - ArrowList로 관리, UI -> 이미지 접근
        Text = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();

        for (int i = 0; i < ArrowListCount; i++) //리스트 자식 불러와서 초기화
        {
            ListImageSpace[i] = ArrowList.transform.GetChild(i).gameObject.GetComponent<Image>();
            ListImageSpace[i].color = Color.black;
        }
        RandomAnswerCreate(Answer); //랜덤 값 생성
        Shuffle(Answer); // 값 섞기
        ImageAnswerCreate(Answer, ListImageSpace, ArrImage_df); //이미지 넣기

        Text.text = TargetValue.ToString(); //텍스트 초기 값
    }

    void Update()
    {
        //상하좌우 받을 때, current(index)가 10 아래, 실패횟수가 3 아래일 때, 계속 매칭되는 함수. //아무키나 받으면 안 될 거 같은데
        if (IsInput(InputKey()) && current < 10 && failCount < 3 && Time.timeScale != 0)
        {
            Matching(InputKey(), Answer);
        }
        //if (TargetValue == 0)
        //{
        //    GameEnd();
        //}

    }
    //-----------------------------------------------------------------------------[ 함수 구현 ]
    void Matching(int inputKey, List<int>answer)         
    {
        //------------------------------------------------[ 정답일 때, 오답일 때]
        if (answer[current] == inputKey)
        {
            Debug.Log("정답" + TargetValue);
            ListImageSpace[current].color = Color.yellow; //컬러값 지정
        }
        else
        {
            failCount++;
            Debug.Log("실패" + failCount);
            ListImageSpace[current].color = Color.red;
        }
        current++;
        //리셋되는 조건
        if (current == 10 || failCount >= 3) 
        {
            if(current == 10) // 10 됐을 때
            {
                TargetValue--;
                Text.text = TargetValue.ToString();
            }
            Invoke("GameReset", 1f);
        }
        Debug.Log(current);
    }
    //세팅, 랜덤값 받아서, 해당되는 이미지 출력
    void RandomAnswerCreate(List<int> Array) //답 생성
    {
        Debug.Log("랜덤엔썰크리에잇");
        for(int i = 0; i < Array.Count; i++)
        {
            Array[i] = Random.Range(0, 4); //0~3
        }
    }
    void ImageAnswerCreate(List<int> array, List<Image> ImageSpace, Sprite[] setImages) //이미지 집어넣기
    {
        for (int i = 0; i < array.Count; i++)
        {
            int number = array[i]; //랜덤 값 생성한거 저장

            if (number == (int)ArrowDt.상)
            {
                ImageSpace[i].sprite = setImages[0];
                Debug.Log("이미지 세팅" + i + "= 상");
            }
            else if (number == (int)ArrowDt.하)
            {
                ImageSpace[i].sprite = setImages[1];
                Debug.Log("이미지 세팅" + i + "= 하");

            }
            else if (number == (int)ArrowDt.좌)
            {
                ImageSpace[i].sprite = setImages[2];
                Debug.Log("이미지 세팅" + i + "= 좌");

            }
            else if (number == (int)ArrowDt.우)
            {
                ImageSpace[i].sprite = setImages[3];
                Debug.Log("이미지 세팅" + i + "= 우");
            }
            
        }
    }
    void Shuffle(List<int> list)
    {
        Debug.Log("셔플");
        int n = list.Count;
        while(n > 1)
        {
            n--;
            int k = Random.Range(0, list.Count);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    } //값 섞기
    void Shuffle(int[] list)
    {
        Debug.Log("셔플");
        int n = list.Length;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, list.Length);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    //키를 받아 값 반환
    int InputKey()
    {
        int result = EnumLength+1;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            result = (int)ArrowDt.상;
            Debug.Log("상상상 " + result);
            //Shuffle(CurrentALSet);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            result = (int)ArrowDt.하;
            Debug.Log("하" + result);

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            result = (int)ArrowDt.좌;
            Debug.Log("좌좌좌" + result);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            result = (int)ArrowDt.우;
            Debug.Log("우" + result);
        }

        return result;
    }
    bool IsInput(int function) // 인풋 받고 있니? 아무키나 받지 말고 0, 1, 2, 3 만... 이넘 갯수만큼용
    {
        if (function < EnumLength) return true;
        else return false;
    }
    void GameReset()
    {
        current = 0;
        failCount = 0;
        for (int i = 0; i < ArrowListCount; i++) //리스트 자식 초기화
        {
              ListImageSpace[i].color = Color.black;
        }
        RandomAnswerCreate(Answer); //랜덤 값 생성
        Shuffle(Answer); // 값 섞기
        ImageAnswerCreate(Answer, ListImageSpace, ArrImage_df);
    }
    public void GameEnd()
    {
        //타임 제로
        Time.timeScale = 0;
        //Debug.Log("게임엔드~~~~~~~~~~~~~~~~~~~~~~~~");
    }
}

