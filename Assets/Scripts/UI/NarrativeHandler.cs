using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.EventSystems; 

public class NarrativeHandler : MonoBehaviour
{
    [SerializeField] private GameObject characters; // All Characters in the Dialogue
    [SerializeField] private DialogueRunner dialogueRunner; // Yarn Spinner Dialogue Runner

    private void Awake()
    {
        AddYarnCommands();
    }

    private void Start()
    {
        StartDialogue("Tester1");
    }

    public void StartDialogue(string node)
    {
        dialogueRunner.StartDialogue(node);
    }

    // Converts Functions to Yarn Commands
    private void AddYarnCommands()
    {
        if(dialogueRunner != null)
        {
            dialogueRunner.AddCommandHandler("YarnTest", () => YarnTest());
            dialogueRunner.AddCommandHandler("ActivateCharacterChildByIndex", (string index, string activate) => 
                            ActivateCharacterChildByIndex(int.Parse(index), bool.Parse(activate)));
        }
    }
    

    private void YarnTest()
    {
        Debug.Log("Test");
    }

    public void ActivateCharacterChildByIndex(int index, bool activate)
    {
        int childCount = characters.transform.childCount;

        if (index >= 0 && index < childCount)
        {
            Transform child = characters.transform.GetChild(index);
            GameObject character = child.gameObject;
            
            if (activate)
            {
                character.SetActive(true);
            }
            else
            {
                character.SetActive(false);
            }
        }
    }
}
