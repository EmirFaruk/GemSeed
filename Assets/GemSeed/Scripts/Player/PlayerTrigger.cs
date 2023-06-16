using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public static bool atSalePoint;
    public static event Action<GameObject> OnCollect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.Collectable)) OnCollect?.Invoke(other.transform.parent.gameObject);
        if (other.CompareTag(TagManager.SalePoint)) atSalePoint = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagManager.SalePoint)) atSalePoint = false;
    }
}
