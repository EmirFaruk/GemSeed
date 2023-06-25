using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SaleController : MonoBehaviour
{
    #region Variables
    public static event Action OnSale;
    public static event Action<float> OnMoneyUpdate;
    
    [SerializeField] private float saleDuration = .1f;
    
    private PlayerStack playerStack;

    private bool canSale = true;
    private int saleOrder;
    
    private float money;

    private Transform target;
    private Animator animator;
    #endregion

    #region Unity Event Functions
    void Start()
    {
        playerStack = FindObjectOfType<PlayerStack>();
        
        money = PlayerPrefs.GetFloat("money");
        OnMoneyUpdate?.Invoke(money);

        target = GameObject.FindWithTag(TagManager.Player).transform;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (PlayerTrigger.atSalePoint)
        {
            StartCoroutine(Selling());
            animator.SetBool("Selling", true);
        }
        else
        {
            StopCoroutine(Selling());
            animator.SetBool("Selling", false);
            Vector3 direction = target.position;
            direction.y = transform.position.y * 2;
            transform.DOLookAt(direction, 1f).SetEase(Ease.OutQuad);
        }
    }

    private void OnEnable()
    {
        PlayerStack.UpdateStackOrder += (int order) => saleOrder = order;
    }
    #endregion

    IEnumerator Selling()
    {        
        if (playerStack.CollectedGems.Count == saleOrder && saleOrder > 0)
        {
            yield return new WaitForSeconds(saleDuration);
            
            if (playerStack.CollectedGems.Count > 0 && PlayerTrigger.atSalePoint && canSale)
            {
                saleOrder--;

                //Get last gem
                Gem gem = playerStack.CollectedGems.Last();

                //Save gem
                PlayerPrefs.SetInt(gem.name, PlayerPrefs.GetInt(gem.name) + 1);
                
                //Money
                float price = gem.gemPrice + (gem.transform.localScale.x) * 100.0f;
                money += price;
                OnMoneyUpdate?.Invoke(money);
                PlayerPrefs.SetFloat("money", money);
                PlayerPrefs.Save();
                print("money: " + money);                

                //Destroy gem
                OnSale?.Invoke();                

                canSale = false;
                yield return new WaitForSeconds(saleDuration);
                canSale = true;
            }
        }
    }
}



#region
//collectObjects[collectOrder].DOMove(new Vector3(collectPoint.position.x, collectPoint.position.y + (float)rawMaterials.Count * verticalGap, collectPoint.position.z), colletDuration).SetEase(collectEase)            

/*targetPos = new Vector3(collectPoint.position.x, collectPoint.position.y + 
    (collectOrder % collectCapacity) * 1 , collectPoint.position.z);*/
#endregion