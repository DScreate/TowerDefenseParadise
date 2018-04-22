using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TowerController))]
public class LevelScriptEditor : Editor {
    public void OnSceneGUI()
    {
        TowerController controller = (TowerController)target;

        Handles.color = Color.red;
        

        Handles.DrawWireArc(controller.transform.position, Vector3.up, Vector3.forward, 360, controller.tower.range / 2f + 0.27f);

    //myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
    //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
    }
}
