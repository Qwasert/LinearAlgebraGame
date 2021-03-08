using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle03Window : WindowRoot
{
    public bool isInit;
    public Text txtInstruction;
    public Text txtChoice1;
    public Text txtChoice2;
    public Text txtFBChoice1;
    public Text txtFBChoice2;
    public int choiceID1 = 0;
    public int choiceID2 = 0;
    public Vector3 choice1Pos;
    public Vector3 choice2Pos;
    public Transform panelChoice;
    public Transform[] panelChoiceList;

    public List<List<ChoiceClickButton>> BtnChoices = new List<List<ChoiceClickButton>>();
    public List<ChoiceClickButton> BtnChoices1;
    public List<ChoiceClickButton> BtnChoices2;
    public List<ChoiceClickButton> BtnChoices3;

    public GameControllerPuzzle03 GCP03;

    private void Start()
    {
        if(isInit)
        {
            InitWindow();
        }
    }

    protected override void InitWindow()
    {
        Debug.Log("Init Puzzle01 window");
        base.InitWindow();

        BtnChoices.Add(BtnChoices1);
        BtnChoices.Add(BtnChoices2);
        BtnChoices.Add(BtnChoices3);

        GCP03 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle03>();
        GCP03.InitGameController(this);

        ClearSpanValues("", "");
        ClearFeedbackPanel();
    }

    public void SetSpanValue(Vector3 spanValue, int choiceID)
    {
        if(choiceID1 == choiceID || choiceID2 == choiceID)
        {
            Debug.Log("You Already Selected");
        }

        if (choiceID1 == 0)
        {
            txtChoice1.text = spanValue[0] + "\n" + spanValue[1] + "\n" + spanValue[2];
            choiceID1 = choiceID;
            choice1Pos = spanValue;
        }
        else if (choiceID2 == 0)
        {
            txtChoice2.text = spanValue[0] + "\n" + spanValue[1] + "\n<color=red>" + spanValue[2] + "</color>";
            choiceID2 = choiceID;
            choice2Pos = spanValue;
        }
        else
        {
            Debug.Log("Full");
        }
    }

    public void ClickSubmitBtn()
    {
        GCP03.TriggerRotation(choiceID1, choiceID2, choice1Pos, choice2Pos);

        //Show the span value in input panel
        /*if(choiceID1 != 0 && choiceID2 != 0)
        {
            txtFBChoice1.text = choice1Pos.x + "\n" + choice1Pos.y + "\n" + choice1Pos.z;
            txtFBChoice2.text = choice2Pos.x + "\n" + choice2Pos.y + "\n" + choice2Pos.z;
        }*/

        ClearSpanValues(txtFBChoice1.text, txtChoice2.text);

        GameRoot.instance.audioService.PlayUIAudio(Constants.audioP03Click);
    }

    public void ClickClearChoice1Btn()
    {/*
        GCP03.ReactivateChoiceBtn(choiceID1);
        txtChoice1.text = "";
        choiceID1 = 0;
        choice1Pos = Vector3.zero;


        GameRoot.instance.audioService.PlayUIAudio(Constants.audioP03ClickClear);*/
    }

    public void ClickClearChoice2Btn()
    {
        GCP03.ReactivateChoiceBtn(choiceID2);
        txtChoice2.text = "";
        choiceID2 = 0;
        choice2Pos = Vector3.zero;

        GameRoot.instance.audioService.PlayUIAudio(Constants.audioP03ClickClear);
    }

    public void ClearFeedbackPanel()
    {
        txtFBChoice1.text = "1\n0\n0";
        txtFBChoice2.text = "0\n1\n<color=red>b</color>";
    }


    public void ClearSpanValues(string choice1, string choice2)
    {
        GCP03.ReactivateChoiceBtn(choiceID1);
        GCP03.ReactivateChoiceBtn(choiceID2);
        txtChoice1.text = choice1;
        txtChoice2.text = choice2;
        choiceID1 = 0;
        choiceID2 = 0;
        choice1Pos = Vector3.zero;
        choice2Pos = Vector3.zero;

        Debug.Log("Test");
    }

    public void ShowChoicePanel(bool status)
    {
        Animation panelChoiceAni = panelChoice.GetComponent<Animation>();

        if (status)
        {
            panelChoiceAni.Play("ChoiceShow");
        }
        else
        {
            panelChoiceAni.Play("ChoiceHide");
        }
    }

    public void ClickChoiceBtn(string value)
    {
        string[] valueArray = value.Split(',');

        SetSpanValue(new Vector3(
                                        (float)Convert.ToDouble(valueArray[0]),
                                        (float)Convert.ToDouble(valueArray[1]),
                                        (float)Convert.ToDouble(valueArray[2])),
                                        Convert.ToInt32(valueArray[3]));
    }

    public void SetPanelChoice(int puzzleID)
    {
        panelChoice.gameObject.SetActive(false);

        panelChoice = panelChoiceList[puzzleID];

        panelChoice.gameObject.SetActive(true);
    }

    public void SetInstructionalText(string content)
    {
        txtInstruction.text = content;
    }
}
