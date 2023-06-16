using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GemSO", menuName = "GemSO/Gem")]
public class GemSO : ScriptableObject
{
    public Attributes attributes;
}

[System.Serializable]
public class Attributes
{
    public string name;
    public int price;
    public Sprite icon;
    public GameObject prefab;
    public Color lineColor;    
}