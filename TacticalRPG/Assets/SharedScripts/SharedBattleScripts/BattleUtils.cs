using UnityEngine;
using System.Collections;

public class BattleUtils : MonoBehaviour {

    public enum DamageTypes { };
    public enum AttackTypes { physical, magical };

    private static BattleUtils instance;

    private BattleUtils() { }

    public static BattleUtils Instance {
        get {
            if (instance == null) {
                instance = new BattleUtils();
            }
            return instance;
        }
    }


}
