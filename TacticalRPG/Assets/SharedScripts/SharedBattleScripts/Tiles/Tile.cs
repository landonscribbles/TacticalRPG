using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    [SerializeField]
    private Vector2 gridLocation;
    // private Vector3 worldPosition;
    private bool tileOccupied;

    [SerializeField]
    private int movementCost;

    [SerializeField]
    private GameObject movementHighlightPrefab;
    private TileHighlight movementHighlight;
    [SerializeField]
    private GameObject attackHighlightPrefab;
    private TileHighlight attackHighlight;

    void Start() {
        tileOccupied = false;
        // worldPosition = transform.position;
        GameObject movementHighlightObject = Instantiate(movementHighlightPrefab, transform.position, Quaternion.identity) as GameObject;
        movementHighlight = movementHighlightObject.GetComponent<TileHighlight>();
        GameObject attackHighlightObject = Instantiate(attackHighlightPrefab, transform.position, Quaternion.identity) as GameObject;
        attackHighlight = attackHighlightObject.GetComponent<TileHighlight>();
        BoardUtils.Instance.RegisterTile(this);
    }

    public void SetCharacterOnTile() {
        tileOccupied = true;
    }

    public void SetCharacterLeftTile() {
        tileOccupied = false;
    }

    public void EnableMovementHighlight() {
        movementHighlight.EnableTileHighlight();
    }

    public void DisableMovementHighlight() {
        movementHighlight.DisableTileHighlight();
    }

    public void EnableAttackHighlight() {
        attackHighlight.EnableTileHighlight();
    }

    public void DisableAttackHighlight() {
        attackHighlight.DisableTileHighlight();
    }

    public int GetMovementCost() {
        if (tileOccupied) {
            movementCost = 10000;
        }
        return movementCost;
    }

    public Vector2 GetGridLocation() {
        return gridLocation;
    }

}