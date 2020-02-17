using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//allows us to look at coordinates of a given cell in the editor
[CustomPropertyDrawer(typeof(HexCoordinate))]
public class HexCoordinatesDrawer : PropertyDrawer {

    public override void OnGUI(
        Rect position, SerializedProperty property, GUIContent label
    ) {
        HexCoordinate coordinates = new HexCoordinate(
            property.FindPropertyRelative("x").intValue,
            property.FindPropertyRelative("z").intValue
            );
        position = EditorGUI.PrefixLabel(position, label);
        GUI.Label(position, coordinates.ToString());
    }

}
