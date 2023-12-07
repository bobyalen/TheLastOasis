using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Contracts", menuName = "Contracts - New")]
public class Contract : ScriptableObject
{
    public string contractName;
    public string contractGains;
    public string contractLosses;
    public List<ContractStack> stacksToAdd;
}

[Serializable]
public class ContractStack
{
    public Stack stack;
    public int stacksToAdd;
}
