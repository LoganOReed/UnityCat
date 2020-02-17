using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapEditor : MonoBehaviour {

    public Color[] colors;
    public HexGrid hexGrid;
    Color activeColor;

    void Awake() {
        SelectColor(0);
    }

    void Update() {
        if (Input.GetMouseButton(0)) {
            HandleInput();
        }
    }

    public void ColorCell(Vector3 position, Color color) {
        position = transform.InverseTransformPoint(position);
        HexCoordinate coordinates = HexCoordinate.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];
        cell.color = color;
        hexMesh.Triangulate(cells);
    }

    void HandleInput() {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            hexGrid.ColorCell(hit.point, activeColor);
        }
    }

    public void SelectColor(int index) {
        activeColor = colors[index];
    }

}
