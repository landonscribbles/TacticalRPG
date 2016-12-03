using UnityEngine;
using System.Collections;

public class BattleLoader : MonoBehaviour {

    public GameObject[] battleCharacters;
    public GameObject loadedCharacter;

	void Start () {
        Vector2 loadCharLocation = new Vector2(0, 0);
        loadedCharacter = Instantiate(battleCharacters[0], loadCharLocation, Quaternion.identity) as GameObject;
        BattleCharacter loadedCharScript = loadedCharacter.GetComponent<BattleCharacter>();
        loadedCharScript.SetAsPlayerCharacter();
	}

}
