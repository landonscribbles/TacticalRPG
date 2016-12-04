using UnityEngine;
using System.Collections;

public class BattleLoader : MonoBehaviour {
    // TEMPORARY

    public GameObject[] battleCharacters;
    public GameObject loadedCharacter;

	void Start () {
        Vector3 startingBerserkerPlayer = BoardUtils.Instance.GetWorldPositionFromTileGrid(new Vector2(8, 0));
        loadedCharacter = Instantiate(battleCharacters[0], startingBerserkerPlayer, Quaternion.identity) as GameObject;
        BattleCharacter loadedCharScript = loadedCharacter.GetComponent<BattleCharacter>();
        loadedCharScript.SetAsPlayerCharacter();
	}

}
