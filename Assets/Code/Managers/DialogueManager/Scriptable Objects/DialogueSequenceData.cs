using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dialogueSequenceData", menuName = "Dialogue/Dialogue Sequence Data", order = 3)]
public class DialogueSequenceData : ScriptableObject
{
    public List<DialogueData> dialogueSequence;
}
