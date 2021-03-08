using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerPuzzle03 : GameControllerRoot
{
    // Found at 68, 1, -12 in hub

    public Camera[] topCameraList;
    public float rotatedSpeed = 1f;
    public Transform plane;
    public Transform tipsPoint2, tipsPoint3;
    public float tipsAnimationSpeed = 0.01f;
    public float planeShiftSpeed = 0.01f;
    public List<ChoiceClickButton> BtnChoices;
    public int choicesAmount = 0;
    public bool[] subPuzzleComplete = { false, false, false };
    public Transform playerPosition;
    public Transform[] playerPositionList;
    public Transform endWall;       //When Player Finish all sub-level, the endWall will inactive

    [HideInInspector]
    public Puzzle03Window P03W;
    [HideInInspector]
    public DatabasePuzzle03 DBP03;

    public bool isInQuestion = false;
    public bool isTriggerQuestion = false;

    [HideInInspector]
    public Vector3 finalRotation;

    private Camera topCamera;
    private Vector3 currentRotation;
    private Vector3 diffRotation;
    private bool isRotate = false;
    private bool isFinded = false;

    private Vector3 tipsPoint2FinalPos;
    private Vector3 tipsPoint3FinalPos;
    private bool isTipsAnimate = false;

    private int subPuzzleID = 1;
    private bool isShiftPlane = false;

    private GameObject player;
    private Vector3 startPosition;
    private Vector3 previousPosition;
    private int timer = 0;

    public override void InitGameController(Puzzle03Window P03W)
    {
        Debug.Log("Init GameController Puzzle03");
        base.InitGameController();

        Debug.Log("Connect Puzzle03 Window");
        this.P03W = P03W;

        Debug.Log("Connect Database of Puzzle03");
        DBP03 = GetComponent<DatabasePuzzle03>();

        Debug.Log("Call Database of Puzzle03 to connect");
        DBP03.InitDatabase();

        if(DialogueManager.showP03_00)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle03_00"));
            DialogueManager.showP03_00 = false;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        startPosition = player.transform.localPosition;

        //Get all the buttons of first sub-buttons from window
        BtnChoices = P03W.BtnChoices[0];

        //Initilize Components
        plane = DBP03.subLevelPlanes[0];
        topCamera = topCameraList[0];
        P03W.SetInstructionalText("Choose a number to assign 'b' a new value. The environment will then imitate the newly generated span.");
    }

    private void Update()
    {
        if(choicesAmount == 0)
        {
            SetSpanValue(Vector3.right, 5);
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.transform.localPosition= startPosition;

            GameRoot.instance.IsLock(false);
            P03W.ShowChoicePanel(false);
            isTriggerQuestion = false;

            GameRoot.ShowTips("", true, false);
        }

        if(isRotate)
        {
            if (Mathf.Abs(diffRotation.x) <= 2f && Mathf.Abs(diffRotation.y) <= 2f && Mathf.Abs(diffRotation.z) <= 2f)
            {
                plane.transform.eulerAngles = finalRotation;
                isRotate = false;

                //Stop audio FX rotated puzzle environment
                plane.GetComponent<PuzzleEnvironmentController>().PlayRotatedSoundFX(false);
            }
            else
            {
                CalDiffRotation();

                currentRotation = Vector3.Slerp(currentRotation, finalRotation, 0.01f * rotatedSpeed);

                plane.transform.eulerAngles = currentRotation;
            }
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            topCamera.depth = topCamera.depth == 0 ? 1 : 0;
        }

        //*********************** Interaction with the trigger and submission *************************

        if(isInQuestion && Input.GetKeyDown(KeyCode.E))
        {
            if(isTriggerQuestion)
            {
                GameRoot.instance.IsLock(false);
                GameRoot.isPuzzleLock = false;
                P03W.ShowChoicePanel(false);
                isTriggerQuestion = false;

                GameRoot.instance.audioService.PlayUIAudio(Constants.audioP03ExitQuestion);
            }
            else
            {
                isTriggerQuestion = true;
                GameRoot.instance.IsLock(true);
                GameRoot.isPuzzleLock = true;
                P03W.ShowChoicePanel(true);

                GameRoot.instance.audioService.PlayUIAudio(Constants.audioP03TriggerQuestion);
            }
        }
        // ***********************************************************************************************

        if(isTipsAnimate)
        {
            tipsPoint2.localPosition = Vector3.Lerp(tipsPoint2.localPosition, tipsPoint2FinalPos, tipsAnimationSpeed);
            tipsPoint3.localPosition = Vector3.Lerp(tipsPoint3.localPosition, tipsPoint3FinalPos, tipsAnimationSpeed);

            Vector3 diffTP2 = tipsPoint2FinalPos - tipsPoint2.localPosition;
            Vector3 diffTP3 = tipsPoint3FinalPos - tipsPoint3.localPosition;

            if((Mathf.Abs(diffTP2.x) <= 0.05f && Mathf.Abs(diffTP2.y) <= 0.05f && Mathf.Abs(diffTP2.z) <= 0.05f) &&
               (Mathf.Abs(diffTP3.x) <= 0.05f && Mathf.Abs(diffTP3.y) <= 0.05f && Mathf.Abs(diffTP3.z) <= 0.05f))
            {
                tipsPoint2.localPosition = tipsPoint2FinalPos;
                tipsPoint3.localPosition = tipsPoint3FinalPos;

                isTipsAnimate = false;
            }
        }

        //When player finish sub-level, the puzzle environment shift into next sub-level
        if(isShiftPlane)
        {
            playerPosition.position = Vector3.Lerp(playerPosition.position, playerPositionList[subPuzzleID].position, planeShiftSpeed);

            Vector3 diffPP = playerPositionList[subPuzzleID].position - playerPosition.position;

            if(Mathf.Abs(diffPP.z) <= 0.05f)
            {
                if(subPuzzleID >= DBP03.subLevelPlanes.Length)
                {
                    playerPosition.position = playerPositionList[subPuzzleID].position;
                    SetActive(endWall, false);
                    P03W.ShowChoicePanel(false);

                    GameRoot.instance.IsLock(false);
                    GameRoot.ShowTips("", false, false);
                    isInQuestion = false;
                    P03W.SetInstructionalText("Go to the exit.");
                }

                isShiftPlane = false;
            }
        }

        previousPosition = player.transform.position;
    }

    private void CalDiffRotation()
    {
        currentRotation = plane.transform.eulerAngles;

        currentRotation.x += currentRotation.x > 180f ? -360f : 0f;
        currentRotation.y += currentRotation.y > 180f ? -360f : 0f;
        currentRotation.z += currentRotation.z > 180f ? -360f : 0f;

        diffRotation = finalRotation - currentRotation;
    }

    public void TriggerRotation(int ID1, int ID2, Vector3 tipsPoint2Pos, Vector3 tipsPoint3Pos)
    {
        foreach(SpanValue spanValue in DBP03.spanValues)
        {
            if((ID1 == spanValue.choiceID1 || ID1 == spanValue.choiceID2) && (ID2 == spanValue.choiceID1 || ID2 == spanValue.choiceID2))
            {
                finalRotation = new Vector3(spanValue.x, spanValue.y, spanValue.z);
                isFinded = true;
                break;
            }
        }

        if(isFinded)
        {
            SetTipsPointsValue(tipsPoint2Pos, tipsPoint3Pos);

            CalDiffRotation();

            isRotate = true;

            //Activate audio FX rotated puzzle environment
            plane.GetComponent<PuzzleEnvironmentController>().PlayRotatedSoundFX(true);
        }
        else
        {
            Debug.Log("Please Try again");
        }

        isFinded = false;
    }
    
    
    public void SetSpanValue(Vector3 spanValue, int choiceID)
    {
        P03W.SetSpanValue(spanValue, choiceID);

        choicesAmount += 1;
    }

    public void SetTipsPointsValue(Vector3 tipsPoint2Pos, Vector3 tipsPoint3Pos)
    {
        tipsPoint2FinalPos = new Vector3(tipsPoint2Pos.x, tipsPoint2Pos.z, tipsPoint2Pos.y);
        tipsPoint3FinalPos = new Vector3(tipsPoint3Pos.x, tipsPoint3Pos.z, tipsPoint3Pos.y);
        isTipsAnimate = true;
    }

    public void ReactivateChoiceBtn(int choiceID)
    {
        foreach(ChoiceClickButton T in BtnChoices)
        {
            if(choiceID == T.choiceID)
            {
                T.GetComponent<Button>().interactable = true;

                choicesAmount -= 1;
            }
        }
    }

    public void FinishSubLevel(int subPuzzleID)
    {
        this.subPuzzleID = subPuzzleID;

        topCamera.depth = 0;

        //Stop audio FX rotated puzzle environment
        plane.GetComponent<PuzzleEnvironmentController>().PlayRotatedSoundFX(false);

        isRotate = false;

        if (subPuzzleID < DBP03.subLevelPlanes.Length)
        {
            topCamera = topCameraList[subPuzzleID];
            BtnChoices = P03W.BtnChoices[subPuzzleID];
            P03W.SetPanelChoice(subPuzzleID);

            DBP03.SetPuzzleActive(subPuzzleID);
            plane = DBP03.GetSubLevelPlanes(subPuzzleID);
        }

        finalRotation = Vector3.zero;

        GameRoot.instance.IsLock(false);
        P03W.ShowChoicePanel(false);
        P03W.ClearSpanValues("test2", "test2");
        P03W.ClearFeedbackPanel();
        isTriggerQuestion = false;

        isShiftPlane = true;
    }
}
