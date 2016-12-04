using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionsMenu : MonoBehaviour {

    private Text characterName;
    private Text characterClass;
    private Text characterHP;
    private Text characterSP;
    private Image characterPortrait;

    private Action highlightTileAction;
    private Action attackButtonAction;
    private Action endTurnButtonAction;

    void Start() {
        characterPortrait = GameObject.Find("ActionMenuPortrait").GetComponent<Image>();
        characterName = GameObject.Find("ActionMenuCharacterName").GetComponent<Text>();
        characterClass = GameObject.Find("ActionMenuClass").GetComponent<Text>();
        characterHP = GameObject.Find("ActionMenuHP").GetComponent<Text>();
        characterSP = GameObject.Find("ActionMenuSP").GetComponent<Text>();
    }

    public void Activate() {
        gameObject.SetActive(true);
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }

    public void SetCharacterName(string newCharacterName) {
        characterName.text = newCharacterName;
    }

    public void SetCharacterClass(string newClassName) {
        characterClass.text = newClassName;
    }

    public void SetPortrait(Sprite newPortrait) {
        characterPortrait.sprite = newPortrait;
    }

    public void SetHitPoints(int currentHP, int maxHP) {
        characterHP.text = "HP: " + currentHP + "/" + maxHP;
    }

    public void SetSkillPoints(int currentSP, int maxSP) {
        characterSP.text = "SP: " + currentSP + "/" + maxSP;
    }

}
