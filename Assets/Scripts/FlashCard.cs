using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FlashCard : MonoBehaviour 
{
    [SerializeField]
	private NumberSlot firstNumnber;
    [SerializeField]
    private OperatorSlot op;
    [SerializeField]
    private NumberSlot secondNumber;
    [SerializeField]
    private OperatorSlot equals;
    [SerializeField]
    private NumberSlot answer;

    [SerializeField]
    private bool StartInputActivated = true;
    private bool InputActivated = true;

    private StringBuilder inputBuilder = new StringBuilder();
    // InputTimer;
    private bool recievingInput;
    public float InputTimeout;
    private float lastInputRecievedTime;

    //TODO: move to larger settings
    public Color Correct = Color.green;
    public Color Incorrect = Color.red;
    public Color Neutral = Color.black;

    [SerializeField]
    private bool resetQuestion = true;
    private System.Random rnd = new System.Random();

	void Start ()
    {
        //ToDo try to find these is they aren't set
        if (firstNumnber == null || op == null || secondNumber == null || equals == null || answer == null)
        {
            throw new InvalidOperationException("Flash card Object is missing a componeent, please set in Unity editor");
        }

        this.equals.SetOperator(global::Operator.Equals);

        this.InputActivated = StartInputActivated;
	}
	
	void Update ()
    {
        if (this.InputActivated)
        {
            //Update Input
            //ToDo check for enter
            string inputStr = Input.inputString;

            if (!String.IsNullOrEmpty(inputStr))
            {
                int input;
                if (int.TryParse(Input.inputString, out input))
                {
                    var filteredInput = input.ToString();

                    recievingInput = true;
                    inputBuilder.Append(filteredInput);
                    this.lastInputRecievedTime = Time.time;

                    this.SetNeutralAnswer(int.Parse(inputBuilder.ToString()));
                }
            }

            if (Input.GetKeyUp(KeyCode.Backspace)) // ToDo make this support holding the key down to deleter a bunch 
            {
                if (this.inputBuilder.Length > 0)
                {
                    this.inputBuilder.Length = inputBuilder.Length - 1;
                    this.lastInputRecievedTime = Time.time;
                }

                if (this.inputBuilder.Length == 0)
                {
                    this.SetNeutralAnswer(null);
                    this.ResetInputCapture();
                }
                else
                {
                    this.SetNeutralAnswer(int.Parse(inputBuilder.ToString()));
                }
            }

            if (this.recievingInput && (Time.time - this.lastInputRecievedTime > this.InputTimeout)
                || Input.GetKeyDown(KeyCode.Return))
            {
                this.CheckInputAnswer();
            }
        }

        if(this.resetQuestion || Input.GetKeyDown(KeyCode.R))
        {
            this.ResetQuestion();
            this.resetQuestion = false;
        }
    }

    public void InputActivate()
    {
        this.InputActivated = true;
    }

    public void InputDeactivate()
    {
        this.CheckInputAnswer();
        this.InputActivated = false;
    }

    public void ResetQuestion()
    {
        //ToDo check for Range
        this.firstNumnber.SetNumber(rnd.Next(0, 10));
        this.secondNumber.SetNumber(rnd.Next(0, 10));

        //ToDo add other operations
        this.op.SetOperator(Operator.Plus);

        this.SetNeutralAnswer(null);

        ResetInputCapture();
    }

    private void ResetInputCapture()
    {
        // Reset Input 
        this.inputBuilder.Length = 0;
        this.inputBuilder.Append("");
        this.recievingInput = false;
    }

    private void CheckInputAnswer()
    {
        var inputSoFar = inputBuilder.ToString();
        if (string.IsNullOrEmpty(inputSoFar))
        {
            this.SetNeutralAnswer(null);
        }
        else
        {
            // ToDo: fix: Could be 0 (or something) if a non number sneaks in somehow...
            this.CheckAnswer(int.Parse(inputSoFar));
        }
    }

    private void CheckAnswer(int a)
    {
        // first ensure you have numbers set
        if (!firstNumnber.GetNumber().HasValue || !secondNumber.GetNumber().HasValue)
        {
            throw new InvalidOperationException("Numbers in the flash card not set up to calculate!");
        }

        int first = firstNumnber.GetNumber().Value;
        int second = secondNumber.GetNumber().Value;

        int correctAnswer = int.MinValue;
        switch (this.op.GetOperator())
        {
            case Operator.Plus:
                correctAnswer = first + second;
                break;
            case Operator.Minus:
                correctAnswer = first - second;
                break;
            case Operator.Times:
                correctAnswer = first * second;
                break;
            case Operator.Devide:
                if (second == 0)
                {
                    throw new InvalidOperationException("Attempt to devide by zero!");
                }
                correctAnswer = first / second;
                break;
            default:
                throw new InvalidOperationException("Operation on flash card set to invalid operation!");
        }

        if (correctAnswer == int.MinValue)
        {
            throw new Exception("Unspecified error (how did we get here?? :p)");
        }

        if (a == correctAnswer)
        {
            this.SetSuccessAnswer(a);
        }
        else
        {
            this.SetFailAnswer(a);
        }
    }

    private void SetSuccessAnswer(int a)
    {
        answer.SetNumber(a);
        answer.SetColor(Correct);
    }

    private void SetFailAnswer(int a)
    {
        answer.SetNumber(a);
        answer.SetColor(Incorrect);
    }

    private void SetNeutralAnswer(int? a)
    {
        answer.SetNumber(a);
        answer.SetColor(Neutral);
    }
}


