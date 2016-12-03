using UnityEngine;
using System.Collections;

public class BattleCharacter : MonoBehaviour {

    [SerializeField]
    private string characterName;
    [SerializeField]
    private string characterClass;
    [SerializeField]
    private Sprite portrait;

    [SerializeField]
    private int baseSkillPoints;

    private int currentSkillPoints;

    private bool playerCharacter = false;

    void Start() {
        SetSprite();
    }

    private void SetSprite() {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        string spriteLocation;
        if (playerCharacter) {
            spriteLocation = "Characters/Ally" + characterClass;
        } else {
            spriteLocation = "Characters/Enemy" + characterClass;
        }
        spriteRenderer.sprite = Resources.Load(spriteLocation, typeof(Sprite)) as Sprite;
    }

    public void SetAsPlayerCharacter() {
        playerCharacter = true;
    }

}
