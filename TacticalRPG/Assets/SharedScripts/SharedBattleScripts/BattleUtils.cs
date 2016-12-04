using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BattleUtils : UtilsUpdateInterface {

    public enum DamageTypes { };
    public enum AttackTypes { physical, magical };

    private List<Initiative> initiatives;
    private BattleActorInterface activeActor;
    private int initiativeThreshold;

    private static BattleUtils instance;

    private BattleUtils() {
    }

    public static BattleUtils Instance {
        get {
            if (instance == null) {
                instance = new BattleUtils();
                instance.Initialize();
            }
            return instance;
        }
    }

    private void Initialize() {
        // This was blowing up in the constructor
        UtilsUpdateWrapper updateWrapper = GameObject.Find("UtilsUpdateWrapper").GetComponent<UtilsUpdateWrapper>();
        updateWrapper.RegisterUpdateUtil(Instance);
        initiatives = new List<Initiative>();
        initiativeThreshold = 100;
    }

    public void RegisterInitiative(Initiative initiative) {
        initiatives.Add(initiative);
    }

    public void EndCurrentActorTurn() {
        activeActor = null;
    }

    private void StartNextActorsTurn() {
        Initiative topInitiative = GetTopInitiative();
        BattleActorInterface battleActor = topInitiative.gameObject.GetComponent<BattleActorInterface>();
        if (battleActor == null) {
            topInitiative.ResetTurnInitiative();
            StartNextActorsTurn();
        }
        activeActor = battleActor;
        battleActor.StartTurn();
    }

    private Initiative GetTopInitiative() {
        Initiative topInitiative = null;
        List<Initiative> orderedInititatives = GetInitiativeOrder();
        int highestInitiative = orderedInititatives[0].GetTurnInitiative();
        List<Initiative> topInitiatives = new List<Initiative>();
        foreach (Initiative orderedInitiative in orderedInititatives) {
            if (orderedInitiative.GetTurnInitiative() == highestInitiative) {
                topInitiatives.Add(orderedInitiative);
            } else {
                break;
            }
        }
        if (topInitiatives.Count > 1) {
            // If we have more than one top initiative pick the one with the highest speed
            List<Initiative> topTurnSpeeds = GetTopTurnSpeeds(topInitiatives);
            if (topTurnSpeeds.Count > 1) {
                // If we also have top intiatives with highest speeds then pick randomly
                topInitiative = topTurnSpeeds[Random.Range(1, topTurnSpeeds.Count) - 1];
            } else {
                topInitiative = topTurnSpeeds[0];
            }
        } else {
            topInitiative = topInitiatives[0];
        }
        return topInitiative;
    }

    private List<Initiative> GetTopTurnSpeeds(List<Initiative> initiatives) {
        List<Initiative> topTurnSpeeds = initiatives.OrderByDescending(o => o.GetTurnSpeed()).ToList();
        return topTurnSpeeds;
    }

    private List<Initiative> GetInitiativeOrder() {
        List<Initiative> orderedInitiative = initiatives.OrderByDescending(o => o.GetTurnInitiative()).ToList();
        return orderedInitiative;
    }

    private bool ActorsReady() {
        bool actorsReady = false;
        foreach (Initiative initiative in initiatives) {
            if (initiative.GetTurnInitiative() >= initiativeThreshold) {
                actorsReady = true;
                break;
            }
        }
        return actorsReady;
    }

    public void Update() {
        if (activeActor == null) {
            foreach (Initiative initiative in initiatives) {
                initiative.UpdateTurnInitiative();
            }
            if (ActorsReady()) {
                StartNextActorsTurn();
            }
        }
    }
}
