using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CloseBoxGame : MonoBehaviour
{
    //-------------------------------------------------���� ���(�����̴�)
    public GameObject NodePrefab;
    public Slider Slide;
    public GameObject Handle;

    public List<BoxCollider2D> nodes = new List<BoxCollider2D>();
    //-------------------------------------------------���� ����
    public int level;
    public TextMeshProUGUI leveltxt;

    public int WantNum, ClearNum;
    public TextMeshProUGUI WantNumtxt;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        leveltxt = GameObject.Find("LevelTxt").GetComponent<TextMeshProUGUI>();
        leveltxt.text = "Level " + level;

        WantNum = 20;
        WantNumtxt = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        WantNumtxt.text = "0/" + WantNum;

        Slide = GameObject.Find("Slider").GetComponent<Slider>();
        NodePrefab = Resources.Load<GameObject>("prefab/MiniGame/Node");
        Handle = Slide.gameObject.transform.GetChild(2).GetChild(0).gameObject;

        GameRedy();
    }

    // Update is called once per frame
    void Update()
    {
        if (Slide.value > 0.0f)
        {
            // �ð��� ������ ��ŭ slider Value ����
            Slide.value -= Time.deltaTime;
        }

        if(Slide.value <= 0)
        {
            ResetNew();
            GameRedy();
        }

        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("����");
            nodeJudg(Color.red);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("������");
            nodeJudg(Color.yellow);
        }
    }

    void GameRedy()
    {
        int j = 0;
        for (float i = 155.3f; i < 648f; i += 34 / 2)
        {
            int num = Random.Range(0, 4);
            if (num != 0 && num != 2 && num != 3)
            {
                GameObject N = Instantiate(NodePrefab, new Vector2(i, 99.8f), Quaternion.identity, Slide.gameObject.transform.GetChild(2));
                N.transform.SetAsFirstSibling();
                N.name = "node_" + j; 
                int randomColor = Random.Range(0, 10);
                nodes.Add(N.GetComponent<BoxCollider2D>());
                if (randomColor < 5)
                {
                    nodes[j].transform.GetComponent<Image>().color = Color.red;
                    print("����");
                }
                else
                {
                    nodes[j].transform.GetComponent<Image>().color = Color.yellow;
                    print("����");
                }
                j++;
            }
        }
        if (nodes.Count < 6)
        {
            Slide.maxValue = Random.Range(2, 4);
        }
        else
        {
            Slide.maxValue = Random.Range(5, 8);
        }
        Slide.value = Slide.maxValue;
    }

    void nodeJudg(Color DireColor)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            Color nodeColor = nodes[i].gameObject.transform.GetComponent<Image>().color;

            if (nodes[i].bounds.Contains(Handle.transform.position))
            {
                if (nodeColor == DireColor)
                {
                    print("������");
                    nodes[i].gameObject.transform.GetComponent<Image>().color = Color.blue;
                }

                else if (nodeColor != DireColor && nodeColor != Color.blue)
                {
                    nodes[i].gameObject.transform.GetComponent<Image>().color = Color.black;
                }
            }
        }
    }

    void ResetNew()
    {
        int num = 0;
        for(int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].gameObject.transform.GetComponent<Image>().color == Color.blue) //�̺κ� �� �ٲٴ� �κп� �־ ���� ��Ű��(����������)
                num++;

            Destroy(nodes[i].gameObject);
        }

        if (num >= (nodes.Count * 70 / 100)) //�̺κе� �� �ٲٴ� �κп� �ֱ�(�ۼ�Ʈ���� ���� �̹��� �ٲ��)
        {
            ClearNum++;
            WantNumtxt.text = ClearNum + "/" + WantNum;
        }

        nodes.Clear();
    }
}
