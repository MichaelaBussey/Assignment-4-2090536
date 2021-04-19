using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text HP;


    public void SetHUD (Stats unit)
    {
        HP.text = unit.currentHP.ToString();        

    }
    /*public void SetHP(int hp)
    {
        hp = currentHP;
    }*/
}
