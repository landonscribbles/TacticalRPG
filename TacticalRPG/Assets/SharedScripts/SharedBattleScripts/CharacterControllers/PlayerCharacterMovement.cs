using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerCharacterMovement : MonoBehaviour {

    [SerializeField]
    private int baseMoveRange;
    [SerializeField]
    private float baseMoveSpeed;

    private Tile occupiedTile;
    private Vector2 gridLocation;

    private bool hasMoved;
    private bool isMoving;
    private bool movementTilesHighlighted;
    private List<Vector2> highlightedTiles;
    private List<Vector2> movementPath;
    private Vector3 moveDestination;

    void Update() {
        if (isMoving) {
            MoveToDestination();
        }
    }

    public bool HasMoved() {
        return hasMoved;
    }

    public bool IsMoving() {
        return isMoving;
    }

    private int GetMoveRange() {
        return baseMoveRange;
    }

    private float GetMoveSpeed() {
        return baseMoveSpeed;
    }

    public void HighlightMoveableTiles() {
        if (!hasMoved && !isMoving && !movementTilesHighlighted) {
            highlightedTiles = BoardUtils.Instance.Calculate2DTileRange(gridLocation, GetMoveRange()).ToList();
            BoardUtils.Instance.SetMoveRangeHighlightTiles(highlightedTiles);
            movementTilesHighlighted = true;
        }
    }

    public void RemoveHighlightedMoveableTiles() {
        BoardUtils.Instance.RemoveMoveRangeHighlightTiles(highlightedTiles);
        movementTilesHighlighted = false;
    }

    // Turn menu will handle mouse clicks and pass them here for movement
    public void MoveIfTileIsReachable(GameObject selectedTile) {
        if (selectedTile != null) {
            Tile tile = selectedTile.GetComponent<Tile>();
            Vector2 tileGridLoc = tile.GetGridLocation();
            if (highlightedTiles.Contains(tileGridLoc)) {
                occupiedTile.SetCharacterLeftTile();
                movementPath = BoardUtils.Instance.GetPathRoute(gridLocation, tile.GetGridLocation(), GetMoveRange(), highlightedTiles);
                Vector3 gridWorldPosition = BoardUtils.Instance.GetWorldPositionFromTileGrid(movementPath[0]);
                moveDestination = new Vector3(gridWorldPosition.x, gridWorldPosition.y, BoardUtils.Instance.GetCharacterZLevel());
                RemoveHighlightedMoveableTiles();
                isMoving = true;
            }
        }
    }

    private void MoveToDestination() {
        transform.position = Vector3.MoveTowards(
            transform.position,
            moveDestination,
            GetMoveSpeed() * Time.deltaTime
        );
        if (transform.position == moveDestination) {
            if (movementPath.Count == 1) {
                gridLocation = movementPath[0];
            }
            if (movementPath.Count == 0) {
                isMoving = false;
                hasMoved = true;
                occupiedTile = BoardUtils.Instance.GetTile(gridLocation);
                occupiedTile.SetCharacterOnTile();
                return;
            } else {
                Vector3 gridWorldDest = BoardUtils.Instance.GetWorldPositionFromTileGrid(movementPath[0]);
                moveDestination = new Vector3(gridWorldDest.x, gridWorldDest.y, BoardUtils.Instance.GetCharacterZLevel());
                movementPath.RemoveAt(0);
            }
        }
    }

}
