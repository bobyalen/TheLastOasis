using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "dialogueData", menuName = "Dialogue/Dialogue Data", order = 2)]
public class DialogueData : ScriptableObject
{
    private GUID guid = default(GUID);
    [Tooltip("Accepts HTML tags for formatting")]
    [TextAreaAttribute(15,20)]
    public string dialogueText;
    [Tooltip("The character assigned to the dialogue piece")]
    public CharacterData characterData;


    private void OnValidate()
    {
        if (guid == default(GUID))
            guid = GUID.Generate();
    }

    public GUID GetGUID()
    {
        return guid;
    }
}
