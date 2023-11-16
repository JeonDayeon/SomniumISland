using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenCreate : MonoBehaviour
{
    public List<GameObject> TokenList;
    public List<GameObject> PositionList;
    public GameObject TokenPrefab;

    public List<Sprite> SpriteList; //�̹���

    int Count;
    int TokenPoint;
    int Score; // scoreSet����
    public enum TokenColor
    {
        Pich,
        Apple,
        Bannana
    }
    void Start()
    {
        Count = 0;
        TokenPoint = 1;
        Score = 0;
        StartTokenInit();
    }

    // Update is called once per frame
    void Update()
    {
        TokenMove();
    }
    //����
    public void Create()
    {
        GameObject token = Instantiate(TokenPrefab, PositionList[0].transform.position, Quaternion.identity); //0�� ����
        TokenSet tokenSet = token.GetComponent<TokenSet>();
        tokenSet.spriterRenderer.sprite = SpriteList[(int)tokenSet.TokenType]; //������, ���, �ٳ��������� 0 1 2
        if(Count >= 4)
        {
            TokenList[Count] = token; //������ ��ū
            Count = 0; //ī��Ʈ �ʱ�ȭ
        }
        else
        {
            TokenList[Count] = token; //��ū����Ʈ�� ��ū �߰�
            Count++;
        }
        tokenSet.tokenTurn = 0; // ���⼭ 0���� ������ִ� ����.. �ʱ�ȭ
        Rigidbody2D rigd = token.GetComponent<Rigidbody2D>();
    }
    //�̵�
    void TokenMove()
    {
        TokenSet targetToken = TokenList[Count].GetComponent<TokenSet>(); //�̵��� Ÿ�� ��ū
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

    //����
    void ScoreCalculation(TokenSet token, int listcount) //�����ȣ
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

    }

    void MoveTokenTrain(Transform Dir)
    {
        for(int i = 0; i < 5; i++)
        {
            TokenSet tokenSet = TokenList[i].GetComponent<TokenSet>(); //�����ڰ��� �����ΰ�
            if(tokenSet.tokenTurn < 4)
            {
                tokenSet.targetTr = PositionList[tokenSet.tokenTurn + 1].transform;
            }
            else
            {
                tokenSet.targetTr = Dir; // �� �Ʒ�, ������� �ش� �������� ��ū �̵���Ű��, tokenSet�� �������̰��� �ٸ���
                //�̵���Ű�� ������
            }
            tokenSet.tokenTurn++;
        }
    }
    void StartTokenInit() //��ū �̹��� �ٲٱ�
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
}
