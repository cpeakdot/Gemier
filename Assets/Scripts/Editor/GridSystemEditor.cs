using UnityEngine;
using UnityEditor;
using Gemier.GridSpace;

[CustomEditor(typeof (GridSystem))]
public class GridSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GridSystem gridSystem = (GridSystem)target;

        if(GUILayout.Button("Generate Grid"))
        {
            gridSystem.InitGrid();
        }
        if(GUILayout.Button("Clear This Grid"))
        {
            gridSystem.ClearGrid();
        }
        if(GUILayout.Button("Clear All & Generate Grid"))
        {
            gridSystem.InitGridClear();
        }
        if(GUILayout.Button("Clear All Grids"))
        {
            gridSystem.ClearAllGrids();
        }
    }
}
