using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

public class DiaInspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<DiaInspectorView, VisualElement.UxmlTraits> { }
    Editor editor;

    public DiaInspectorView()
    {

    }

    public void UpdateSelection(DiaNodeView nodeView)
    {
        if (nodeView == null)
            return;

        Clear();
        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(nodeView.node);
        IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
        Add(container);
    }
}
