using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[System.Serializable]
enum FruitType
{
    사과,
    복숭아,
    바나나
}

[System.Serializable]
struct FruitBoxList
{
    public FruitType fruit;
    public int Boxweight;
    public int wantNum;
    public int completeNum;
}

public class FlashGameManager : MonoBehaviour
{
    //------------------------------------------------------저울
    public Sprite[] FruitSprite;
    SpriteRenderer[] OnFruit;
    int FruitArrNum;//어레이 자리수

    string FirstFruitName;
    int FirstFruitNum;//과일 어떤건지 배열 자리수
    int SetOnFruitNum;//몇번째 과일 저울에 올라가는지 배열 자리수
    bool SetOnFruitBool = false;

    int Weight;
    TextMeshProUGUI ScaleText;
    //------------------------------------------------------박스
    int BoxWeight;
    TextMeshProUGUI Boxscale;

    SpriteRenderer inboxfruit;
    public Sprite[] inboxFruitSprite;
    //------------------------------------------------------목록
    [SerializeField]
    FruitBoxList[] boxlist;
    int Wi = 0;
    public int RandomListNum;

    public TextMeshProUGUI[] BoxlistText;
    //-----------------------------------------------------게임엔드
    public GameObject GameEndPanel;
    public TextMeshProUGUI CompleteNum;
    bool IsEnd = false;//게임 진행 안되게 막는 변수

    public int EndCoin = 0; //게임 진행 결과에 따른 코인

    // Start is called before the first frame update
    void Start()
    {
        GameEndPanel = GameObject.Find("Canvas").transform.GetChild(4).gameObject;
        CompleteNum = GameEndPanel.transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>();

        Boxscale = GameObject.Find("BoxScale").GetComponent<TextMeshProUGUI>();
        ScaleText = GameObject.Find("ScaleText").GetComponent<TextMeshProUGUI>();
        GameObject Fruit = GameObject.Find("Fruits").gameObject;
        FruitArrNum = Fruit.gameObject.transform.childCount;
        OnFruit = new SpriteRenderer[FruitArrNum];
        
        for(int Wi = 0; Wi < FruitArrNum; Wi++)
        {
            OnFruit[Wi] = GameObject.Find("Fruits").transform.GetChild(Wi).gameObject.GetComponent<SpriteRenderer>();
        }

        inboxfruit = GameObject.Find("Box").transform.GetChild(0).GetComponent<SpriteRenderer>();

        RandomListNum = 20;//박스 개수 따라 배열 생성
        while (RandomListNum > 0) //박스 개수 따라 배열 생성
        {
            Array.Resize(ref boxlist, Wi + 1);
            if (Wi < 3)
            {
                boxlist[Wi].wantNum = UnityEngine.Random.Range(3, 6);
            }
            else
            {
                if (boxlist.Length > 5)
                {
                    boxlist[Wi].wantNum = RandomListNum;
                }
                else
                {
                    boxlist[Wi].wantNum = UnityEngine.Random.Range(1, RandomListNum);
                }
                
            }
            RandomListNum -= boxlist[Wi].wantNum;
            Wi++;
        }

        GameObject listTxtp = GameObject.Find("GameList"); //목록 출력할 거 추가
        BoxlistText = new TextMeshProUGUI[boxlist.Length];
        for (int i = 0; i < boxlist.Length; i++)
        {
            BoxlistText[i] = listTxtp.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>();

            int num = UnityEngine.Random.Range(0, 3);

            switch(num) //각 목록 과일 종류 정하기
            {
                case 0:
                    boxlist[i].fruit = FruitType.사과;
                    break;
                case 1:
                    boxlist[i].fruit = FruitType.복숭아;
                    break;
                case 2:
                    boxlist[i].fruit = FruitType.바나나;
                    break;
            }
            int g = UnityEngine.Random.Range(100, 3000); //각 목록 무게 랜덤 정해주기
            g -= g % 100; //10자리부터 제거
            boxlist[i].Boxweight = g;//확정

            BoxlistText[i].text = "- " + boxlist[i].fruit + " " + FruitWeight(boxlist[i].Boxweight)+ " " + "(" + boxlist[i].completeNum + "/" + boxlist[i].wantNum + ")";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !IsEnd)//마우스 버튼 눌렀을 때
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero, 0f);

            if (hit.collider != null) //마우스 버튼 누른 곳에 콜라이더가 있을 때
            {
                GameObject clicks = hit.transform.gameObject;
                Debug.Log(clicks.name);

                if (Input.GetMouseButtonDown(0))//왼쪽 마우스 클릭시
                {
                    if (!SetOnFruitBool)
                    {
                        switch (clicks.name)//콜라이더 이름별로 행동 분류
                        {
                            case "AppleBox":
                                FirstFruitNum = 0;
                                SetFruit(0, clicks.name);
                                break;

                            case "PeachBox":
                                FirstFruitNum = 1;
                                SetFruit(1, clicks.name);
                                break;

                            case "BananaBox":
                                FirstFruitNum = 2;
                                SetFruit(2, clicks.name);
                                break;
                        }
                    }

                    else if (SetOnFruitNum < OnFruit.Length && clicks.name == FirstFruitName && Weight < 1500)//과일 박스 눌렀을 때
                    {
                        if (SetOnFruitNum <= 0)
                        {
                            SetFruit(FirstFruitNum, clicks.name);
                        }
                        else
                        {
                            Weight += 100;
                            ScaleText.text = FruitWeight(Weight);
                            if (Weight % 6 == 0)
                            {
                                OnFruit[SetOnFruitNum].gameObject.SetActive(true);
                                SetOnFruitNum++;
                            }
                        }
                    }

                    else
                    {
                        if (clicks.name == "Box")
                        {
                            BoxWeight += Weight;
                            Boxscale.text = FruitWeight(BoxWeight);
                            ReScale();
                            inboxfruit.sprite = inboxFruitSprite[FirstFruitNum];
                            inboxfruit.gameObject.SetActive(true);
                        }
                    }
                }

                else if(Input.GetMouseButtonDown(1))//오른쪽 마우스 클릭시 상자 넣기
                {

                    if (clicks.name == "Box")
                    {
                        if (BoxWeight != 0) //목록에 맞춰서 했는지 확인해줌
                        {

                            for (int i = 0; i < boxlist.Length; i++)
                            {
                                int fruits = (int)boxlist[i].fruit;
                                if(fruits == FirstFruitNum)
                                {
                                    if(boxlist[i].Boxweight == BoxWeight && boxlist[i].completeNum < boxlist[i].wantNum)
                                    {

                                        boxlist[i].completeNum += 1;
                                        BoxlistText[i].text = "- " + boxlist[i].fruit + " " + FruitWeight(boxlist[i].Boxweight) + " " + "(" + boxlist[i].completeNum + "/" + boxlist[i].wantNum + ")";
                                        EndCoin += BoxWeight / 100;
                                        if (boxlist[i].completeNum == boxlist[i].wantNum)
                                        {
                                            for(int j = 0; j < boxlist.Length; j++)
                                            {
                                                if(boxlist[j].completeNum != boxlist[j].wantNum)
                                                {
                                                    break;
                                                }

                                                else
                                                { 
                                                    if(j + 1 == boxlist.Length)
                                                    {
                                                        EndCoin += 300;
                                                        GameEnd();
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                Debug.Log(fruits);
                            }
                        }

                        inboxfruit.gameObject.SetActive(false);
                        BoxWeight = 0;
                        FirstFruitName = null;
                        Boxscale.text = FruitWeight(BoxWeight);
                        ReScale();
                        SetOnFruitBool = false;
                    }
                }
            }
        }
    }

    void SetFruit(int i, string name) //처음 과일 저울에 올릴 시
    {
        Weight += 100;
        ScaleText.text = FruitWeight(Weight);
        for (int j = 0; j < OnFruit.Length; j++)
        {
            OnFruit[j].sprite = FruitSprite[i];
        }
        SetOnFruitBool = true;
        FirstFruitName = name;
        OnFruit[SetOnFruitNum].gameObject.SetActive(true);
        SetOnFruitNum++;
    }

    string FruitWeight(int wet) //과일 무게 g, kg변경
    {
        if (wet < 1000)
        {
            return (wet + "g");
        }
        else
        {
            float Kg = wet / 1000f;
            return (Kg + "kg");
        }
    }
    
    void ReScale()//무게 다시 잴 때
    {
        SetOnFruitNum = 0;
        Weight = 0;
        ScaleText.text = Weight + "g";

        for(int i = 0; i < OnFruit.Length; i++)
        {
            OnFruit[i].gameObject.SetActive(false);
        }
    }

    public void GameEnd()
    {
        Time.timeScale = 0;
        IsEnd = true;
        int Cnum = 0;
        GameEndPanel.SetActive(true);
        for(int i = 0; i < boxlist.Length; i++)
        {
            Cnum += boxlist[i].completeNum;
        }
        CompleteNum.text = Cnum + "/" + 20;
    }

    public void Reward()
    {
        inventorys.coin += EndCoin;
    }
}
