using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowTimer : MonoBehaviour
{
    public Slider Timers;
    public Image TimersFill;
    Arrow ArrowGame;
    TokenCreate tokenC;

    // Start is called before the first frame update
    void Start()
    {
        Timers = gameObject.GetComponent<Slider>(); //색깔 접근 숫자
        TimersFill = GameObject.Find("Fill").GetComponent<Image>();
        //TimersFill = Timers.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>();
        ArrowGame = FindObjectOfType<Arrow>();
        SetTimer(20);

        tokenC = FindObjectOfType<TokenCreate>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Timers.value > 0.0f){ 
            if (Timers.value < Timers.maxValue / 1.5f){ // 비율에 따라 이미지 색깔 바꾸기
                TimersFill.color = Color.yellow;
                if (Timers.value < Timers.maxValue / 3.0f){
                    TimersFill.color = Color.red;
                }
            }
            Timers.value -= Time.deltaTime;
        }
        else
        {
            if (ArrowGame != null)
                ArrowGame.GameEnd();
            else if (tokenC != null)
                tokenC.GameEnd();
        }
        
    }

    public void SetTimer(float time)
    {
        Timers.maxValue = time;
        Timers.value = Timers.maxValue;
    }
    public void BackTime(float time)
    {
        Timers.value += time;
    }
}

