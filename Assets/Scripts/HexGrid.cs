using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//struct for Coordinates(HexCoordinates with an s is a unity struct)
[System.Serializable]
public struct HexCoordinate {
    [SerializeField]
    private int x, z;

    public int X {
        get {
            return x;
        }
    }
    public int Z {
        get {
            return z;
        }
    }
    public int Y {
        get {
            return -X - Z;
        }
    }

    public HexCoordinate(int x, int z) {
        this.x = x;
        this.z = z;
    }

    public static HexCoordinate FromOffsetCoordinates(int x, int z) {
        return new HexCoordinate(x - z / 2, z);
    }

    public override string ToString() {
        return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringSeperateLines() {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }

    //gives hexcoords from position of click
    public static HexCoordinate FromPosition(Vector3 position) {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = -x;

        float offset = position.z / (HexMetrics.outerRadius * 3f);
        x -= offset;
        y -= offset;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x -y);

        if(iX + iY + iZ != 0) {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x-y - iZ);

            if(dX > dY && dX > dZ) {
                iX = -iY - iZ;
            }else if(dZ > dY) {
                iZ = -iX - iY;
            }
        }

        return new HexCoordinate(iX, iZ);
    }
}

//
//
//
//
//

//main class for the grid
//initialized grid objects, mesh, and coordinates
public class HexGrid : MonoBehaviour {

    Canvas gridCanvas;

    public int width = 6;
    public int height = 6;

    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;

    public HexCell cellPrefab;
    HexCell[] cells;

    public Text cellLabelPrefab;

    HexMesh hexMesh;

    //initialize components and puts objects in game
    void Awake() {

        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        cells = new HexCell[height * width];

        for(int z=0,i=0;z < height; z++) {
            for(int x = 0; x < width; x++) {
                CreateCell(x, z, i++);
            }
        }
    }

    //creates mesh on start
    void Start() {
        hexMesh.Triangulate(cells);
    }

    //instantiates a hex cell with parent and location and coordinates
    void CreateCell(int x, int z, int i) {
        //position
        Vector3 position;
        position.x = (x + z * 0.5f - (z / 2)) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        //gives position a cell with the needed parent and position
        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinate.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;


        //gives coordinates
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringSeperateLines();

    }

    public void ColorCell(Vector3 position, Color color) {
        position = transform.InverseTransformPoint(position);
        HexCoordinate coordinates = HexCoordinate.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];
        cell.color = color;
        hexMesh.Triangulate(cells);
    }

}
