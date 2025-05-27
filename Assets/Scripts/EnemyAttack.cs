using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Attack
{
    public override void AttackHit()
    {
        Debug.Log("BANG BANG!");
        base.AttackHit();
    }
}
