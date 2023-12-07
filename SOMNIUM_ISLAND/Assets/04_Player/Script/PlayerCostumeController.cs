using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCostumeController : MonoBehaviour
{
    public float speed = 0.0f;
    float Walkspeed = 9.0f;
    float Runspeed = 20.0f;
    Animator anim;
    AnimatorOverrideController aoc;
    float h;
    float v;
    bool isHorizonMove;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        speed = Walkspeed;
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
    }
}
