using UnityEngine;
using System.Collections;

public class TileHighlight : MonoBehaviour {

    [SerializeField]
    private GameObject tileHighlightPrefab;
    private GameObject tileHighlightObject;
    private Vector3 highlightTileLocation;
    private bool highlightTileActive;
    [SerializeField]
    private float maxHighlightOpacity;
    [SerializeField]
    private float highlightCycleStep;

    void Start() {
        highlightTileLocation = transform.position;
    }

    void Update() {
        if (highlightTileActive) {
            CycleTileHighlight();
        }
    }

    public void EnableTileHighlight() {
        highlightTileActive = true;
        tileHighlightObject = Instantiate(tileHighlightPrefab, highlightTileLocation, Quaternion.identity) as GameObject;
        Color tempColor = tileHighlightObject.GetComponent<SpriteRenderer>().color;
        tempColor.a = 0;
        tileHighlightObject.GetComponent<SpriteRenderer>().color = tempColor;
    }

    public void DisableTileHighlight() {
        highlightTileActive = false;
        Destroy(tileHighlightObject);
        highlightCycleStep = Mathf.Abs(highlightCycleStep);
    }

    private void CycleTileHighlight() {
        Color tempColor = tileHighlightObject.GetComponent<SpriteRenderer>().color;
        tempColor.a = tempColor.a + (highlightCycleStep * Time.deltaTime);
        if (tempColor.a <= 0.0) {
            tempColor.a = 0;
            highlightCycleStep = highlightCycleStep * -1;
        } else if (tempColor.a >= maxHighlightOpacity) {
            tempColor.a = maxHighlightOpacity;
            highlightCycleStep = highlightCycleStep * -1;
        }
        tileHighlightObject.GetComponent<SpriteRenderer>().color = tempColor;
    }
}
