using UnityEngine;
using System.Collections;

public class Initiative : MonoBehaviour {

    [SerializeField]
    private int baseTurnSpeed;
    private int turnInitiative;

    void Awake() {
        turnInitiative = 0;
    }

    void Start() {
        BattleUtils.Instance.RegisterInitiative(this);
    }

    public int GetTurnSpeed() {
        return baseTurnSpeed;
    }

    public void UpdateTurnInitiative() {
        turnInitiative += GetTurnSpeed();
    }

    public int GetTurnInitiative() {
        return turnInitiative;
    }

    public void ResetTurnInitiative() {
        turnInitiative = 0;
    }

}
