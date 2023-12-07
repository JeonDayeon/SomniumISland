using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Costume
{
    public string name;
    public AnimatorOverrideController overrideAnim;
}

[System.Serializable]
public struct Costumes
{
    public enum Type
    {
        Hair,
        Face,
        Closet,
        shoes
    };
    public Type type;
    public Costume[] c;
}
public static class playerSave
{
    public static int[] CustomSettings = new int[4];
    public static Color CustomColor = Color.white;
}

public class PlayerController : MonoBehaviour
{
//대화----------------------------------------------------------------------------------------
    public GameManager Game;
    public GameObject scanObject;

//캐릭터 이동---------------------------------------------------------------------------------
    public float speed = 0.0f;
    float Walkspeed = 9.0f;
    float Runspeed = 20.0f;

    Animator anim;
    float h;
    float v;
    bool isHorizonMove;

    Rigidbody2D rbody;
    bool isMoving = true;

//캐릭터 커스터마이징--------------------------------------------------------------------------
    [SerializeField]
    public Costumes[] Costume;

    public Animator[] Bodypart = new Animator[4];
    public int[] CustomSettings = new int[4];
    public SpriteRenderer spriteRender;

    public GameObject CustomizingCanvas;
    //캐릭터 맵 레이어------------------------------------------------------------------------------
    //rigid
    SpriteRenderer sr;
    public SpriteRenderer[] Csr = new SpriteRenderer[4];//스킨용
    void Start()
    {
        Game = FindObjectOfType<GameManager>();
        rbody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        for (int i = 1; i < transform.childCount; i++)
        {
            Csr[i - 1] = transform.GetChild(i).GetComponent<SpriteRenderer>();
            Bodypart[i - 1] = transform.GetChild(i).GetComponent<Animator>();
        }
        spriteRender = transform.GetComponent<SpriteRenderer>();
        speed = Walkspeed;
        
        CustomSettings = playerSave.CustomSettings;
        setCostume();
    }

    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        bool hDown = Input.GetButtonDown("Horizontal");
        bool vDown = Input.GetButtonDown("Vertical");
        bool hUp = Input.GetButtonUp("Horizontal");
        bool vUp = Input.GetButtonUp("Vertical");
        bool Run = Input.GetKeyDown(KeyCode.LeftShift);

       
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log(anim.speed);
            anim.speed = 1.5f;
            speed = Runspeed;
        }
        else
        {
            anim.speed = 1f;
            speed = Walkspeed;
        }

        if (hDown)
        {
            isHorizonMove = true;
        }
        else if (vDown)
        {
            isHorizonMove = false;
        }
        else if (hUp)
        {
            isHorizonMove = h != 0;
        }

        if (anim.GetInteger("hAxisRaw") != h)
        {
            anim.SetBool("isChange", true);
            anim.SetInteger("hAxisRaw", (int)h);

        }
        else if (anim.GetInteger("vAxisRaw") != v)
        {
            anim.SetBool("isChange", true);
            anim.SetInteger("vAxisRaw", (int)v);
        }
        else
        {
            anim.SetBool("isChange", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && scanObject != null && !Game.Store.activeSelf)
        {
            isMoving = false;
            switch(scanObject.name)
            {
                case "Mirror":
                    CustomizingCanvas.SetActive(true);
                    break;
                case "GreenHouse":
                    Game.PlayeMiniGame(scanObject);
                    break;
                case "소만": case "추분": case "곡우":
                    Game.Action(scanObject);
                    break;
            }
        }

        sr.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
        for(int i = 0; i < 4; i++)
        {
            Csr[i].sortingOrder = sr.sortingOrder + 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        scanObject = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        scanObject = null;
    }

    private void FixedUpdate()
    {
        Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
        rbody.velocity = moveVec * speed;
    }

    public void setCostume()
    {
        spriteRender.color = playerSave.CustomColor;
        for (int i = 0; i < Costume.Length; i++)
        {
            Bodypart[i].runtimeAnimatorController = Costume[i].c[CustomSettings[i]].overrideAnim;
        }
    }
}
