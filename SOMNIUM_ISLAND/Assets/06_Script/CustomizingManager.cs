using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CustomizingManager : MonoBehaviour
{
    public PlayerController player;
    public GameObject playerEx;
    public Animator[] BodypartEx = new Animator[4];
    public TextMeshProUGUI[] NameTxt = new TextMeshProUGUI[2];
    public int[] PointNum = new int[2];
    public int[] CostumePage = new int[2];
    public GameObject[] Canvases;
    public int CanvasPage = 0;

    public GameObject[] TypeNode;

    public GameObject EndCanvas;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        playerEx = GameObject.Find("PlayerEx");

        Canvases = new GameObject[2];
        Canvases[0] = transform.GetChild(1).gameObject;
        Canvases[1] = transform.GetChild(2).gameObject;

        TypeNode = new GameObject[2];
        TypeNode[0] = Canvases[1].transform.GetChild(2).GetChild(1).gameObject;
        TypeNode[1] = Canvases[1].transform.GetChild(2).GetChild(2).gameObject;

        //EndCanvas = GameObject.Find("EndCostumeCanvas");
        EndCanvas.SetActive(false);
        TypePage();

        for (int i = 0; i < transform.childCount; i++)
        {
            BodypartEx[i] = playerEx.transform.GetChild(i).GetComponent<Animator>();
        }

        SetCustom();
    }

    public void NextType()
    {
        if(CanvasPage < 4)
        {
            CanvasPage++;
            TypePage();
        }
    }
    public void PreviousType()
    {
        if (CanvasPage > 0)
        {
            CanvasPage--;
            TypePage();           
        }
    }
    public void ColorBtn()
    {
        Image ClickBtn = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        Color Btncol = ClickBtn.color;

        playerSave.CustomColor = Btncol;
        player.gameObject.GetComponent<SpriteRenderer>().color = Btncol;
        playerEx.GetComponent<SpriteRenderer>().color = Btncol;
    }
    public void TypePage()
    {
        TextMeshProUGUI TypeTxt = transform.GetChild(3).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        NameTxt[0] = TypeNode[0].transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        NameTxt[1] = TypeNode[1].transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

        switch (CanvasPage)
        {
            case 0:
                Canvases[0].SetActive(true);
                Canvases[1].SetActive(false);
                TypeTxt.text = "ÇÇºÎ»ö";
                break;
            case 1:
                PointNum[0] = 0;
                PointNum[1] = 1;

                Canvases[0].SetActive(false);
                Canvases[1].SetActive(true);
                TypeTxt.text = "Æ¯Â¡";

                TypeNode[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Çì¾î";
                TypeNode[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "¾ó±¼";

                CostumePage[0] = player.CustomSettings[PointNum[0]];
                CostumePage[1] = player.CustomSettings[PointNum[1]];

                NameTxt[0].text = player.Costume[PointNum[0]].c[CostumePage[0]].name;
                NameTxt[1].text = player.Costume[PointNum[1]].c[CostumePage[1]].name;
                
                break;
            case 2:
                PointNum[0] = 2;
                PointNum[1] = 3;

                Canvases[0].SetActive(false);
                Canvases[1].SetActive(true);
                TypeTxt.text = "ÀÇ»ó";

                TypeNode[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "¿Ê";
                TypeNode[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "½Å¹ß";

                CostumePage[0] = player.CustomSettings[PointNum[0]];
                CostumePage[1] = player.CustomSettings[PointNum[1]];

                NameTxt[0].text = player.Costume[PointNum[0]].c[CostumePage[0]].name;
                NameTxt[1].text = player.Costume[PointNum[1]].c[CostumePage[1]].name;
                break;
            case 3:
                EndCanvas.SetActive(true);
                break;
            case 4:
                EndCanvas.SetActive(false);
                gameObject.SetActive(false);
                break;
        }
    }

    public void SetCustom()
    {
        for (int i = 0; i < player.Costume.Length; i++)
        {
            BodypartEx[i].runtimeAnimatorController = player.Costume[i].c[player.CustomSettings[i]].overrideAnim;
        }

        player.setCostume();
    }

    public void NextCustom()
    {
        Image ClickBtn = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        int row = int.Parse(ClickBtn.transform.parent.transform.parent.name);
        Debug.Log(row);

        if (CostumePage[row] >= player.Costume[PointNum[row]].c.Length - 1)
            CostumePage[row] = 0;

        else
            CostumePage[row]++;

        NameTxt[row].text = player.Costume[PointNum[row]].c[CostumePage[row]].name;
        ChangeCustom(PointNum[row], row);
    }
    public void PreviousCustom()
    {
        Image ClickBtn = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        int row = int.Parse(ClickBtn.transform.parent.transform.parent.name);
        Debug.Log(row);

        if (CostumePage[row] <= 0)
            CostumePage[row] = player.Costume[PointNum[row]].c.Length-1;

        else
            CostumePage[row]--;

        NameTxt[row].text = player.Costume[PointNum[row]].c[CostumePage[row]].name;
        ChangeCustom(PointNum[row], row);
    }

    public void ChangeCustom(int index, int row)
    {
        player.CustomSettings[index] = CostumePage[row];
        playerSave.CustomSettings[index] = CostumePage[row];
        BodypartEx[index].runtimeAnimatorController = player.Costume[index].c[player.CustomSettings[index]].overrideAnim;
        player.setCostume();
    }
}
