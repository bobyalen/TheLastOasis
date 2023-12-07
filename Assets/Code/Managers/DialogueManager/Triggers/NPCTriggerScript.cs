using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTriggerScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    DialogueSequenceData sequenceData;
    bool hasTriggered = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !hasTriggered)
        {
            EventManager.TriggerEvent(Event.DialogueStart, new StartDialoguePacket()
            {
                dialogueSequence = sequenceData
            });
            hasTriggered = true;
        }
    }
}
