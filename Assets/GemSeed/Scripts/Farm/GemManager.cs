using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GemManager : MonoBehaviour
{
    #region Variables

    public List<GemSeed> gemStruck = new List<GemSeed>();

    [System.Serializable]
    public class GemSeed
    {
        public string name;
        public int price;
        public Sprite icon;
        public GameObject gemPrefabs;
        public Color lineColor;
    }
    #endregion

    public void CreateGem(Transform parent)
    {
        int randomGem = Random.Range(0, gemStruck.Count);
   
        GameObject gem = Instantiate(gemStruck[randomGem].gemPrefabs, parent);
        gem.GetComponent<Gem>().Asign(gemStruck[randomGem].name, gemStruck[randomGem].price, gemStruck[randomGem].icon, randomGem);
    }

    public GameObject Respawn(int id,Transform parent)
    {
        GameObject gem = Instantiate(gemStruck[id].gemPrefabs, parent);
        gem.GetComponent<Gem>().Asign(gemStruck[id].name, gemStruck[id].price, gemStruck[id].icon, id);
        return gem;
    }
}
