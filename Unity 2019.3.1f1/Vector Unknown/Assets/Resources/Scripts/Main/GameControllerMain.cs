using UnityEngine;

public class GameControllerMain : GameControllerRoot
{
    [Header("Player & Camera")]

    //*** Player Transforms to spawn into area right outside the puzzle commented LaQuez
    public Transform PCFromPuzzle01; 
    public Transform PCFromPuzzle02;
    public Transform PCFromPuzzle03;
    //*****
    public Transform sunLight;
    public Transform moonLight;
    public LPWAsset.LowPolyWaterScript ocean;
    public Material night;
    public Transform portalPuzzle03;
    public Sprite[] spriteChecks;

    [HideInInspector]
    public MainWindow MW;

    private Transform player;
    private Vector3 startPosition;
    private Vector3 previousPosition;
    private int timer = 0;

    public override void InitGameController(MainWindow MW)
    {
        Debug.Log("Init GameController Main");
        base.InitGameController();

        Debug.Log("Connect Main Window");
        this.MW = MW;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = player.transform.position;


        //*** Switch case to control where the player spawns after completeing the puzzles
        switch (GameRoot.instance.exitPuzzle)
        { 
            case 1:
                SetActive(PCFromPuzzle01, true);
                SetActive(PCFromPuzzle02, false);
                SetActive(PCFromPuzzle03, false);
                break;

            case 2:
                SetActive(PCFromPuzzle01, false);
                SetActive(PCFromPuzzle02, true);
                SetActive(PCFromPuzzle03, false);
                break;

            case 3:
                SetActive(PCFromPuzzle01, false);
                SetActive(PCFromPuzzle02, false);
                SetActive(PCFromPuzzle03, true);
                break;
        }

        //If player only finish puzzle 01
        if(GameRoot.instance.puzzleCompleted[0] == true && GameRoot.instance.puzzleCompleted[1] != true)
        {
            if(DialogueManager.showM_00)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Main_00"));
                DialogueManager.showM_00 = false;
            }

            //Set the instructional text and check image
           // MW.SetInstructionText("Find & enter the cave to beat the pirates."); // original
            MW.SetInstructionText("Explore the land and find what else might be out there"); //added by LaQuez
            MW.SetCheckImage(0, spriteChecks[0]);
        }
        //If player finish puzzle 01 & puzzle 02 turn sky into night
        else if(GameRoot.instance.puzzleCompleted[0] == true && GameRoot.instance.puzzleCompleted[1] == true)
        {
            SetActive(sunLight, false);
            SetActive(moonLight, true);
            ocean.sun = moonLight.GetChild(0).GetComponent<Light>();

            RenderSettings.skybox = night;
            SetActive(portalPuzzle03, true);

            if(DialogueManager.showM_01)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Main_01"));
                DialogueManager.showM_01 = false;
            }

            MW.SetInstructionText("Find & enter the pirates' hideout to save villagers.");          
            MW.SetCheckImage(0, spriteChecks[0]);
            MW.SetCheckImage(1, spriteChecks[1]);
        }
     }

    void Update()
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

        previousPosition = player.transform.position;
    }
}
