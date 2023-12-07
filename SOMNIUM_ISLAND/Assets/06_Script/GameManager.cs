using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public RawImage Standing;
    public Texture[] NpcT;

    int talkid;
    int talkindex;
    string talkType;
    bool isTalk;
    //선택-------------------------------------------
    public GameObject selectMenu;
    bool isMenu = false;
    //인벤토리---------------------------------------
    public Item itemslist;
    public GameObject Inven;
    public TextMeshProUGUI CoinTxt;
    public GameObject[] Slots = new GameObject[14];
    public GameObject ClickInven;
    public int ClickInvenNum;
    //상점--------------------------------------------
    public GameObject Store;
    public GameObject[] StoreSlots = new GameObject[7];
    public GameObject StoreButton;
    //선물--------------------------------------------
    public string NPCname;
    //미니게임----------------------------------------
    public GameObject MiniGamePopup;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        //Find는 맵에 들어가면 알아서 찾을 수 있게 하기 위함
        player = FindObjectOfType<PlayerController>();

        talkmanager = FindObjectOfType<TalkManager>();
        Standing = GameObject.Find("TalkCanvas").transform.GetChild(0).GetComponent<RawImage>();
        //TalkBox = GameObject.Find("TalkBox");
        //talkText = GameObject.Find("TalkText").GetComponent<Text>();
        //nameText = GameObject.Find("NameText").GetComponent<Text>();

        talkindex = 0; //톡 데이터 순서대로 내보내기 위함
        talkType = null;
        //맵 아이디 가져오기

        itemslist = FindObjectOfType<Item>();

        Inven = GameObject.Find("Inventory");
        Inven.SetActive(false);

        GameObject slotsParents = Inven.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i] = slotsParents.transform.GetChild(i).gameObject;
        }

        CoinTxt = Inven.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

        Store = GameObject.Find("StoreCanvas");
        Store.SetActive(false);

        slotsParents = Store.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject;

        for (int i = 0; i < StoreSlots.Length; i++)
        {
            StoreSlots[i] = slotsParents.transform.GetChild(i).gameObject;
        }

        SetStore();
        StoreButton = TalkBox.transform.GetChild(2).GetChild(2).gameObject;

        MiniGamePopup = GameObject.Find("MiniGameCanvas").transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("인벤토리");
            if (Inven.activeSelf)
            {
                Inven.SetActive(false);
                Destroy(ClickInven);
                ClickInven = null;
                ClickInvenNum = 0;
            }

            else
            {
                SetInven();
                Inven.SetActive(true);
            }
        }

        if (ClickInven != null && Inven.activeSelf)
            ClickInven.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, -Camera.main.transform.position.z)); //마우스 커서에 오브젝트가 따라다니게
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
        NPCname = nameData;
        if (NPCname == "추분")
        {
            StoreButton.SetActive(true);
        }
        string image = talkmanager.GetTalk(talkId, talkindex, "Image", talkType);
        if (image != " " || image != null)
        {
            for (int i = 0; i < NpcT.Length; i++)
            {
                if (image == NpcT[i].name)
                {
                    Standing.texture = NpcT[i];
                }
            }
        }
        Standing.gameObject.SetActive(true);
        talkText.text = talkData;
        nameText.text = nameData;
        Time.timeScale = 0;

        if (talkType == null && talkindex > 0) //대사 아래 다른 캐릭터 것은 출력 못하도록
        {
            talkData = null;
            talkid = talkId;
        }

        if (talkData == null && isMenu != true)
        {

            talkindex = 0;
            TalkBox.SetActive(false);
            Standing.gameObject.SetActive(false);
            isTalk = false;
            Time.timeScale = 1.0f;

            Inven.SetActive(false);
            Store.SetActive(false);
            StoreButton.SetActive(false);
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

    public void SetInven()
    {
        for (int i = 0; i < inventorys.invenItems.Count; i++)
        {
            GameObject slotItem = Slots[i].transform.GetChild(0).gameObject;
            slotItem.name = itemslist.items[inventorys.invenItems[i]].name;
            slotItem.GetComponent<Image>().sprite = itemslist.items[inventorys.invenItems[i]].image;
            slotItem.SetActive(true);
        }
        CoinTxt.text = inventorys.coin.ToString() + " Coin";
    }
    public void SetStore()
    {
        for (int i = 0; i < itemslist.items.Length; i++)
        {
            GameObject slotItem = StoreSlots[i].transform.GetChild(0).gameObject;
            slotItem.name = itemslist.items[i].name;
            StoreSlots[i].transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().text =
                itemslist.items[i].price.ToString();
            StoreSlots[i].transform.GetChild(3).transform.GetComponent<Text>().text = itemslist.items[i].id.ToString();
            slotItem.GetComponent<Image>().sprite = itemslist.items[i].image;
            slotItem.SetActive(true);
        }
    }

    public void InvenBtn()
    {
        if (ClickInven == null)
        {
            GameObject Clickobj = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject;
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Clickobj.transform.parent.gameObject == Slots[i])
                {
                    ClickInvenNum = i;
                    break;
                }
            }

            string name = Clickobj.name;
            ClickInven = Instantiate(Clickobj, Inven.transform);
            Clickobj.name = "ItemImage";
            Clickobj.transform.GetComponent<Image>().sprite = null;
            Clickobj.SetActive(false);
            ClickInven.name = name;
        }
    }

    public void StoreBtn()
    {
        Transform ClickStoreObj = EventSystem.current.currentSelectedGameObject.transform;
        int ObjId;
        ObjId = int.Parse(ClickStoreObj.transform.GetChild(3).GetComponent<Text>().text);
        int price = int.Parse(ClickStoreObj.GetChild(1).GetComponent<TextMeshProUGUI>().text);
        if (inventorys.invenItems.Count < 14 && inventorys.coin > price)
        {
            inventorys.coin -= price;
            inventorys.invenItems.Add(ObjId);
            SetInven();
        }
    }

    public void Present()
    {
        if (ClickInven != null)
        {
            switch (NPCname)
            {
                case "소만":
                    talkText.text = "어머, 고마워요!";
                    break;
                case "곡우":
                    talkText.text = "저한테 주시는 건가요?, 감사합니다.";
                    break;
                case "추분":
                    talkText.text = "저한테 뭔가 바라시는 거라도 있습니까? 세상 공짜는 없죠.";
                    TradeItem();
                    break;
            }
            int arrNum = inventorys.invenItems.Count;
            Slots[arrNum - 1].transform.GetChild(0).gameObject.SetActive(false);
            inventorys.invenItems.RemoveAt(ClickInvenNum);
            SetInven();
            Destroy(ClickInven);
            ClickInven = null;
            ClickInvenNum = 0;
        }
    }
    public void TradeItem()
    {
        for(int i = 0; i < itemslist.items.Length; i++)
        {
            if (ClickInven.name == itemslist.items[i].name)
            {
                inventorys.coin += itemslist.items[i].price;
                break;
            }
        }
    }

    public void UseStore()
    {
        if(Store.activeSelf)
        {
            Store.SetActive(false);
            Inven.SetActive(false);
            talkText.text = "다음에 또 이용해주십쇼~!";
        }

        else 
        {
            Store.SetActive(true);
            Inven.SetActive(true);
            SetInven();
            SetStore();
            talkText.text = "구매하러 왔습니까? 한번 쭉 둘러보고 가십쇼~";
        }
    }

    public void PlayeMiniGame(GameObject scan)
    {
        ScanObj = scan;
        MiniGamePopup.SetActive(true);
    }

    public void LetsTalk()
    {

    }

    public void PlayQuest()
    {

    }

}
