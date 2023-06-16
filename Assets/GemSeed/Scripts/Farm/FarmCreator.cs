using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FarmCreator : MonoBehaviour
{
    #region Variables
    [SerializeField] private Farm farm;
    private List<Farm> farms = new List<Farm>();
    #endregion

    public void CreateFarm()
    {
        farms.Add(Instantiate(farm, transform.position, Quaternion.identity, transform));
        farms.Last().name = "Farm " + farms.Count;

#if UNITY_EDITOR
        FarmGUI manipulateInspector = new FarmGUI();
        manipulateInspector.farm = farms[farms.Count - 1];
#endif
    }

    private void OnDrawGizmos()
    {
        if (farms.Count > 0 && farms.Last() == null) farms.Remove(farms.Last());
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(FarmCreator))]
    public class FarmCreatorInspactor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            FarmCreator farmCreator = (FarmCreator)target;

            GUILayout.Space(10);
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Create Farm", GUILayout.Width(160), GUILayout.Height(40)))
                farmCreator.CreateFarm();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }
    }
#endif
}
