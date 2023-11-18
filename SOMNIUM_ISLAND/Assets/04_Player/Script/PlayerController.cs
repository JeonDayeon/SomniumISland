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

public class PlayerController : MonoBehaviour
{
//대화----------------------------------------------------------------------------------------
    public GameManager Game;
    public GameObject scanObject;

//캐릭터 이동---------------------------------------------------------------------------------
    public float speed = 0.0f;
    public float Walkspeed = 9.0f;
    public float Runspeed = 10.0f;

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

    void Start()
    {
        Game = FindObjectOfType<GameManager>();
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        for (int i = 1; i < transform.childCount; i++)
        {
            Bodypart[i - 1] = transform.GetChild(i).GetComponent<Animator>();
        }
        spriteRender = transform.GetComponent<SpriteRenderer>();
        speed = Walkspeed;

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
            anim.speed = 1.3f;
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

        if (Input.GetKeyDown(KeyCode.Space) && scanObject != null)
        {
            isMoving = false;
            Game.Action(scanObject);
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
        for(int i = 0; i < Costume.Length; i++)
        {
            Bodypart[i].runtimeAnimatorController = Costume[i].c[CustomSettings[i]].overrideAnim;
        }
    }
}
