using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour {

    [SerializeField]
    private int baseHitPoints;
    private int currentHitPoints;
    [SerializeField]
    private int basePhysicalDodge;
    [SerializeField]
    private int baseMagicDodge;

    [System.Serializable]
    public class TypeResistance {
        public BattleUtils.DamageTypes damageType;
        public int resistanceAmount;

        public TypeResistance(BattleUtils.DamageTypes damageType, int resistanceAmount) {
            this.damageType = damageType;
            this.resistanceAmount = resistanceAmount;
        }
    }
    public TypeResistance[] typeResistances;

    public void TakeDamage(int damageAmount, BattleUtils.AttackTypes attackType, BattleUtils.DamageTypes damageType) {
        int attackRoll = Random.Range(0, 100);
        if (attackType == BattleUtils.AttackTypes.physical) {
            if (GetPhysicalDodge() > attackRoll) {
                // FIXME: Indicate the attack was dodged
                Debug.Log("Physical attack was dodged");
            }
        } else if (attackType == BattleUtils.AttackTypes.magical) {
            if (GetMagicalDodge() > attackRoll) {
                // FIXME: Indicate attack was dodged
                Debug.Log("Magical attack was dodged");
            }
        }
        int actualDamage = CalculateResistedDamage(damageAmount, damageType);
        currentHitPoints -= actualDamage;
    }

    private int CalculateResistedDamage(int rawDamageAmount, BattleUtils.DamageTypes damageType) {
        TypeResistance resistType = null;
        foreach (TypeResistance resType in typeResistances) {
            if (resType.damageType == damageType) {
                resistType = resType;
            }
        }

        if (resistType == null) {
            Debug.Log("NO RESISTANCE SET FOR TYPE: " + damageType.ToString());
            resistType = new TypeResistance(damageType, 0);
        }
        float resistancePercent = resistType.resistanceAmount / 100.0f;
        float damageResisted = rawDamageAmount * resistancePercent;
        float actualDamage = rawDamageAmount - damageResisted;
        return (int)actualDamage;
    }

    private int GetPhysicalDodge() {
        return basePhysicalDodge;
    }

    private int GetMagicalDodge() {
        return baseMagicDodge;
    }

    public int GetCurrentHitPoints() {
        return currentHitPoints;
    }

    public int GetBaseHitPoints() {
        return baseHitPoints;
    }

}
