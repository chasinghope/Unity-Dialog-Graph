using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueCanvas : MonoBehaviour
{
    DiaComponent diaComponent;

    [Header("Chat Scroll View")]
    public Transform Content;
    public DialogueTemplateView otherTemplateView;
    public DialogueTemplateView selfTemplateView;
    public Scrollbar charBar;
    public ScrollRect charRect;

    [Header("Reply Scroll View")]
    public Transform ReplyContent;
    public DialogueReplyView replyView;
    public GameObject ReplyPanel;

    [Header("Player Setting")]
    public string PlayerName;
    public Sprite PlayerIcon;

    private Canvas myCanvas;

    private RectTransform refreshRect;


    /// <summary>
    /// 是否在等待下一句话
    /// </summary>
    public bool IsWaittingNext = false;

    public bool IsTaking = false;


    void Awake()
    {
        myCanvas = this.GetComponent<Canvas>();
        refreshRect = Content.GetComponent<RectTransform>();
        diaComponent = Transform.FindObjectOfType<DiaComponent>();
        ReplyPanel.SetActive(false);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q) && !IsTaking)
    //    {
    //        myCanvas.enabled = true;
    //        diaComponent = Transform.FindObjectOfType<DiaComponent>();
    //        CreateDiaContent(diaComponent.startDiaNode);
    //        IsTaking = true;
    //    }

    //    if (Input.GetKeyDown(KeyCode.Space) && IsWaittingNext)
    //    {
    //        IsWaittingNext = false;
    //        GoNextDialogue();
    //    }
    //}


    /// <summary>
    /// 开启对话
    /// </summary>
    public void DoAction_Dialogue()
    {
        if (!IsTaking)
        {
            myCanvas.enabled = true;
            CreateDiaContent(diaComponent.startDiaNode);
            IsTaking = true;
        }
    }

    /// <summary>
    /// 继续对话
    /// </summary>
    public void DoAction_ContinueTalk()
    {
        if (IsWaittingNext)
        {
            IsWaittingNext = false;
            GoNextDialogue();
        }
    }



    /// <summary>
    /// 根据回答前往下一句话
    /// </summary>
    /// <param name="index"></param>
    private void GoNextDialogueByReply(int index)
    {
        if (diaComponent == null)
            return;
        DiaNode diaNode = diaComponent.FindNextNode(index);
        if (diaNode)
        {
            CreateDiaContent(diaNode);
            IsWaittingNext = true;
        }
        else
        {
            ClearDialogueView();
        }
    }

    /// <summary>
    /// 前往下一句话
    /// </summary>
    private void GoNextDialogue()
    {
        if (diaComponent == null)
            return;
        DiaNode diaNode = diaComponent.FindNextNode(0);
        if (diaNode)
        {
            CreateDiaContent(diaNode);
        }
        else
        {
            ClearDialogueView();
        }
    }

    /// <summary>
    /// 打印对话内容
    /// </summary>
    /// <param name="startDiaNode"></param>
    private void PrintDiaContent(DiaNode diaNode)
    {
        Debug.Log(diaNode.Text);

        if(diaNode.Type == DiaType.PersonalityAdjudication)
        {
            for (int i = 0; i < diaNode.Output.Count; i++)
            {
                if (!string.IsNullOrEmpty(diaNode.Output[i].Answer))
                    Debug.Log($"{i+1}: {diaNode.Output[i].Answer}");
            }
        }

    }


    /// <summary>
    /// 创建对话内容
    /// </summary>
    /// <param name="diaNode"></param>
    private void CreateDiaContent(DiaNode diaNode)
    {


        switch (diaNode.Type)
        {
            case DiaType.PersonalityNarration:

                OtherSay(diaNode);
                IsWaittingNext = true;
                break;
            case DiaType.PersonalityAdjudication:
                OtherSay(diaNode);
                //清理对话选项
                DialogueReplyView[] replys = ReplyContent.transform.GetComponentsInChildren<DialogueReplyView>();
                foreach (var item in replys)
                {
                    GameObject.DestroyImmediate(item.gameObject);
                }
                //打开回复面板
                ReplyPanel.SetActive(true);

                for (int i = 0; i < diaNode.Output.Count; i++)
                {
                    if (!string.IsNullOrEmpty(diaNode.Output[i].Answer))
                    {
                        //Debug.Log($"{i + 1}: {diaNode.Output[i].Answer}");
                        DialogueReplyView reply = Instantiate(replyView.gameObject).GetComponent<DialogueReplyView>();
                        reply.transform.SetParent(ReplyContent);
                        reply.CreateDiaReplyView(i + 1, diaNode.Output[i].Answer);
                        reply.gameObject.SetActive(true);
                        reply.OnSendMessage += SendReply;

                        reply.RefreshLayout();

                    }
                }
                break;
            case DiaType.Lead:

                SelfSay(diaNode);
                IsWaittingNext = true;

                break;
            case DiaType.NPC:
                break;
            default:
                break;
        }

        charRect.verticalNormalizedPosition = 0;
        //charBar.value = 0;
        //charRect.verticalNormalizedPosition 
    }



    private void OtherSay(DiaNode diaNode)
    {
        DialogueTemplateView other = Instantiate(otherTemplateView.gameObject).GetComponent<DialogueTemplateView>();
        other.transform.SetParent(Content);
        other.SpeakerImg.sprite = diaNode.Icon;
        other.NameText.text = diaNode.Name;
        other.ContentText.text = diaNode.Text;
        other.gameObject.SetActive(true);


        other.RefreshLayout();
        LayoutRebuilder.ForceRebuildLayoutImmediate(refreshRect);
    }


    private void SelfSay(DiaNode diaNode)
    {
        DialogueTemplateView self = Instantiate(selfTemplateView.gameObject).GetComponent<DialogueTemplateView>();
        self.transform.SetParent(Content);
        self.SpeakerImg.sprite = PlayerIcon;
        self.NameText.text = PlayerName;
        self.ContentText.text = diaNode.Text;
        self.gameObject.SetActive(true);


        self.RefreshLayout();
        LayoutRebuilder.ForceRebuildLayoutImmediate(refreshRect);
    }


    private void SendReply(string message, int index)
    {
        ReplyPanel.SetActive(false);
        DialogueTemplateView self = Instantiate(selfTemplateView.gameObject).GetComponent<DialogueTemplateView>();
        self.transform.SetParent(Content);
        self.SpeakerImg.sprite = PlayerIcon;
        self.NameText.text = PlayerName;
        self.ContentText.text = message;
        self.gameObject.SetActive(true);



        self.RefreshLayout();

        GoNextDialogueByReply(index);
    }


    private void ClearDialogueView()
    {
        myCanvas.enabled = false;
        IsTaking = false;

        diaComponent.ResetDiaTree();

        DialogueReplyView[] replys = ReplyContent.transform.GetComponentsInChildren<DialogueReplyView>();
        foreach (var item in replys)
        {
            GameObject.DestroyImmediate(item.gameObject);
        }

        DialogueTemplateView[] dialogues = Content.transform.GetComponentsInChildren<DialogueTemplateView>();
        foreach (var item in dialogues)
        {
            GameObject.DestroyImmediate(item.gameObject);
        }

    }

}
