using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    PlayerController player;

    //대화-------------------------------------------
    TalkManager talkmanager;
    public GameObject TalkBox;
    public Text talkText;
    public Text nameText;
    public GameObject ScanObj;

    int talkid;
    int talkindex;
    string talkType;
    bool isTalk;
    //선택-------------------------------------------
    public GameObject selectMenu;
    bool isMenu = false;
    // Start is called before the first frame update
    void Start()
    {
        //Find는 맵에 들어가면 알아서 찾을 수 있게 하기 위함
        player = FindObjectOfType<PlayerController>();

        talkmanager = FindObjectOfType<TalkManager>();
        //TalkBox = GameObject.Find("TalkBox");
        //talkText = GameObject.Find("TalkText").GetComponent<Text>();
        //nameText = GameObject.Find("NameText").GetComponent<Text>();

        talkindex = 0; //톡 데이터 순서대로 내보내기 위함
        talkType = null;
        //isTalk = true;
        //맵 아이디 가져오기
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Action(GameObject scanObj)
    {
        ScanObj = scanObj;
        NpcData npcdata = ScanObj.gameObject.GetComponent<NpcData>();
        Talk(npcdata.id);
    }
    public void Talk(int talkId)
    {
        TalkBox.SetActive(true);

        string talkData = talkmanager.GetTalk(talkId, talkindex, "Content", talkType);
        string nameData = talkmanager.GetTalk(talkId, talkindex, "Name", talkType);
        talkText.text = talkData;
        nameText.text = nameData;
        Time.timeScale = 0;

        if(talkType == null && talkindex > 0) //대사 아래 다른 캐릭터 것은 출력 못하도록
        {
            Debug.Log("꺼져꺼져꺼져꺼져꺼져");
            talkData = null;
            talkid = talkId;
        }

        if (talkData == null && isMenu != true)
        {
            talkindex = 0;
            TalkBox.SetActive(false);
            isTalk = false;
            Time.timeScale = 1.0f;
            return;
        }
        
        else
        {
            talkindex++;
        }
    }

    public void GoTalk()
    {
        talkType = "talk";
        Talk(talkid);
    }
}
