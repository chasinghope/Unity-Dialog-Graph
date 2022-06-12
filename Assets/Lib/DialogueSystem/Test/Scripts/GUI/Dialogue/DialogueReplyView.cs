using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueReplyView : MonoBehaviour
{ 
    public Text InfoText;
    public DiaButton MyButton;
    public Color DefaultColor;
    public Color HightColor;

    private string Message;
    private int Index; // Ë÷Òý
    public event Action<string, int> OnSendMessage;
    private List<RectTransform> layoutElements;

    private void Awake()
    {
        var layouts = new List<LayoutGroup>(GetComponentsInChildren<LayoutGroup>());
        layoutElements = new List<RectTransform>();
        foreach (var item in layouts)
        {
            layoutElements.Add(item.GetComponent<RectTransform>());
        }

        InfoText.color = DefaultColor;
        MyButton.onEnter += () => { InfoText.color = HightColor; };
        MyButton.onExit += () => { InfoText.color = DefaultColor; };
        MyButton.onClick.AddListener(SendMessage);

    }

    private void OnDestroy()
    {
        OnSendMessage = null;
    }


    private void SendMessage()
    {
        if (OnSendMessage != null)
            OnSendMessage.Invoke(Message, Index);
    }


    public void CreateDiaReplyView(int index, string message)
    {
        InfoText.text = $"{index}. {message}";
        Message = message;
        Index = index - 1;
    }

    public void RefreshLayout()
    {
        foreach (var item in layoutElements)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
        }
    }
}
