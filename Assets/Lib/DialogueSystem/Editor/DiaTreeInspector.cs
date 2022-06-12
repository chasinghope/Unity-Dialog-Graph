using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(DiaTree))]
public class DiaTreeInspector : Editor
{
    private DiaTree diaTree;
    private VisualElement rootElement;
    private VisualTreeAsset visualTree;

    private void OnEnable()
    {
        diaTree = target as DiaTree;
        rootElement = new VisualElement();
        visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Lib/DialogueSystem/Editor/DiaTreeInspector.uxml");
    }

    public override VisualElement CreateInspectorGUI()
    {
        var root = rootElement;
        root.Clear();

        visualTree.CloneTree(root);
        var btn = root.Q<Button>("Button");
        btn.clicked += () => {
            DiaTreeEditorWindow window = (DiaTreeEditorWindow)EditorWindow.GetWindow(typeof(DiaTreeEditorWindow));
            window.Show(); 
        };
        var des = root.Q<TextField>("Describe");

        des.bindingPath = "Info";



        return root;
    }


}
