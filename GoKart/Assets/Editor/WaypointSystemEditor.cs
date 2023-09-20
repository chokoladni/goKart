using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaypointSystem), true)]
public class WaypointSystemEditor : Editor
{

    public void OnSceneGUI() {
        WaypointSystem ws = target as WaypointSystem;

        List<Vector3> waypoints = ws.waypoints;

        for (int i = 0; i < waypoints.Count; i++) {
            //enable dragging of vectors
            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.PositionHandle(waypoints[i], Quaternion.identity);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(target, "Move point");
                ws.updateWaypoint(i, newPos);
            }
            //draw sphere and line
            Handles.color = i == 0 ? Color.yellow : Color.red;
            Handles.DrawLine(waypoints[i], waypoints[(i + 1) % waypoints.Count]);
            Handles.SphereHandleCap(0, waypoints[i], Quaternion.identity, 1.0f, EventType.Repaint);
        }


        if (Event.current.type == EventType.MouseDown && Event.current.button == 0) {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(worldRay, out hitInfo, Mathf.Infinity)) {
                if (Event.current.modifiers == EventModifiers.Shift) {
                    Undo.RecordObject(target, "Add waypoint");
                    ws.insertWaypoint(hitInfo.point);
                } else if (Event.current.modifiers == EventModifiers.Control) {
                    Undo.RecordObject(target, "Add waypoint");
                    ws.appendWaypoint(hitInfo.point);
                }
                Event.current.Use();
            }

        }

    }
}
