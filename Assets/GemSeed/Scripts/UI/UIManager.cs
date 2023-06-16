using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using ColorUtility = UnityEngine.ColorUtility;
using Image = UnityEngine.UI.Image;

public class UIManager : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject gemUIPrefab;
    
    private GemManager gemManager;    
    [SerializeField] private Transform content;

    [SerializeField] TextMeshProUGUI tMoney;

    private int gemTypeCount;
    #endregion

    private void Awake()
    {
        gemManager = FindObjectOfType<GemManager>();
    }

    private void OnEnable()
    {        
        SaleController.OnMoneyUpdate += (float f) => tMoney.text = f.ToString();
    }

    private void Start()
    {        
        gemTypeCount = gemManager.gemStruck.Count;
        CreateContentWindow();
    }
 
    public void CreateContentWindow()
    {           
        for (int i = 0; i < gemTypeCount; i++)
        {
            Transform gemUI = Instantiate(gemUIPrefab, content).transform;
            //Name
            gemUI.name = gemManager.gemStruck[i].name;

            //Icon
            gemUI.GetComponent<Image>().sprite = gemManager.gemStruck[i].icon;

            //TMP
            TextMeshProUGUI[] texts = gemUI.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = gemManager.gemStruck[i].name; //name
            texts[1].text = PlayerPrefs.GetInt(gemManager.gemStruck[i].name).ToString(); //value

            //TPM Color
            Color tempColor = Color.white;
            gemUI.GetComponentInChildren<TextMeshProUGUI>().color = tempColor;
            gemUI.GetComponentInChildren<TextMeshProUGUI>().colorGradient = new TMPro.VertexGradient(gemManager.gemStruck[i].lineColor, tempColor, gemManager.gemStruck[i].lineColor, tempColor);

            //Line
            gemUI.Find("Line").GetComponent<Image>().color = gemManager.gemStruck[i].lineColor;
        }
    }
    
    public void UpdateUI()
    {
        for (int i = 0; i < gemTypeCount; i++)
        {            
            TextMeshProUGUI[] texts = content.GetChild(i).GetComponentsInChildren<TextMeshProUGUI>();            
            texts[1].text = PlayerPrefs.GetInt(gemManager.gemStruck[i].name).ToString(); //value
        }
    }
    
#region >>  GUI  <<
#if UNITY_EDITOR
    [CustomEditor(typeof(UIManager))]
    public class UIManagerGUI : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            UIManager uiManager = (UIManager)target;

            GUILayout.Space(10);
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Delete Saved UI Data\r\n", GUILayout.Width(160), GUILayout.Height(40)))
                PlayerPrefs.DeleteAll();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }
    }
#endif
#endregion
}