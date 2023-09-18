using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Arrow : MonoBehaviour
{
    public Sprite[] ArrImage_df; 
    public List<int> Answer; //현재 답 10개
    public List<Image> ListImageSpace; //이미지, 자리
    public int TargetValue; //목표치 나무 //텍스트 값 연결
    public int ArrowListCount; //몇개 할거야
    int failCount; //틀린 횟수
    Sprite[] setImages;
    bool isSucc = true;
    int current = 0;

    public TextMeshProUGUI Text;
    enum ArrowDt // 상 - 0, 하 - 1, 좌 - 2, 우 - 3
    {
        상 = 0,
        하 = 1,
        좌 = 2,
        우 = 3
    }
    void Start()
    {
        GameObject ArrowList = GameObject.Find("ArrowList");

        Answer = new List<int>(new int[10]); //답
        ListImageSpace = new List<Image>(new Image[10]); //생성될 이미지 //변할 수 있을 만한 건 list
        TargetValue = 100;
        ArrowListCount = ArrowList.transform.childCount; //있는 값 만큼 // ArrowList로 관리
        Text = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();

        for (int i = 0; i < ArrowListCount; i++) //리스트 자식 초기화
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
        //int inputKey = InputKey(); //키값 받아서 나온 값
        if (Input.anyKeyDown && current < 10 && failCount < 3)
        {
            Matching(InputKey(), Answer);
        }

    }
    // 인풋키
    void Matching(int inputKey, List<int>answer)             
    {
        
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
            if(current == 10)
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
    }
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
        int result = 0;
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
}

