using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

#if UNITY_EDITOR
[CustomEditor(typeof(Farm))]
public class FarmGUI : Editor
{
    public Farm farm;
     

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        farm = (Farm)target;
        
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        #region Build Farm
        if (!farm.built && GUILayout.Button("Build Farm", GUILayout.Width(200), GUILayout.Height(50)))
            farm.BuildFarm();
        else if (farm.built)
        {
            if (GUILayout.Button("Destroy Farm", GUILayout.Width(200), GUILayout.Height(50)))
                farm.DestroyFarm();
            if (GUILayout.Button("Edit Farm", GUILayout.Width(200), GUILayout.Height(50)))
                farm.EditFarm();
        } 
        #endregion

        #region Edit Farm
        /*GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();*/

        

        /*GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();*/
        #endregion

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();
    }
}
#endif