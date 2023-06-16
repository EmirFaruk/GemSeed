using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStack : MonoBehaviour
{
    #region Variables    
    public static event Action<int> UpdateStackOrder;
    public static event Action OnCollected;
    [HideInInspector] public List<Gem> CollectedGems { get; private set; } = new List<Gem>();
    [HideInInspector] public List<GameObject> triggedCollectables { get; private set; } = new List<GameObject>();

    [SerializeField] private Transform collectPoint;
    [SerializeField] private float collectCapacity = 5;
    [SerializeField] private float verticalGap = 1;
    [SerializeField] private float collectDuration;

    private Vector3 targetPos;
    private int collectOrder;
    #endregion

    #region Unity Event Functions
    private void Awake()
    {        
        triggedCollectables = new List<GameObject>();
        CollectedGems = new List<Gem>();
    }

    void Update()
    {
        if (triggedCollectables.Count > collectOrder) StackMovement();         
    }

    private void OnEnable()
    {
        PlayerTrigger.OnCollect += AddObject;
        SaleController.OnSale += RemoveFromStack;
    }
    #endregion


    public void AddObject(GameObject obj)
    {        
        if(obj.transform.GetChild(0).CompareTag(TagManager.Collectable))
        {
            if (triggedCollectables.Count > 0 && triggedCollectables.Last() != obj && triggedCollectables.Count < collectCapacity)
            {
                triggedCollectables.Add(obj);
                triggedCollectables.Last().transform.GetChild(0).tag = TagManager.Collected;
            }
            else if (triggedCollectables.Count == 0)
            {
                triggedCollectables.Add(obj);
                triggedCollectables.Last().transform.GetChild(0).tag = TagManager.Collected;
            }
        }
    }

    void StackMovement()
    {
        targetPos = new Vector3(collectPoint.position.x, triggedCollectables[collectOrder].transform.localScale.x / 2 + verticalGap + collectPoint.position.y, collectPoint.position.z);

        triggedCollectables[collectOrder].transform.position = Vector3.Lerp(triggedCollectables[collectOrder].transform.position, targetPos, collectDuration * Time.deltaTime);

        if (triggedCollectables[collectOrder] != null && Vector3.Distance(triggedCollectables[collectOrder].transform.position, targetPos) < .75f)
        {
            triggedCollectables[collectOrder].transform.position = targetPos;
            float scale = triggedCollectables[collectOrder].transform.localScale.x * triggedCollectables[collectOrder].transform.parent.localScale.x;
            triggedCollectables[collectOrder].transform.SetParent(collectPoint);
            triggedCollectables[collectOrder].transform.localScale = Vector3.one * scale;
            triggedCollectables[collectOrder].transform.forward = collectPoint.forward;
            verticalGap += triggedCollectables[collectOrder].transform.GetChild(0).localScale.x / 2;
            CollectedGems.Add(triggedCollectables[collectOrder].transform.GetChild(0).GetComponent<Gem>());
            collectOrder++;

            UpdateStackOrder?.Invoke(CollectedGems.Count);
            OnCollected?.Invoke();
        }
    }

    void RemoveFromStack()
    {
        verticalGap -= CollectedGems.Last().transform.localScale.x / 2;
        Destroy(CollectedGems.Last().transform.parent.gameObject);
        CollectedGems.Remove(CollectedGems.Last());
        triggedCollectables.Remove(triggedCollectables.Last());
        collectOrder--;
    }
}