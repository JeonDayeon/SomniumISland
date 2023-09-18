using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Arrow : MonoBehaviour
{
    public Sprite[] ArrImage_df; 
    public List<int> Answer; //���� �� 10��
    public List<Image> ListImageSpace; //�̹���, �ڸ�
    public int TargetValue; //��ǥġ ���� //�ؽ�Ʈ �� ����
    public int ArrowListCount; //� �Ұž�
    int failCount; //Ʋ�� Ƚ��
    Sprite[] setImages;
    bool isSucc = true;
    int current = 0;

    public TextMeshProUGUI Text;
    enum ArrowDt // �� - 0, �� - 1, �� - 2, �� - 3
    {
        �� = 0,
        �� = 1,
        �� = 2,
        �� = 3
    }
    void Start()
    {
        GameObject ArrowList = GameObject.Find("ArrowList");

        Answer = new List<int>(new int[10]); //��
        ListImageSpace = new List<Image>(new Image[10]); //������ �̹��� //���� �� ���� ���� �� list
        TargetValue = 100;
        ArrowListCount = ArrowList.transform.childCount; //�ִ� �� ��ŭ // ArrowList�� ����
        Text = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();

        for (int i = 0; i < ArrowListCount; i++) //����Ʈ �ڽ� �ʱ�ȭ
        {
            ListImageSpace[i] = ArrowList.transform.GetChild(i).gameObject.GetComponent<Image>();
            ListImageSpace[i].color = Color.black;
        }
        RandomAnswerCreate(Answer); //���� �� ����
        Shuffle(Answer); // �� ����
        ImageAnswerCreate(Answer, ListImageSpace, ArrImage_df); //�̹��� �ֱ�

        Text.text = TargetValue.ToString(); //�ؽ�Ʈ �ʱ� ��
    }

    void Update()
    {
        //int inputKey = InputKey(); //Ű�� �޾Ƽ� ���� ��
        if (Input.anyKeyDown && current < 10 && failCount < 3)
        {
            Matching(InputKey(), Answer);
        }

    }
    // ��ǲŰ
    void Matching(int inputKey, List<int>answer)             
    {
        
        if (answer[current] == inputKey)
        {
            Debug.Log("����" + TargetValue);
            ListImageSpace[current].color = Color.yellow; //�÷��� ����
        }
        else
        {
            failCount++;
            Debug.Log("����" + failCount);
            ListImageSpace[current].color = Color.red;
        }
        current++;
        //���µǴ� ����
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
    //����, ������ �޾Ƽ�, �ش�Ǵ� �̹��� ���
    void RandomAnswerCreate(List<int> Array) //�� ����
    {
        Debug.Log("��������ũ������");
        for(int i = 0; i < Array.Count; i++)
        {
            Array[i] = Random.Range(0, 4); //0~3
        }
    }
    void ImageAnswerCreate(List<int> array, List<Image> ImageSpace, Sprite[] setImages) //�̹��� ����ֱ�
    {
        for (int i = 0; i < array.Count; i++)
        {
            int number = array[i]; //���� �� �����Ѱ� ����

            if (number == (int)ArrowDt.��)
            {
                ImageSpace[i].sprite = setImages[0];
                Debug.Log("�̹��� ����" + i + "= ��");
            }
            else if (number == (int)ArrowDt.��)
            {
                ImageSpace[i].sprite = setImages[1];
                Debug.Log("�̹��� ����" + i + "= ��");

            }
            else if (number == (int)ArrowDt.��)
            {
                ImageSpace[i].sprite = setImages[2];
                Debug.Log("�̹��� ����" + i + "= ��");

            }
            else if (number == (int)ArrowDt.��)
            {
                ImageSpace[i].sprite = setImages[3];
                Debug.Log("�̹��� ����" + i + "= ��");
            }
            
        }
    }
    void Shuffle(List<int> list)
    {
        Debug.Log("����");
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
        Debug.Log("����");
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
    //Ű�� �޾� �� ��ȯ
    int InputKey()
    {
        int result = 0;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            result = (int)ArrowDt.��;
            Debug.Log("���� " + result);
            //Shuffle(CurrentALSet);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            result = (int)ArrowDt.��;
            Debug.Log("��" + result);

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            result = (int)ArrowDt.��;
            Debug.Log("������" + result);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            result = (int)ArrowDt.��;
            Debug.Log("��" + result);
        }

        return result;
    }
    void GameReset()
    {
        current = 0;
        failCount = 0;
        for (int i = 0; i < ArrowListCount; i++) //����Ʈ �ڽ� �ʱ�ȭ
        {
              ListImageSpace[i].color = Color.black;
        }
        RandomAnswerCreate(Answer); //���� �� ����
        Shuffle(Answer); // �� ����
        ImageAnswerCreate(Answer, ListImageSpace, ArrImage_df);
    }
}

