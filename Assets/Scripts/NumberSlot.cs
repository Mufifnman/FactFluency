using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSlot : MonoBehaviour
{
    private static string NoNumber = "_";
    [SerializeField]
    private Text text;


    private void Start()
    {
        if (this.text == null)
        {
            throw new InvalidOperationException("Text field not set, plase set in editor!");
        }
    }

    // Null refers to blank
    public void SetNumber(int? i)
    {
        if (!i.HasValue)
        {
            this.text.text = NoNumber;
            return;
        }

        //TODO Check GameState here to make sure we don't pass an ilegal number
        this.text.text = i.ToString();
    }

    public void SetColor(Color c)
    {
        this.text.color = c;
    }

    public int? GetNumber()
    {
        var filteredNum = int.Parse(text.text).ToString();

        if (string.IsNullOrEmpty(filteredNum))
        {
            return null;
        }

        return int.Parse(filteredNum);
    }
}
