using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Farm: MonoBehaviour
{
    #region Variables
    [HideInInspector] public List<Transform> placeUnits { get; private set; } = new List<Transform>();
    [HideInInspector] public bool built = false;

    [SerializeField] private GameObject placeUnitPrefab;
    [SerializeField] private Vector2Int gridUnits;
    [SerializeField] private Vector2 offset;

    private List<int> respawnedGemsId = new List<int>();
    private GemManager gemManager;
    private float gemHandleScale;

    [SerializeField] private float generateDuration;
    private bool canGenerate = true;
    #endregion

    #region Unity Funtion Events
    private void Start()
    {
        gemHandleScale = transform.GetChild(0).GetChild(0).localScale.x;
    }

    private void OnEnable()
    {
        PlayerStack.OnCollected += ()=> StartCoroutine(GenerateGem());
    }
    #endregion
    

    IEnumerator GenerateGem()
    {
        yield return new WaitForSeconds(generateDuration);

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).childCount < 1 && canGenerate)
            {
                Transform t = Instantiate(new GameObject(), transform.GetChild(i)).transform;
                t.localScale = Vector3.one * gemHandleScale;

                gemManager = GameObject.FindGameObjectWithTag(TagManager.Manager).GetComponent<GemManager>();
                gemManager.GetComponent<GemManager>().CreateGem(t);

                canGenerate = false;
                yield return new WaitForSeconds(generateDuration);
                canGenerate = true;
            }
        }
    }
   
    public void BuildFarm()
    {
        for (int i = 0; i < gridUnits.x * gridUnits.y; i++)
        {
            Vector3 placeUnitPos = transform.position + Vector3.right * (offset.x * (i % gridUnits.x)) + Vector3.forward * ((Mathf.FloorToInt(i/ gridUnits.x)) * offset.y);
            
            Transform placeUnit = Instantiate(placeUnitPrefab, placeUnitPos, Quaternion.identity, transform).transform;
            placeUnits.Add(placeUnit);

            gemManager = GameObject.FindGameObjectWithTag(TagManager.Manager).GetComponent<GemManager>();
            gemManager.GetComponent<GemManager>().CreateGem(placeUnit.GetChild(0));

            if(i < (gridUnits.x * gridUnits.y) - 1) built = true;
        }
    }
    
    public void EditFarm()
    {
        #region ReBuild
        if (placeUnits.Count != gridUnits.x * gridUnits.y)
        {
            //Add
            while (placeUnits.Count < gridUnits.x * gridUnits.y)
            {
                Transform t = Instantiate(placeUnitPrefab, transform).transform;
                placeUnits.Add(t);

                gemManager = GameObject.FindGameObjectWithTag(TagManager.Manager).GetComponent<GemManager>();
                gemManager.GetComponent<GemManager>().CreateGem(t.GetChild(0));
            }

            //Remove
            while (placeUnits.Count > gridUnits.x * gridUnits.y)
            {
                if (placeUnits.Count > 0)
                {
                    DestroyImmediate(placeUnits.Last().gameObject);
                    placeUnits.Remove(placeUnits.Last());
                }
            }
        }
        #endregion

        #region RePlace
        int placeUnitsCount = placeUnits.Count;
        for (int i = 0; i < placeUnitsCount; i++)
        {            
            Vector3 placeUnitPos = transform.position + Vector3.right * (offset.x * (i % gridUnits.x)) + Vector3.forward * ((Mathf.FloorToInt(i / gridUnits.x)) * offset.y);
            placeUnits[i].transform.position = placeUnitPos;

            #region Respawn Gem
            int id = placeUnits[i].GetChild(0).GetChild(0).GetComponent<Gem>().id;
            respawnedGemsId.Add(id);

            #region Only Gem Respawn
            /*foreach (Transform child in placeUnits[i])
                id = child.GetComponent<Gem>() != null? child.GetComponent<Gem>().id : 0;*/

            /*while (placeUnits[i].childCount > 0)
              DestroyImmediate(placeUnits[i].GetChild(0).gameObject);*/

            /*GameObject gemManager = GameObject.Find("GemManager");
            gemManager.GetComponent<GemManager>().Respawn(id, placeUnits[i].GetChild(0));*/
            #endregion
            #endregion

            /*DestroyImmediate(placeUnits[i].gameObject);
            //placeUnits.Remove(placeUnits.First());

            Transform placeUnit = Instantiate(placeUnitPrefab, placeUnitPos, Quaternion.identity, transform).transform;
            placeUnits[i] = (placeUnit);
            
            GameObject gemManager = GameObject.Find("GemManager");
            gemManager.GetComponent<GemManager>().Respawn(respawnedGemsId[i], placeUnits[i].GetChild(0));*/
        }

        #region Respawn PlaceUnit
        while (placeUnits.Count > 0)
        {
            DestroyImmediate(placeUnits.First().gameObject);
            placeUnits.Remove(placeUnits.First());
        }

        for (int i = 0; i < gridUnits.x * gridUnits.y; i++)
        {
            Vector3 placeUnitPos = transform.position + Vector3.right * (offset.x * (i % gridUnits.x)) + Vector3.forward * ((Mathf.FloorToInt(i / gridUnits.x)) * offset.y);

            Transform placeUnit = Instantiate(placeUnitPrefab, placeUnitPos, Quaternion.identity, transform).transform;
            placeUnits.Add(placeUnit);

            gemManager = GameObject.FindGameObjectWithTag(TagManager.Manager).GetComponent<GemManager>();
            gemManager.GetComponent<GemManager>().Respawn(respawnedGemsId[i], placeUnits[i].GetChild(0));
        }
        
        #endregion

        #endregion
    }

    public void DestroyFarm()
    {
        DestroyImmediate(gameObject);
    }
}
