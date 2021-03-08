using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance = null;
    public static bool isInDialogue = false;
    public static bool isPuzzleLock = false;

    public Transform dialogueWindow;
    public Text nameText;
    public Text dialogueText;
    public Image characterImage;
    public Animator animator;
    public Transform uiWindows;

    private GameControllerPuzzle01 GCP01;
    private Conversation conversation;
    private int dialoguesSize;
    private List<int> sentencesSize = new List<int>();
    private int dialogueIndex;
    private int sentenceIndex;
    private AudioService audioService;
    private bool isFirstSentence;

    private void Start()
    {
        instance = this;
        audioService = GameRoot.instance.audioService;
    }

    public void StartDialogue(Conversation conversation)
    {
        // setup dialogue system
        dialogueWindow.gameObject.SetActive(true);

        isInDialogue = true;
        GameRoot.instance.IsLock(true);
        uiWindows.gameObject.SetActive(false);

        //************* Reset the Conversation ****************
        this.conversation = conversation;
        dialoguesSize = conversation.dialogues.Length;
        sentencesSize.Clear();
        foreach (Dialogue dialogue in conversation.dialogues)
        {
            sentencesSize.Add(dialogue.sentences.Length);
        }
        dialogueIndex = 0;
        sentenceIndex = 0;
        isFirstSentence = true;
        //*******************************************************
        
        nameText.text = conversation.dialogues[0].character.name;
        characterImage.sprite = conversation.dialogues[0].character.protrait;

        animator.SetBool("IsOpen", true);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        //******************** Display Sound FX *******************
        if(isFirstSentence)
        {
            isFirstSentence = false;
        }
        else
        {
            audioService.PlayUIAudio(Constants.audioUINextBtn);
        }
        //***********************************************************

        // exit dialogue once all the characters in string are read
        if (dialogueIndex >= dialoguesSize)
        {
            EndDialogue();
            return;
        }

        //********************** Set Current Sentence Infomation ****************************
        nameText.text = conversation.dialogues[dialogueIndex].character.name;
        characterImage.sprite = conversation.dialogues[dialogueIndex].character.protrait;
        string sentence = conversation.dialogues[dialogueIndex].sentences[sentenceIndex];
        //**************************************************************************************
        
        if(sentenceIndex + 1 >= sentencesSize[dialogueIndex])
        {
            //Move into Next Dialogue
            dialogueIndex++;
            sentenceIndex = 0;
        }
        else
        {
            sentenceIndex++;
        }

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);

        uiWindows.gameObject.SetActive(true);

        if(!isPuzzleLock)
        {
            GameRoot.instance.IsLock(false);
            Debug.Log("Unlock From Dialogue");

            Debug.Log("Start Try");
            // lock mouse
            try
            {
                GameObject.Find("Main Camera").GetComponent<CameraController>().postPuzzleLock = true;
                GameObject.Find("Main Camera").GetComponent<CameraController>().isLock = true;
                Debug.Log("In Try");
            }
            catch { Debug.Log("In Catch"); }
            Debug.Log("After Try Catch");
        }

        isInDialogue = false;

        try
        {
            GameObject.Find("MainCamera").GetComponent<CameraController>().isLock = true;
        }
        catch { }
    }

    //Flags for puzzle01 dialogue
    public static bool showP01_00 = true;
    public static bool showP01_01 = true;
    public static bool showP01_02 = true;
    public static bool showP01_03 = true;
    public static bool showP01_04 = true;
    public static bool showP01_05 = true;
    public static bool showP01_06 = true;
    public static bool showP01_07 = true;
    public static bool showP01_08 = true;
    public static bool showP01_09 = true;

    public static bool showP03_00 = true;

    public static bool showM_00 = true;
    public static bool showM_01 = true;

    public void ResetAll()
    {
        showP01_00 = true;
        showP01_01 = true;
        showP01_02 = true;
        showP01_03 = true;
        showP01_04 = true;
        showP01_05 = true;
        showP01_06 = true;
        showP01_07 = true;
        showP01_08 = true;
        showP01_09 = true;

        showP03_00 = true;

        showM_00 = true;
        showM_01 = true;
        //TODO

        uiWindows.gameObject.SetActive(true);
        dialogueWindow.gameObject.SetActive(false);
    }
}
