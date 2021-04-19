using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public string Name;
    public int dmg, maxHP, currentHP;

    public bool TakeDamage (int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            return true;
        }

        else
            return false; 

    }

    public void Heal(int Amount)
    {
        currentHP += Amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }
}
