using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    PlayerController player;

    //��ȭ-------------------------------------------
    TalkManager talkmanager;
    public GameObject TalkBox;
    public Text talkText;
    public Text nameText;
    public GameObject ScanObj;

    int talkid;
    int talkindex;
    string talkType;
    bool isTalk;
    //����-------------------------------------------
    public GameObject selectMenu;
    bool isMenu = false;
    // Start is called before the first frame update
    void Start()
    {
        //Find�� �ʿ� ���� �˾Ƽ� ã�� �� �ְ� �ϱ� ����
        player = FindObjectOfType<PlayerController>();

        talkmanager = FindObjectOfType<TalkManager>();
        //TalkBox = GameObject.Find("TalkBox");
        //talkText = GameObject.Find("TalkText").GetComponent<Text>();
        //nameText = GameObject.Find("NameText").GetComponent<Text>();

        talkindex = 0; //�� ������ ������� �������� ����
        talkType = null;
        //isTalk = true;
        //�� ���̵� ��������
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

        if(talkType == null && talkindex > 0) //��� �Ʒ� �ٸ� ĳ���� ���� ��� ���ϵ���
        {
            Debug.Log("��������������������");
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
