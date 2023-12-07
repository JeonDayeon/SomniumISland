using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[System.Serializable]
enum FruitType
{
    ���,
    ������,
    �ٳ���
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
    //------------------------------------------------------����
    public Sprite[] FruitSprite;
    SpriteRenderer[] OnFruit;
    int FruitArrNum;//��� �ڸ���

    string FirstFruitName;
    int FirstFruitNum;//���� ����� �迭 �ڸ���
    int SetOnFruitNum;//���° ���� ���￡ �ö󰡴��� �迭 �ڸ���
    bool SetOnFruitBool = false;

    int Weight;
    TextMeshProUGUI ScaleText;
    //------------------------------------------------------�ڽ�
    int BoxWeight;
    TextMeshProUGUI Boxscale;

    SpriteRenderer inboxfruit;
    public Sprite[] inboxFruitSprite;
    //------------------------------------------------------���
    [SerializeField]
    FruitBoxList[] boxlist;
    int Wi = 0;
    public int RandomListNum;

    public TextMeshProUGUI[] BoxlistText;
    //-----------------------------------------------------���ӿ���
    public GameObject GameEndPanel;
    public TextMeshProUGUI CompleteNum;
    bool IsEnd = false;//���� ���� �ȵǰ� ���� ����

    public int EndCoin = 0; //���� ���� ����� ���� ����

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

        RandomListNum = 20;//�ڽ� ���� ���� �迭 ����
        while (RandomListNum > 0) //�ڽ� ���� ���� �迭 ����
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

        GameObject listTxtp = GameObject.Find("GameList"); //��� ����� �� �߰�
        BoxlistText = new TextMeshProUGUI[boxlist.Length];
        for (int i = 0; i < boxlist.Length; i++)
        {
            BoxlistText[i] = listTxtp.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>();

            int num = UnityEngine.Random.Range(0, 3);

            switch(num) //�� ��� ���� ���� ���ϱ�
            {
                case 0:
                    boxlist[i].fruit = FruitType.���;
                    break;
                case 1:
                    boxlist[i].fruit = FruitType.������;
                    break;
                case 2:
                    boxlist[i].fruit = FruitType.�ٳ���;
                    break;
            }
            int g = UnityEngine.Random.Range(100, 3000); //�� ��� ���� ���� �����ֱ�
            g -= g % 100; //10�ڸ����� ����
            boxlist[i].Boxweight = g;//Ȯ��

            BoxlistText[i].text = "- " + boxlist[i].fruit + " " + FruitWeight(boxlist[i].Boxweight)+ " " + "(" + boxlist[i].completeNum + "/" + boxlist[i].wantNum + ")";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !IsEnd)//���콺 ��ư ������ ��
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero, 0f);

            if (hit.collider != null) //���콺 ��ư ���� ���� �ݶ��̴��� ���� ��
            {
                GameObject clicks = hit.transform.gameObject;
                Debug.Log(clicks.name);

                if (Input.GetMouseButtonDown(0))//���� ���콺 Ŭ����
                {
                    if (!SetOnFruitBool)
                    {
                        switch (clicks.name)//�ݶ��̴� �̸����� �ൿ �з�
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

                    else if (SetOnFruitNum < OnFruit.Length && clicks.name == FirstFruitName && Weight < 1500)//���� �ڽ� ������ ��
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

                else if(Input.GetMouseButtonDown(1))//������ ���콺 Ŭ���� ���� �ֱ�
                {

                    if (clicks.name == "Box")
                    {
                        if (BoxWeight != 0) //��Ͽ� ���缭 �ߴ��� Ȯ������
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

    void SetFruit(int i, string name) //ó�� ���� ���￡ �ø� ��
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

    string FruitWeight(int wet) //���� ���� g, kg����
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
    
    void ReScale()//���� �ٽ� �� ��
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
