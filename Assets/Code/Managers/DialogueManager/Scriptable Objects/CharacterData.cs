using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="characterData", menuName="Dialogue/Character Data", order = 1 )]
public class CharacterData : ScriptableObject
{
    public Sprite characterImage;
    public string characterName;
    public Color nameColor;
    public Color dialogueColor;
}
