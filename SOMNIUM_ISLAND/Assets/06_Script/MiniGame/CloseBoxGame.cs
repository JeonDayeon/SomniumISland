using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CloseBoxGame : MonoBehaviour
{
    //-------------------------------------------------게임 노드(슬라이더)
    public GameObject NodePrefab;
    public Slider Slide;
    public GameObject Handle;

    public List<BoxCollider2D> nodes = new List<BoxCollider2D>();
    //-------------------------------------------------게임 레벨
    public int level;
    public TextMeshProUGUI leveltxt;

    public int WantNum, ClearNum;
    public TextMeshProUGUI WantNumtxt;

    //-----------------------------------------------------게임엔드
    public GameObject GameEndPanel;
    public TextMeshProUGUI CompleteNum;
    bool IsEnd = false;//게임 진행 안되게 막는 변수

    public int EndCoin = 0; //게임 진행 결과에 따른 코인

    // Start is called before the first frame update
    void Start()
    {
        GameEndPanel = GameObject.Find("Canvas").transform.GetChild(5).gameObject;
        CompleteNum = GameEndPanel.transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>();

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
            // 시간이 변경한 만큼 slider Value 변경
            Slide.value -= Time.deltaTime;
        }

        if(Slide.value <= 0)
        {
            ResetNew();
            GameRedy();
        }

        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("왼쪽");
            nodeJudg(Color.red);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("오른쪽");
            nodeJudg(Color.yellow);
        }
    }

    void GameRedy()
    {
        float NodeX = Slide.gameObject.transform.position.x + 30;
        float NodeY = Slide.gameObject.transform.position.y;
        float SlideWidth = Slide.gameObject.GetComponent<RectTransform>().rect.width;
        int j = 0;
        for (float i = 102; i < 581; i += 34 / 2)
        {
            int num = Random.Range(0, 4);
            if (num != 0 && num != 2 && num != 3)
            {
                GameObject N = Instantiate(NodePrefab, new Vector2(i, NodeY), Quaternion.identity, Slide.gameObject.transform.GetChild(2));
                N.transform.SetAsFirstSibling();
                N.name = "node_" + j; 
                int randomColor = Random.Range(0, 10);
                nodes.Add(N.GetComponent<BoxCollider2D>());
                if (randomColor < 5)
                {
                    nodes[j].transform.GetComponent<Image>().color = Color.red;
                    print("레드");
                }
                else
                {
                    nodes[j].transform.GetComponent<Image>().color = Color.yellow;
                    print("엘로");
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
                    print("들어갔지롱");
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
            if (nodes[i].gameObject.transform.GetComponent<Image>().color == Color.blue) //이부분 색 바꾸는 부분에 넣어서 측정 시키기(전역변수로)
                num++;

            Destroy(nodes[i].gameObject);
        }

        if (num >= (nodes.Count * 70 / 100)) //이부분도 색 바꾸는 부분에 넣기(퍼센트마다 상자 이미지 바뀌게)
        {
            ClearNum++;
            WantNumtxt.text = ClearNum + "/" + WantNum;

            EndCoin += nodes.Count * 5;
        }

        nodes.Clear();
    }

    public void GameEnd()
    {
        Time.timeScale = 0;
        IsEnd = true;
        GameEndPanel.SetActive(true);
        CompleteNum.text = ClearNum + "/" + 20;
    }

    public void Reward()
    {
        inventorys.coin = EndCoin;
    }
}
