using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInput : MonoBehaviour
{
    public DialogueCanvas diaCanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            diaCanvas.DoAction_Dialogue();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            diaCanvas.DoAction_ContinueTalk();
        }
    }
}
