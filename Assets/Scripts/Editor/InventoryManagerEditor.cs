using UnityEngine;
using UnityEditor;
using Gemier.Managers;

[CustomEditor(typeof (InventoryManager))]
[CanEditMultipleObjects]
public class InventoryManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        InventoryManager im = (InventoryManager)target;

        if(GUILayout.Button("Save"))
        {
            im.SaveInventory();
        }
        if(GUILayout.Button("Delete Save"))
        {
            im.DeleteSave();
        }
    }
}