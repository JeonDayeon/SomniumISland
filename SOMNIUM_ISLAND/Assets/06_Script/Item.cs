using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemArray
{
    public int id;
    public string name;
    public int price;

    public Sprite image;
}

public static class inventorys
{
    public static List<int> invenItems = new List<int>(); //�κ��丮 ������ ���� ����

    public static int coin = 1000;
}

public class Item : MonoBehaviour
{
    [SerializeField]
    public ItemArray[] items;
}
