using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{      
    public Transform target; // 이동할 타겟 설정
    public Transform Ctarget;

    public GameObject thePlayer;
    public GameObject theCamera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // 박스 콜라이더에 닿는 순간 이벤트 발생
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("들어옴");
        thePlayer.transform.position = target.transform.position;
    }


}