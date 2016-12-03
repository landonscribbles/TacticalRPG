using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BoardUtils {

    private int characterZLevel;
    private float boardXBound;
    private float boardYBound;
    private List<Tile> tiles = new List<Tile>();

    private static BoardUtils instance;

    private BoardUtils() {
        characterZLevel = 10;
    }

    public static BoardUtils Instance {
        get {
            if (instance == null) {
                instance = new BoardUtils();
            }
            return instance;
        }
    }

    public int GetCharacterZLevel() {
        return characterZLevel;
    }

    public void RegisterTile(Tile tile) {
        tiles.Add(tile);
        if (tile.GetGridLocation().x > boardXBound) {
            boardXBound = tile.GetGridLocation().x;
        }
        if (tile.GetGridLocation().y > boardYBound) {
            boardYBound = tile.GetGridLocation().y;
        }
    }

    public Vector3 GetWorldPositionFromTileGrid(Vector2 tileGridLocation) {
        Vector3 returnLocation = new Vector3(-1000, -1000, -1000);
        foreach (Tile tile in tiles) {
            Vector3 tileWorldPos = tile.GetGridLocation();
            if (tileWorldPos.x == tileGridLocation.x && tileWorldPos.y == tileGridLocation.y) {
                returnLocation = tile.transform.position;
                break;
            }
        }
        if (returnLocation.z == -1000) {
            Debug.Log("Tile lookup failed for tile at: " + tileGridLocation);
        }
        return returnLocation;
    }

    public Tile GetTile(Vector2 tileGridLocation) {
        Tile returnTile = null;
        foreach(Tile tile in tiles) {
            Vector2 tileLoc = tile.GetGridLocation();
            if (tileLoc.x == tileGridLocation.x && tileLoc.y == tileGridLocation.y) {
                returnTile = tile;
                break;
            }
        }
        return returnTile;
    }

    public void SetMoveRangeHighlightTiles(List<Vector2> tilesToHighlight) {
        foreach(Vector2 tileLocation in tilesToHighlight) {
            Tile tile = GetTile(tileLocation);
            tile.EnableMovementHighlight();
        }
    }

    public void RemoveMoveRangeHighlightTiles(List<Vector2> tilesToHighlight) {
        foreach (Vector2 tileLocation in tilesToHighlight) {
            Tile tile = GetTile(tileLocation);
            tile.DisableMovementHighlight();
        }
    }

    public void SetAttackRangeHighlightTiles(List<Vector2> tilesToHighlight) {
        foreach (Vector2 tileLocation in tilesToHighlight) {
            Tile tile = GetTile(tileLocation);
            tile.EnableAttackHighlight();
        }
    }

    public void RemoveAttackRangeHighlightTiles(List<Vector2> tilesToHighlight) {
        foreach (Vector2 tileLocation in tilesToHighlight) {
            Tile tile = GetTile(tileLocation);
            tile.DisableAttackHighlight();
        }
    }

    private Dictionary<Vector2, int> GetNeighbors(Vector2 startGridPoint) {
        List<Vector2> vectorAdditions = new List<Vector2>() {
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(-1, 0),
            new Vector2(0, -1)
        };

        Dictionary<Vector2, int> neighbors = new Dictionary<Vector2, int>();

        foreach(Vector2 vectorAdd in vectorAdditions) {
            Vector2 potentialNeighbor = startGridPoint + vectorAdd;
            if (potentialNeighbor.x > boardXBound || potentialNeighbor.x < 0 || potentialNeighbor.y > boardYBound || potentialNeighbor.y < 0) {
                continue;
            } else {
                Tile tile = GetTile(potentialNeighbor);
                if (tile == null) {
                    continue;
                }
                neighbors[potentialNeighbor] = tile.GetMovementCost();
            }
        }
        return neighbors;
    }

    public List<Vector2> GetPathRoute(Vector2 gridStartPoint, Vector2 gridEndPoint, int moveRange, List<Vector2> moveableTiles) {
        Debug.Log("Moveable tiles: " + moveableTiles.Count);

        Dictionary<Vector2, int> unvisitedTiles = new Dictionary<Vector2, int>();
        Dictionary<Vector2, int> visitedTiles = new Dictionary<Vector2, int>();
        Dictionary<Vector2, Vector2> traveledPath = new Dictionary<Vector2, Vector2>();

        foreach (Vector2 tileLocation in moveableTiles) {
            unvisitedTiles[tileLocation] = 10000;
            traveledPath[tileLocation] = new Vector2(-1, -1);
        }

        Vector2 currentPoint = gridStartPoint;
        int currentDistance = 0;
        unvisitedTiles[currentPoint] = currentDistance;

        while (true) {
            Dictionary<Vector2, int> neighborDistance = GetNeighbors(currentPoint);
            foreach (KeyValuePair<Vector2, int> neighbor in neighborDistance) {
                if (!unvisitedTiles.Keys.ToList().Contains(neighbor.Key)) {
                    continue;
                }
                int newDistance = currentDistance + neighbor.Value;
                if (unvisitedTiles[neighbor.Key] > newDistance) {
                    unvisitedTiles[neighbor.Key] = newDistance;
                    traveledPath[neighbor.Key] = currentPoint;
                }
            }
            visitedTiles[currentPoint] = currentDistance;
            unvisitedTiles.Remove(currentPoint);
            if (unvisitedTiles.Count == 0) {
                break;
            }
            List<KeyValuePair<Vector2, int>> orderedCandidates = unvisitedTiles.ToList().OrderBy(o => o.Value).ToList();
            KeyValuePair<Vector2, int> nextNode = orderedCandidates[0];
            currentPoint = nextNode.Key;
            currentDistance = nextNode.Value;
        }

        List<KeyValuePair<Vector2, int>> shortPathDistance = new List<KeyValuePair<Vector2, int>>();
        List<Vector2> shortPath = new List<Vector2>();

        Vector2 previousPoint = gridEndPoint;

        while (previousPoint != gridStartPoint) {
            KeyValuePair<Vector2, int> pointAndCost = new KeyValuePair<Vector2, int>(previousPoint, visitedTiles[previousPoint]);
            shortPathDistance.Add(pointAndCost);
            previousPoint = traveledPath[previousPoint];
        }

        shortPathDistance.Reverse();

        foreach(KeyValuePair<Vector2, int> move in shortPathDistance) {
            if (move.Value > moveRange) {
                break;
            }
            shortPath.Add(move.Key);
        }
        return shortPath;
    }

    // FIXME: This way of calculating this is terrible and needs to be replaced
    // public HashSet<Vector2> Calculate2DTileRange(Vector2 startingPoint, int range) {
    public List<Vector2> Calculate2DTileRange(Vector2 startingPoint, int range) {
        // HashSet<Vector2> tileRange = new HashSet<Vector2>();
        List<Vector2> tileRange = new List<Vector2>();
        Vector2 rangePoint;
        foreach (int i in Enumerable.Range(0, range)) {
            rangePoint = new Vector2(startingPoint.x + i, startingPoint.y);
            tileRange.Add(rangePoint);
            rangePoint = new Vector2(startingPoint.x - i, startingPoint.y);
            tileRange.Add(rangePoint);
            rangePoint = new Vector2(startingPoint.x, startingPoint.y + i);
            tileRange.Add(rangePoint);
            rangePoint = new Vector2(startingPoint.x, startingPoint.y - i);
            tileRange.Add(rangePoint);
        }
        int xVal;
        int yVal;
        foreach (int i in Enumerable.Range(0, range)) {
            if (i == 0) {
                continue;
            }
            xVal = i - 1;
            yVal = (range - i) + (int)startingPoint.y;
            while (yVal > startingPoint.y) {
                rangePoint = new Vector2(startingPoint.x + xVal, yVal);
                tileRange.Add(rangePoint);
                yVal -= 1;
            }

            xVal = (i - 1) * -1;
            yVal = (range - i) + (int)startingPoint.y;
            while (yVal > startingPoint.y) {
                rangePoint = new Vector2(startingPoint.x + xVal, yVal);
                tileRange.Add(rangePoint);
                yVal -= 1;
            }

            xVal = (i - 1) * -1;
            yVal = ((range - i) * -1) + (int)startingPoint.y;
            while (yVal < startingPoint.y) {
                rangePoint = new Vector2(startingPoint.x + xVal, yVal);
                tileRange.Add(rangePoint);
                yVal += 1;
            }

            xVal = i - 1;
            yVal = ((range - i) * -1) + (int)startingPoint.y;
            while (yVal < startingPoint.y) {
                rangePoint = new Vector2(startingPoint.x + xVal, yVal);
                tileRange.Add(rangePoint);
                yVal += 1;
            }
        }
        return tileRange;
    }

}
