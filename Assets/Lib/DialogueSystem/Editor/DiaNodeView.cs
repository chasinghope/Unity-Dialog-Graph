using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using System;

public class DiaNodeView : UnityEditor.Experimental.GraphView.Node
{

    public Action<DiaNodeView> OnNodeSelected;

    public DiaNode node;
    public Port inputPort;
    public List<Port> outputPorts;

    //public Port outputPort { get; private set; }

    public DiaNodeView(DiaNode node)
    {
        this.node = node;
        this.title = node.Title;
        this.viewDataKey = node.Guid;

        style.left = node.position.x;
        style.top = node.position.y;
        outputPorts = new List<Port>();

        CreateInputPorts();
        CreateOutputPorts();
    }




    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
        evt.menu.AppendAction("添加输出端口", (a) => AddOutputPort());
        evt.menu.AppendAction("删除未连接输出端口", (a) => RemoveDisconnectPorts());
    }

    private void RemoveDisconnectPorts()
    {
        List<Port> disconnectPorts = outputPorts.FindAll(port=>{
            int count = 0;
            foreach (var item in port.connections)
            {
                count++;
            }
            return count == 0 ? true : false;
        });
        foreach (var item in disconnectPorts)
        {
            outputPorts.Remove(item);
            outputContainer.Remove(item);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
            OnNodeSelected.Invoke(this);
    }

    void CreateOutputPorts()
    {

        if(node.Output.Count == 0)
        {
            Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            outputPort.portName = "next";
            outputPort.portColor = new Color(0 / 255f, 180 / 255f, 235 / 255f);
            outputContainer.Add(outputPort);
        }

        foreach (var item in node.Output)
        {
            Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            outputPort.portName = item.Descirbe;
            outputPort.portColor = new Color(0 / 255f, 180 / 255f, 235 / 255f);
            outputContainer.Add(outputPort);
            outputPorts.Add(outputPort);
        }
    }

    void CreateInputPorts()
    {
        if(inputPort == null)
        {
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "enter";
            inputPort.portColor = new Color(50/255f, 205/255f, 50/255f);
            inputContainer.Add(inputPort);
        }
    }

    void AddOutputPort()
    {
        Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        outputPort.portName = "next";
        outputPort.portColor = new Color(0 / 255f, 180 / 255f, 235 / 255f);
        outputContainer.Add(outputPort);
        outputPorts.Add(outputPort);
    }

}
