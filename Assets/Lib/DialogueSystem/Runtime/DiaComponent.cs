using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaComponent : MonoBehaviour
{
    [Header("¶Ô»°Ê÷")]
    public DiaTree DiaInfo;

    public DiaNode startDiaNode;
    private DiaNode currentDiaNode;

    private void Start()
    {
        currentDiaNode = DiaInfo.root;
        startDiaNode = DiaInfo.root;
    }



    public DiaNode FindNextNode(int index)
    {
        if (currentDiaNode.Output.Count == 0)
            return null;
        currentDiaNode = currentDiaNode.Output[index].NextNode;
        return currentDiaNode;
    }

    public void ResetDiaTree()
    {
        currentDiaNode = startDiaNode;
    }

}
