using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenSet : MonoBehaviour
{
    public Transform targetTr;
    public int tokenTurn;
    public int Speed;
    public Vector3 pos;
    public SpriteRenderer spriterRenderer;

    public enum TokenColor
    {
        Pich,
        Apple,
        Bannana
    }

    public TokenColor TokenType;
    private void Awake()
    {
        spriterRenderer = gameObject.GetComponent<SpriteRenderer>();
        targetTr = gameObject.transform; //본인
        Speed = 100;
        TokenInit();
    }
    public void TokenInit()
    {
        TokenType = (TokenColor)Random.Range(0, 3);
    }
    
    private void Update()
    {
        pos = targetTr.position;
        if(transform.position != targetTr.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTr.position, Speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderCorrect")
        {
            Debug.Log("디스트로이이이ㅣ이이이ㅣ이이이이ㅣ이이");
            Destroy(gameObject); //본인이요
        }
    }
}
