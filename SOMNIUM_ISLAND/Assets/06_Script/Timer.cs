using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Slider Timers;
    float fSliderBarTime;
    FlashGameManager game;
    void Start()
    {
        Timers = gameObject.GetComponent<Slider>();
        game = FindObjectOfType<FlashGameManager>();
        SetTimer(50);
    }

    void Update()
    {
        if (Timers.value > 0.0f)
        {
            // �ð��� ������ ��ŭ slider Value ����
            Timers.value -= Time.deltaTime;
        }
        else
        {
            game.GameEnd();
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
