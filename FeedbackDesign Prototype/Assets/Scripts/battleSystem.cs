using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState
{
    START, PLAYERTURN, ENEMYTURN, WON, LOST
}

public class battleSystem : MonoBehaviour
{
    public BattleState State;
    public Text CombatLog, atk1Dmg, atk2Dmg, healAmtText;
    public GameObject playerPrefab, enemyPrefab, critHit, winBox, loseBox, wonEnd, loseEnd;
    public GameObject increaseStats;
    public bool HPUp, Atk1Up, Atk2Up, HealUp = false;
    public Stats PlayerUnit, EnemyUnit;
    public Transform playerLocation, enemyLocation;
    public BattleHUD playerHUD, enemyHUD;
    public AudioClip Crit, Heal, Hit, Miss;
    int atk1, atk2, healAmt;

    int atk1Min = 3;
    int atk1Max = 6;
    int atk2Min = 5;
    int atk2Max = 10;
    int healMin = 4;
    int healMax = 8;



    void Start()
    {
        State = BattleState.START;
        playerPrefab.SetActive(true);
        enemyPrefab.SetActive(true);
        playerHUD.HP.text = PlayerUnit.maxHP.ToString();
        StartCoroutine(SetupBattle());
    }
    IEnumerator SetupBattle()
    {
        PlayerUnit = playerPrefab.GetComponent<Stats>();
        EnemyUnit = enemyPrefab.GetComponent<Stats>();
        playerHUD.SetHUD(PlayerUnit);
        enemyHUD.SetHUD(EnemyUnit);
        yield return new WaitForSeconds(0);
        State = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAtk1()
    {
        if (State == BattleState.PLAYERTURN)
        {
            atk1 = Random.Range(atk1Min, atk1Max);
            bool isDead = EnemyUnit.TakeDamage(atk1);
            //ACHIEVED MULTIPLE SOUNDS ON ONE SCRIPT!!
            GetComponent<AudioSource>().clip = Hit;
            GetComponent<AudioSource>().Play();
            enemyHUD.HP.text = EnemyUnit.currentHP.ToString();
            State = BattleState.ENEMYTURN;
            CombatLog.text += atk1.ToString() + "dmg dealt to enemy." + "\n";
            CombatLog.color = Color.blue;
            yield return new WaitForSeconds(2);

            if (isDead)
            {
                State = BattleState.WON;
                EndBattle();
            }

            else
            {
                StartCoroutine(EnemyTurn());
            }
        }

    }

    IEnumerator PlayerAtk2()
    {
        if (State == BattleState.PLAYERTURN)
        {
            int hitChance = 70;
            bool isDead = false;

            if (Random.Range(0, 100) <= hitChance)
            {
                atk2 = Random.Range(atk2Min, atk2Max);
                isDead = EnemyUnit.TakeDamage(atk2);
                GetComponent<AudioSource>().clip = Hit;
                GetComponent<AudioSource>().Play();
                enemyHUD.HP.text = EnemyUnit.currentHP.ToString();
                CombatLog.text += atk2.ToString() + "dmg dealt to enemy." + "\n";
                CombatLog.color = Color.blue;
                if (atk2 >= 7)
                {
                    GetComponent<AudioSource>().clip = Crit;
                    GetComponent<AudioSource>().Play();
                    Instantiate(critHit, enemyLocation);
                    CombatLog.text += "It was a critical hit!" + "\n";
                }
            }
            else
            {
                GetComponent<AudioSource>().clip = Miss;
                GetComponent<AudioSource>().Play();
                CombatLog.text += ("Attack missed!" + "\n");
                CombatLog.color = Color.red;
            }



            State = BattleState.ENEMYTURN;
            yield return new WaitForSeconds(2);
            if (isDead)
            {
                State = BattleState.WON;
                EndBattle();
            }

            else
            {
                StartCoroutine(EnemyTurn());
            }
        }

    }

    IEnumerator PlayerHeal()
    {
        if (State == BattleState.PLAYERTURN)
        {
            healAmt = Random.Range(healMin, healMax);
            PlayerUnit.Heal(healAmt);
            GetComponent<AudioSource>().clip = Heal;
            GetComponent<AudioSource>().Play();
            CombatLog.text += "+" + healAmt.ToString() + " HP healed!" + "\n";
            CombatLog.color = Color.green;
            playerHUD.HP.text = PlayerUnit.currentHP.ToString();
            State = BattleState.ENEMYTURN;
            yield return new WaitForSeconds(2);
            StartCoroutine(EnemyTurn());
        }

    }


    void EndBattle()
    {
        if (State == BattleState.WON)
        {
            winBox.SetActive(true);
            //WON.SetActive(true);
            wonEnd.SetActive(true);
        }

        else if (State == BattleState.LOST)
        {
            loseBox.SetActive(true);
            //LOST.SetActive(true);
            loseEnd.SetActive(true);
        }

    }

    IEnumerator EnemyTurn()
    {
        //int hitChance = 85;
        //isDead = false;
        yield return new WaitForSeconds(1);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Enemy1Atk1();
        }

        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            float Move = Random.Range(0f, 1f);
            print(Move);

            if (Move < 0.70)
            {
                Enemy2Atk1();
                print("atk1");
            }
            else
            {
                Enemy2Atk2();
                print("atk2");
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            float Move = Random.Range(0f, 1f);
            if (Move < 0.65)
            {
                Enemy3Atk1();
            }
            else
            {
                Enemy3Atk2();
            }
        }
    }



    void PlayerTurn()
    {
        //Choose an action
    }

    public void OnAttack1()
    {
        if (State != BattleState.PLAYERTURN)
        {
            return;
        }
        else
        {
            StartCoroutine(PlayerAtk1());
        }
    }

    public void OnAttack2()
    {
        if (State != BattleState.PLAYERTURN)
        {
            return;
        }
        else
        {
            StartCoroutine(PlayerAtk2());
        }
    }

    public void OnHeal()
    {
        if (State != BattleState.PLAYERTURN)
        {
            return;
        }
        else
        {
            StartCoroutine(PlayerHeal());
        }


    }

    public void Enemy1Atk1()
    {
        int hitChance = 85;
        bool isDead = false;

        if (Random.Range(0, 100) <= hitChance)
        {
            int EnemyAtk = Random.Range(4, 7);
            isDead = PlayerUnit.TakeDamage(EnemyAtk);
            GetComponent<AudioSource>().clip = Hit;
            GetComponent<AudioSource>().Play();
            CombatLog.text += EnemyAtk.ToString() + "dmg dealt to you." + "\n";
            CombatLog.color = Color.red;
            playerHUD.HP.text = PlayerUnit.currentHP.ToString();
        }
        else
        {
            GetComponent<AudioSource>().clip = Miss;
            GetComponent<AudioSource>().Play();
            CombatLog.text += ("Enemy attack missed!" + "\n");
            CombatLog.color = Color.red;
        }

        if (isDead)
        {
            State = BattleState.LOST;
            EndBattle();
        }
        else
        {
            State = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }
    public void Enemy2Atk1()
    {
        int hitChance = 85;
        bool isDead = false;

        if (Random.Range(0, 100) <= hitChance)
        {
            int EnemyAtk = Random.Range(4, 8);
            isDead = PlayerUnit.TakeDamage(EnemyAtk);
            GetComponent<AudioSource>().clip = Hit;
            GetComponent<AudioSource>().Play();
            CombatLog.text += EnemyAtk.ToString() + "dmg dealt to you." + "\n";
            CombatLog.color = Color.red;
            playerHUD.HP.text = PlayerUnit.currentHP.ToString();
        }
        else
        {
            GetComponent<AudioSource>().clip = Miss;
            GetComponent<AudioSource>().Play();
            CombatLog.text += ("Enemy attack missed!" + "\n");
            CombatLog.color = Color.red;
        }

        if (isDead)
        {
            State = BattleState.LOST;
            EndBattle();
        }
        else
        {
            State = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }
    public void Enemy2Atk2()
    {
        int hitChance = 70;
        bool isDead = false;

        if (Random.Range(0, 100) <= hitChance)
        {
            int EnemyAtk = Random.Range(6, 9);
            isDead = PlayerUnit.TakeDamage(EnemyAtk);
            GetComponent<AudioSource>().clip = Hit;
            GetComponent<AudioSource>().Play();
            CombatLog.text += EnemyAtk.ToString() + "dmg dealt to you." + "\n" + "It was a Hard Attack!" + "\n";
            CombatLog.color = Color.red;
            playerHUD.HP.text = PlayerUnit.currentHP.ToString();
        }
        else
        {
            GetComponent<AudioSource>().clip = Miss;
            GetComponent<AudioSource>().Play();
            CombatLog.text += ("Enemy's Hard attack missed!" + "\n");
            CombatLog.color = Color.red;
        }

        if (isDead)
        {
            State = BattleState.LOST;
            EndBattle();
        }
        else
        {
            State = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }
    public void Enemy3Atk1()
    {
        int hitChance = 90;
        bool isDead = false;

        if (Random.Range(0, 100) <= hitChance)
        {
            int EnemyAtk = Random.Range(4, 9);
            isDead = PlayerUnit.TakeDamage(EnemyAtk);
            GetComponent<AudioSource>().clip = Hit;
            GetComponent<AudioSource>().Play();
            CombatLog.text += EnemyAtk.ToString() + "dmg dealt to you." + "\n";
            CombatLog.color = Color.red;
            playerHUD.HP.text = PlayerUnit.currentHP.ToString();
        }
        else
        {
            GetComponent<AudioSource>().clip = Miss;
            GetComponent<AudioSource>().Play();
            CombatLog.text += ("Enemy attack missed!" + "\n");
            CombatLog.color = Color.red;
        }

        if (isDead)
        {
            State = BattleState.LOST;
            EndBattle();
        }
        else
        {
            State = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }
    public void Enemy3Atk2()
    {
        int hitChance = 80;
        bool isDead = false;

        if (Random.Range(0, 100) <= hitChance)
        {
            int EnemyAtk = Random.Range(7, 11);
            isDead = PlayerUnit.TakeDamage(EnemyAtk);
            GetComponent<AudioSource>().clip = Hit;
            GetComponent<AudioSource>().Play();
            CombatLog.text += EnemyAtk.ToString() + "dmg dealt to you." + "\n" + "It was a Hard Attack!" + "\n";
            CombatLog.color = Color.red;
            playerHUD.HP.text = PlayerUnit.currentHP.ToString();
        }
        else
        {
            GetComponent<AudioSource>().clip = Miss;
            GetComponent<AudioSource>().Play();
            CombatLog.text += ("Enemy attack missed!" + "\n");
            CombatLog.color = Color.red;
        }

        if (isDead)
        {
            State = BattleState.LOST;
            EndBattle();
        }
        else
        {
            State = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    //Button fns
    public void HPUpBtn1()
    {
        HPUp = true;
        PlayerUnit.maxHP += 4;
        PlayerUnit.currentHP += 4;
        playerHUD.HP.text = PlayerUnit.maxHP.ToString();
        increaseStats.SetActive(false);
    }
    public void Atk1UpBtn1()
    {
        Atk1Up = true;
        atk1Min = 5;
        atk1Max = 8;
        atk1Dmg.text = atk1Min.ToString() + "-" + "7 dmg";
        increaseStats.SetActive(false);
    }
    public void Atk2UpBtn1()
    {
        Atk2Up = true;
        atk2Min = 6;
        atk2Max = 11;
        atk2Dmg.text = atk2Min.ToString() + "-" + "10 dmg";
        increaseStats.SetActive(false);
    }
    public void HealUpBtn1()
    {
        HealUp = true;
        healMin = 5;
        healMax = 9;
        healAmtText.text = healMin.ToString() + "-" + "8 HP";
        increaseStats.SetActive(false);
    }

    public void HPUpBtn2()
    {
        HPUp = true;
        PlayerUnit.maxHP += 4;
        PlayerUnit.currentHP += 4;
        playerHUD.HP.text = PlayerUnit.maxHP.ToString();
        increaseStats.SetActive(false);
    }
    public void Atk1UpBtn2()
    {
        Atk1Up = true;
        atk1Min = 6;
        atk1Max = 9;
        atk1Dmg.text = atk1Min.ToString() + "-" + "8 dmg";
        increaseStats.SetActive(false);
    }
    public void Atk2UpBtn2()
    {
        Atk2Up = true;
        atk2Min = 7;
        atk2Max = 12;
        atk2Dmg.text = atk2Min.ToString() + "-" + "11 dmg";
        increaseStats.SetActive(false);
    }
    public void HealUpBtn2()
    {
        HealUp = true;
        healMin = 6;
        healMax = 10;
        healAmtText.text = healMin.ToString() + "-" + "9 HP";
        increaseStats.SetActive(false);
    }
}
 
