using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CripplePlayerStack", menuName = "Stacks/Cripple Player Stack")]
public class CripplePlayerStack : TimeDepletableStack
{
    public override void OnAdd()
    {
        Debug.Log("Cripple added!");
        PlayerAttack.Instance.canAttack = false;
    }

    public override void OnDeplete()
    {
    }

    public override void OnFinalDeplete()
    {
        Debug.Log("Cripple removed!");
        PlayerAttack.Instance.canAttack = true;
    }
}
