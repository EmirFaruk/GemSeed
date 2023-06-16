using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Gem : MonoBehaviour
{
    #region Variables
    public string gemName { get; private set; }
    public int gemPrice { get; private set; }
    public Sprite gemIcon { get; private set; }
    public bool collectable { get; private set; }
    public int id { get; private set; }

    private bool scaled;
    float scale;
    #endregion    

    public void Asign(string gemName, int price, Sprite icon, int id)
    {
        name = gemName;
        this.gemName = gemName;
        gemPrice = price;
        gemIcon = icon;
        this.id = id;
    }

    private void Awake()
    {
        transform.localScale = Vector3.zero;     
    }
    
    private void Update()
    {
        if (CompareTag(TagManager.Collected)) Scale();
        else ScaleUp();
    }

    public void ScaleUp() 
    {
        if (transform.localScale.x >= .25f && !CompareTag(TagManager.Collected)) transform.tag = TagManager.Collectable;
            
        if (CompareTag(TagManager.Collected)) return;
        else if (transform.localScale.x < 1)
            transform.DOScale(transform.localScale.x + .05f, .25f).SetEase(Ease.Linear);
        else
            transform.localScale = Vector3.one;
    }

    private void Scale()
    {
        if (!scaled) scale = (Mathf.Round((float)transform.localScale.x * 100.0f) * 0.01f);
        transform.localScale = new Vector3(scale, scale, scale);
        print(name + " : " + transform.localScale.x + "\tScale : " + scale + "\tlocal : " + transform.localScale.x);
        scaled = true;
    }
}
