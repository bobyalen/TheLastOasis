using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System.Linq;

public enum Stat
{
    Health,
    Speed,
    Defence,
    Dexterity,
    Damage,
    Coin_Gain,
    Enemy_Dmg,
    Enemy_HP,
    Healing,
    Death_Save,
    Blindness,
    UI_Blindness,
    Current_Health,
    Coin_Loss,
    Divine_Shield
}

public enum MaxStat


{
    MaxHealth,
    MaxSpeed,
    MaxDefence,
    MaxDexterity,
    MaxDamage
}

public enum StatModifierType
{
    SET,
    PERCENTAGE,
    NUMERICAL
}
[Serializable]
public class StatModifier
{
    public Stat stat;
    public StatModifierType statModifierType;
    public float modifierValue;
    public object modifier;
    public Type modifierType;

    public StatModifier(Stat stat, StatModifierType type, float value)
    {
        this.stat = stat;
        this.statModifierType = type;
        modifierValue = value;
    }
}

public class PlayerStats : MonoBehaviour
{
    //Values that the player is currently holding

    private bool hasBeenInit = false;
    public static PlayerStats Instance { get; private set; }

    [SerializeField]
    private StatValues baseValuesObject;

    public Dictionary<Stat, float> baseStatValues;
    public Dictionary<Stat, float> cachedCalculatedValues;
    public List<StatModifier> statModifiers;

    bool isDead = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;

        }
        statModifiers = new List<StatModifier>();
        if (PlayerPrefs.HasKey("isSet"))
            hasBeenInit = Convert.ToBoolean(PlayerPrefs.GetString("isSet"));
        else
        {
            hasBeenInit = false;
            PlayerPrefs.SetString("isSet", false.ToString());
        }
        if(!hasBeenInit)
        {
            hasBeenInit = true;
            baseStatValues = new Dictionary<Stat, float>();
            cachedCalculatedValues = new Dictionary<Stat, float>();
            foreach(StatValue sv in baseValuesObject.statValues)
            {
                baseStatValues.Add(sv.stat, sv.value);
                cachedCalculatedValues.Add(sv.stat, sv.value);
            }

        }
        else
        {
            LoadValues();
        }
        EventManager.StartListening(Event.EnemyHitPlayer, OnEnemyHit);
        EventManager.StartListening(Event.PlayerDeath, OnPlayerDeath);
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {

    }

    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetStatValues();
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            cachedCalculatedValues[Stat.Current_Health] = 0;
        }
        SetPlayerHealthToMaxHealth();
        if (IsPlayerDead())
        {
            if (cachedCalculatedValues[Stat.Death_Save] > 0)
            {
                EventManager.TriggerEvent(Event.DeathSave, null);
                cachedCalculatedValues[Stat.Death_Save] = 0;
                cachedCalculatedValues[Stat.Current_Health] = (int)(30.0f * cachedCalculatedValues[Stat.Health] / 100.0f);
                return;

            }
            cachedCalculatedValues[Stat.Current_Health] = 0;

            if(!isDead)
            {
                isDead = true;
                EventManager.TriggerEvent(Event.PlayerDeath, null);
            }
        }
        if(Input.GetKeyDown(KeyCode.L)) 
        {
            hasBeenInit = false;
        }
        if(Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.H))
        {
            SetPlayerHealthToMaxHealth(true);
        }
    }

    bool IsPlayerDead()
    {
        return cachedCalculatedValues[Stat.Current_Health] <= 0;
    }

    void SetPlayerHealthToMaxHealth(bool debug = false)
    {
        if (cachedCalculatedValues[Stat.Current_Health] > cachedCalculatedValues[Stat.Health] || debug)
        {
            cachedCalculatedValues[Stat.Current_Health] = cachedCalculatedValues[Stat.Health];
            baseStatValues[Stat.Current_Health] = cachedCalculatedValues[Stat.Health];
        }
    }


    private void OnEnemyHit(IEventPacket packet)
    {
        EnemyHitPacket ehp = packet as EnemyHitPacket;
        if (!ehp.playerInvulnerability)
        {
            if (!ehp.playerDivineShield)
            {
                float enemyDamage = ehp.healthDeplete;
                enemyDamage = (cachedCalculatedValues[Stat.Enemy_Dmg]) / 100.0f * enemyDamage;
                WoundPlayer((int)enemyDamage, false);
                EventManager.TriggerEvent(Event.DamageDealt, new DamageDealtPacket()
                {
                    textColor = Color.red,
                    position = PlayerController.Instance.transform.position,
                    damage = (int)enemyDamage
                });
            }
            else
            {
                EventManager.TriggerEvent(Event.DivineShieldHit, null);
                EventManager.TriggerEvent(Event.DamageDealt, new DamageDealtPacket()
                {
                    textColor = Color.blue,
                    position = PlayerController.Instance.transform.position,
                    damage = 0
                });

            }

            PlayerController.Instance.invulnerability = true;
        }
    }
    private void OnDestroy()
    {
        EventManager.StopListening(Event.EnemyHitPlayer, OnEnemyHit);
        EventManager.StopListening(Event.PlayerDeath, OnPlayerDeath);
        SaveValues();
    }

    public void SaveValues()
    {
        foreach(var kvp in baseStatValues)
        {
            PlayerPrefs.SetFloat(kvp.Key.ToString(), kvp.Value);
        }
        PlayerPrefs.SetString("isSet", hasBeenInit.ToString());
    }

    public void LoadValues()
    {
        baseStatValues = new Dictionary<Stat, float>();
        cachedCalculatedValues = new Dictionary<Stat, float>();
        var values = Enum.GetValues(typeof(Stat));
        foreach(var stat in values)
        {
            baseStatValues.Add((Stat)stat, PlayerPrefs.GetFloat(stat.ToString()));
            cachedCalculatedValues.Add((Stat)stat, PlayerPrefs.GetFloat(stat.ToString()));

        }
        hasBeenInit = Convert.ToBoolean(PlayerPrefs.GetString("isSet"));
    }

    public void ResetStatValues()
    {
        baseStatValues.Clear();
        cachedCalculatedValues.Clear();
        foreach(var sv in baseValuesObject.statValues)
        {
            baseStatValues.Add(sv.stat, sv.value);
            cachedCalculatedValues.Add(sv.stat, sv.value);
        }
    }

    private void OnPlayerDeath(IEventPacket packet)
    {
        //currentHealth = maxHealth;
        if(PlayerPrefs.HasKey(PlayerPrefsKeys.DEATH_NUMBER.ToString()))
        {
            int value = PlayerPrefs.GetInt(PlayerPrefsKeys.DEATH_NUMBER.ToString());
            PlayerPrefs.SetInt(PlayerPrefsKeys.DEATH_NUMBER.ToString(), value + 1);
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.DEATH_NUMBER.ToString(), 1);
        }

    }

    public void WoundPlayer(int amount, bool isDefence)
    {
        if(isDefence)
        {

        }
        else
        {
            cachedCalculatedValues[Stat.Current_Health] -= amount;
        }
    }

    public void HealPlayer(float amount)
    {
        amount = (int)((cachedCalculatedValues[Stat.Healing] / 100.0f) * amount);
        cachedCalculatedValues[Stat.Current_Health] += amount;
    }

    public void AddModifier(StatModifier modifier)
    {
        statModifiers.Add(modifier);
        CalculateStat(modifier.stat);
    }

    public void RemoveModifier(StatModifier modifier)
    {
        statModifiers.Remove(modifier);
        CalculateStat(modifier.stat);
    }

    public void CalculateStat(Stat stat)
    {
        List<StatModifier> modifiers = statModifiers.Where(x => x.stat == stat).ToList();
        List<StatModifier> setModifiers = modifiers.Where(x => x.statModifierType == StatModifierType.SET).ToList();
        List<StatModifier> percentageModifiers = modifiers.Where(x => x.statModifierType == StatModifierType.PERCENTAGE).ToList();
        List<StatModifier> numericalModifiers = modifiers.Where(x => x.statModifierType == StatModifierType.NUMERICAL).ToList();
        if(setModifiers.Count != 0)
        {
            cachedCalculatedValues[stat] = setModifiers[0].modifierValue;
            EventManager.TriggerEvent(Event.StatChanged, new StatChangedPacket()
            {
                stat = stat
            });
            return;
        }
        else
        {
            float preliminaryStat = baseStatValues[stat];
            foreach(var num in numericalModifiers)
            {
                preliminaryStat += num.modifierValue;
            }
            foreach(var percentage in percentageModifiers)
            {
                preliminaryStat += (percentage.modifierValue * preliminaryStat) / 100.0f;
            }
            if (IsStatInteger(stat))
            {
                if (stat == Stat.Current_Health && Mathf.Round(preliminaryStat) < 1)
                    preliminaryStat = 1;
                else
                    preliminaryStat = Mathf.Round(preliminaryStat);
            }
            cachedCalculatedValues[stat] = preliminaryStat;
            EventManager.TriggerEvent(Event.StatChanged, new StatChangedPacket()
            {
                stat = stat
            });
        }
    }

    private bool IsStatInteger(Stat stat)
    {
        switch(stat)
        {
            case Stat.Dexterity:
            case Stat.Speed:
                return false;
            default:
                return true;
        }
    }

    public float GetCurrentHealthPercentage()
    {
        return cachedCalculatedValues[Stat.Current_Health] / cachedCalculatedValues[Stat.Health];
    }

    public void ResetDeath()
    {
        //currentHealth = maxHealth;
        cachedCalculatedValues[Stat.Current_Health] = baseStatValues[Stat.Health];
        isDead = false;
    }
}