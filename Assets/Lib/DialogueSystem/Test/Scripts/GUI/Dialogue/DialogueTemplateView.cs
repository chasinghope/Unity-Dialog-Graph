using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTemplateView : MonoBehaviour
{
    public Image SpeakerImg;
    public Text NameText;
    public Text ContentText;


    private List<RectTransform> layoutElements;

    public void Awake()
    {
        var layouts = new List<LayoutGroup>(GetComponentsInChildren<LayoutGroup>());
        layoutElements = new List<RectTransform>();
        foreach (var item in layouts)
        {
            layoutElements.Add(item.GetComponent<RectTransform>());
        }

    }

    public void RefreshLayout()
    {
        foreach (var item in layoutElements)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
        }
    }
}
