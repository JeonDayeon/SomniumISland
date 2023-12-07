using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TokenCreate : MonoBehaviour
{
    public List<GameObject> TokenList;
    public List<GameObject> PositionList;
    public GameObject TokenPrefab;

    public List<Sprite> SpriteList; //이미지

    int Count;
    int TokenPoint;
    int Score; // scoreSet연결

    public TextMeshProUGUI ScoreTxt;
    public enum TokenColor
    {
        Pich,
        Apple,
        Bannana
    }

    //-----------------------------------------------------게임엔드
    public GameObject GameEndPanel;
    public TextMeshProUGUI CompleteNum;
    bool IsEnd = false;//게임 진행 안되게 막는 변수

    public int EndCoin = 0; //게임 진행 결과에 따른 코인

    void Start()
    {
        GameEndPanel = GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        CompleteNum = GameEndPanel.transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>();

        ScoreTxt = GameObject.Find("ScoreText").transform.GetComponent<TextMeshProUGUI>();

        Count = 0;
        TokenPoint = 1;
        Score = 0;
        StartTokenInit();

        ScoreTxt.text = Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        TokenMove();
    }
    //생성
    public void Create()
    {
        GameObject token = Instantiate(TokenPrefab, PositionList[0].transform.position, Quaternion.identity); //0에 생성
        TokenSet tokenSet = token.GetComponent<TokenSet>();
        tokenSet.spriterRenderer.sprite = SpriteList[(int)tokenSet.TokenType]; //복숭아, 사과, 바나나순서로 0 1 2
        if(Count >= 4)
        {
            TokenList[Count] = token; //마지막 토큰
            Count = 0; //카운트 초기화
        }
        else
        {
            TokenList[Count] = token; //토큰리스트에 토큰 추가
            Count++;
        }
        tokenSet.tokenTurn = 0; // 여기서 0으로 만들어주는 이유.. 초기화
        Rigidbody2D rigd = token.GetComponent<Rigidbody2D>();
    }
    //이동
    void TokenMove()
    {
        TokenSet targetToken = TokenList[Count].GetComponent<TokenSet>(); //이동될 타겟 토큰
        if (Input.GetKeyDown(KeyCode.LeftArrow)) //pich
        {
            ScoreCalculation(targetToken, 0);
            MoveTokenTrain(PositionList[5].transform);
            Create();
        }
        if(Input.GetKeyDown(KeyCode.Space)) // apple
        {
            ScoreCalculation(targetToken, 1);
            MoveTokenTrain(PositionList[6].transform);
            Create();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) // bannana
        {
            ScoreCalculation(targetToken, 2);
            MoveTokenTrain(PositionList[7].transform);
            Create();

        }
    }

    //점수
    void ScoreCalculation(TokenSet token, int listcount) //정답번호
    {
        Debug.Log("Token Type : " + token.TokenType + " || listcount : " + listcount);
        if (listcount == (int)(token.TokenType))
        {
            Score += TokenPoint;
        }
        else
        {
            if(Score > 0)
            {
                Score -= TokenPoint;
            }
        }
        ScoreTxt.text = Score.ToString();
        EndCoin += 5;
    }

    void MoveTokenTrain(Transform Dir)
    {
        for(int i = 0; i < 5; i++)
        {
            TokenSet tokenSet = TokenList[i].GetComponent<TokenSet>(); //생성자같은 느낌인가
            if(tokenSet.tokenTurn < 4)
            {
                tokenSet.targetTr = PositionList[tokenSet.tokenTurn + 1].transform;
            }
            else
            {
                tokenSet.targetTr = Dir; // 맨 아래, 맞춰야할 해당 방향으로 토큰 이동시키기, tokenSet에 포지션이값이 다르면
                //이동시키게 돼있음
            }
            tokenSet.tokenTurn++;
        }
    }
    void StartTokenInit() //토큰 이미지 바꾸기
    {
        for(int i = 0; i < TokenList.Count; i++)
        {
            TokenList[i].transform.position = PositionList[4 - i].transform.position;
            TokenSet tokenSet = TokenList[i].GetComponent<TokenSet>();
            switch ((int)tokenSet.TokenType)
            {
                case (int)TokenColor.Pich:
                    tokenSet.spriterRenderer.sprite = SpriteList[0];
                    break;
                case (int)TokenColor.Apple:
                    tokenSet.spriterRenderer.sprite = SpriteList[1];
                    break;
                case (int)TokenColor.Bannana:
                    tokenSet.spriterRenderer.sprite = SpriteList[2];
                    break;
            }

        }
    }

    public void GameEnd()
    {
        //타임 제로
        Time.timeScale = 0;

        IsEnd = true;
        GameEndPanel.SetActive(true);
        CompleteNum.text = Score.ToString();
        //Debug.Log("게임엔드~~~~~~~~~~~~~~~~~~~~~~~~");
    }

    public void Reward()
    {
        inventorys.coin += EndCoin;
    }
}
