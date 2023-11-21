using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Slider Timers;
    float fSliderBarTime;

    FlashGameManager MiniGame1;
    CloseBoxGame MiniGame2;

    void Start()
    {
        Timers = gameObject.GetComponent<Slider>();
        MiniGame1 = FindObjectOfType<FlashGameManager>();
        MiniGame2 = FindObjectOfType<CloseBoxGame>();
        SetTimer(50);
    }

    void Update()
    {
        if (Timers.value > 0.0f)
        {
            // 시간이 변경한 만큼 slider Value 변경
            Timers.value -= Time.deltaTime;
        }
        else
        {
            if(MiniGame1 != null)
                MiniGame1.GameEnd();
            else if(MiniGame2 != null)
                MiniGame2.GameEnd();
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
