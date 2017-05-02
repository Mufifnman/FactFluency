using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperatorSlot : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private Operator currentOp;

    private void Start()
    {
        if (this.text == null)
        {
            throw new InvalidOperationException("Text field not set, plase set in editor!");
        }
    }

    public void SetOperator(Operator o)
    {
        this.text.text = o.ToOperatorString();
        this.currentOp = o;
    }

    public Operator GetOperator()
    {
        return this.currentOp;
    }
}
