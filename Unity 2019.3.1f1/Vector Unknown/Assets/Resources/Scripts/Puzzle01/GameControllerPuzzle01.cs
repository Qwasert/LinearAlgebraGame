using UnityEngine;

public class GameControllerPuzzle01 : GameControllerRoot
{
    public GameObject topCamPlayer;

    [Header("Environment Components")]
    public Transform sunLight;
    public Transform moonLight;
    public LPWAsset.LowPolyWaterScript ocean;
    public Material night;

    [Header("Question Trigger Components")]
    public int questionNum;
    public bool isInQues = false;
    public bool isTriggerQuestion = false;
    public bool isAnswerCorrect = true;

    [Header("Mast Trigger Components")]
    public bool isInMast = false;
    public bool isTriggerMast = false;
    public ParticleSystem congrats;
    public Transform endportal;

    //MVC Components
    [HideInInspector]
    public Puzzle01Window P01W;
    [HideInInspector]
    public DatabasePuzzle01 DBP01;

    private GameObject player;
    private Vector3 startPosition;
    private bool isFirstTimeTriggerQuestion = true;
    private Vector3 previousPosition;
    public int timer = 0;

    public override void InitGameController(Puzzle01Window P01W)
    {
        Debug.Log("Init GameController for Puzzle01");
        base.InitGameController();

        Debug.Log("Connect Puzzle01 Window");
        this.P01W = P01W;

        Debug.Log("Connect Database of Puzzle01");
        DBP01 = GetComponent<DatabasePuzzle01>();

        Debug.Log("Call Database of Puzzle01 to connect");
        DBP01.InitDatabase();

        player = GameObject.FindGameObjectWithTag("Player");
        startPosition = player.transform.position;

        if (DialogueManager.showP01_00)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_00"));
            DialogueManager.showP01_00 = false;
        }

        if (GameRoot.instance.puzzleCompleted[0] == true && GameRoot.instance.puzzleCompleted[1] == true)
        {
            SetActive(sunLight, false);
            SetActive(moonLight, true);
            ocean.sun = moonLight.GetChild(0).GetComponent<Light>();
            RenderSettings.skybox = night;
        }

        //Init Components
        SetActive(endportal, false);
     }

    private void Update()
    {

        if (player.transform.position.x - previousPosition.x < 0.01 && player.transform.position.y - previousPosition.y < 0.01 && player.transform.position.y - previousPosition.y < 0.01)
        {
            timer += (int)Time.deltaTime + 1;
            if (timer == 1200)
            {
                GameRoot.ShowTips("Press 'R' to reset position.", true, false);
            }
        }
        else
        {
            timer = 0;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            player.transform.position = startPosition;
            GameRoot.ShowTips("", true, false);
        }

        //Update the instruction text based on the player status
        if (isInQues == true)
        {
            if (isTriggerQuestion)
            {
                SetText("- Press 'E': Exit the question.\n" +
                             "- Press 'Z': Top-down camera.");
            }
            else
            {
                SetText("- Press 'E': Trigger the question.");
            }
        }
        else
        {
            SetText("Please stand on the question platform.");
        }
        //************************************************************

        if (!DialogueManager.isInDialogue)
        {
            // Z key to switch camera
            if (Input.GetKeyDown(KeyCode.Z))
            {
                SwitchCamera();
            }

            if (isInQues && questionNum != 0)
            {
                // E key for the question tigger and exit
                if (!isTriggerQuestion && Input.GetKeyDown(KeyCode.E))
                {
                    //Show the dialogue when player trigger the question
                    if (questionNum == 1 && DialogueManager.showP01_01)
                    {
                        FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_01"));
                        DialogueManager.showP01_01 = false;
                    }
                    else if (questionNum == 2 && DialogueManager.showP01_03)
                    {
                        FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_03"));
                        DialogueManager.showP01_03 = false;
                    }
                    else if (questionNum == 3 && DialogueManager.showP01_05)
                    {
                        FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_05"));
                        DialogueManager.showP01_05 = false;
                    }
                    else if (questionNum == 4 && DialogueManager.showP01_07)
                    {
                        FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_07"));
                        DialogueManager.showP01_07 = false;
                    }
                    //********************************************************

                    isTriggerQuestion = true;

                    P01W.ShowInputPanel(true);
                    P01W.ShowFeedbackPanel(true);

                    GameRoot.instance.IsLock(true);

                    //Resume cannot unlock the lock
                    GameRoot.isPuzzleLock = true;
 
                    //Dialogue manager cannot unlock the lock;
                    DialogueManager.isPuzzleLock = true;

                    //Set current question tips in feedback panel
                    if (isFirstTimeTriggerQuestion)
                    {
                        P01W.SetFeedbackQuestionTips("Find the displacement\n" + DBP01.GetCurrentVector(questionNum) + " ->" + DBP01.GetCurrentVector(questionNum + 1));
                        isFirstTimeTriggerQuestion = false;
                    }
                }
                else if (isTriggerQuestion && Input.GetKeyDown(KeyCode.E))
                {
                    isTriggerQuestion = false;

                    P01W.ShowInputPanel(false);
                    P01W.ShowFeedbackPanel(false);

                    GameRoot.instance.IsLock(false);

                    //Resume can unlock the lock
                    GameRoot.isPuzzleLock = false;

                    //Dialogue manager can unlock the lock;
                    DialogueManager.isPuzzleLock = false;

                    // lock mouse after leaving question and not finishing it
                    try
                    {
                        GameObject.Find("MainCamera").GetComponent<CameraController>().isLock = true;
                        Debug.Log("mouse locked");
                    }
                    catch { }
                }
            }

            //Trigger Mast prop to end the puzzle01
            if(isInMast && !isTriggerMast)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    isTriggerMast = true;

                    congrats.Play();
                    endportal.gameObject.SetActive(true);

                    //Congratulation FX
                    audioService.PlayFXAudio(Constants.audioP01Congratulation);

                    if (DialogueManager.showP01_09)
                    {
                        FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_09"));
                        DialogueManager.showP01_09 = false;
                    }
                }
            }
        }

        previousPosition = player.transform.position;
    }

    private void SwitchCamera()
    {
        switch (Camera.main.depth)
        {
            case 1:
                Camera.main.depth = -1;
                topCamPlayer.GetComponent<TopCameraPlayer>().isTopCamera = true;
                GameRoot.instance.IsLock(true);
                break;

            case -1:
                Camera.main.depth = 1;
                topCamPlayer.GetComponent<TopCameraPlayer>().isTopCamera = false;
                if (!isTriggerQuestion)
                {
                    GameRoot.instance.IsLock(false);
                }
                break;
        }
    }

    public void SendConstrains(float defaultScalar, float defaultX, float defaultY, float defaultZ, int questionNum)
    {
        P01W.ClearInputField();

        P01W.defaultScalar = defaultScalar;
        P01W.defaultX = defaultX;
        P01W.defaultY = defaultY;
        P01W.defaultZ = defaultZ;
        P01W.questionNum = questionNum;

        this.questionNum = questionNum;
    }

    public bool CheckAnswer(float scalar, float x, float y, float z)
    {
        bool check = DBP01.Calculation(questionNum, scalar, x, y, z);

        string formula = DBP01.GetCurrentVector(questionNum) + " + " + scalar + " * (" + x + ", " + y + ", " + z + ") = " + DBP01.GetResultVector();
        if(check)
        {
            isAnswerCorrect = true;

            P01W.SetFeedback(formula, "Correct", Color.black);

            //Prepare next question to show the question tips in feedback panel
            isFirstTimeTriggerQuestion = true;   

            //DBP01.ClearGreenLineTips();

            //Correct answer audio FX;
            audioService.PlayFXAudio(Constants.audioP01CorrectAnswer);
        }
        else
        {
            isAnswerCorrect = false;

            P01W.SetFeedback(formula, "Not quite, please try again...", Color.red);

            //Wrong answer audio FX
            audioService.PlayFXAudio(Constants.audioP01WrongAnswer);
        }

        //Show the dialogue after player input the correct answer
        if (questionNum == 1 && check && DialogueManager.showP01_02)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_02"));
            DialogueManager.showP01_02 = false;
        }
        else if (questionNum == 2 && check && DialogueManager.showP01_04)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_04"));
            DialogueManager.showP01_04 = false;
        }
        else if (questionNum == 3 && check && DialogueManager.showP01_06)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_06"));
            DialogueManager.showP01_06 = false;
        }
        else if (questionNum == 4 && check && DialogueManager.showP01_08)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_08"));
            DialogueManager.showP01_08 = false;
        }
        //********************************************************

        if (check)
        {
            questionNum = 0;
        }

        //Resume can unlock the lock
        GameRoot.isPuzzleLock = false;

        //Dialogue manager can unlock the lock;
        DialogueManager.isPuzzleLock = false;

        return check;
    }

    public void SetText(string content)
    {
        SetText(P01W.txtInstruction, content);
    }

    //methods for dynamic input field calls
    public void updateLine(int index, float value)
    {
        if (index == 0) { DBP01.updateLineScalar(value, questionNum); }
        else if (index == 1) { DBP01.updateLineX(value, questionNum); }
        else if (index == 2) { DBP01.updateLineY(value, questionNum); }
        else { DBP01.updateLineZ(value, questionNum); }
    }

}
