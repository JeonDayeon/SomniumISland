using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager Game;
    public GameObject scanObject;

    public float speed = 0.0f;
    public float Walkspeed = 9.0f;
    public float Runspeed = 10.0f;

    Animator anim;

    float h;
    float v;
    bool isHorizonMove;

    Rigidbody2D rbody;
    bool isMoving = true;

    // Start is called before the first frame update
    void Start()
    {
        Game = FindObjectOfType<GameManager>();
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        speed = Walkspeed;
    }

    // Update is called once per frame
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

}
