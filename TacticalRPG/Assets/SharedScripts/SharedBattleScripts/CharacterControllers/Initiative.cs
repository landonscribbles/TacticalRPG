using UnityEngine;
using System.Collections;

public class Initiative : MonoBehaviour {

    [SerializeField]
    private int baseTurnSpeed;
    private int turnInitiative;

    void Awake() {
        turnInitiative = 0;
    }

    public void UpdateTurnInitiative() {
        turnInitiative += GetTurnSpeed();
    }

    private int GetTurnSpeed() {
        return baseTurnSpeed;
    }
}
