using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof (TrackWaypointSystem), true)]
[CanEditMultipleObjects]
public class WaypointSystemEditor2 : Editor
{

    readonly GUIStyle style = new GUIStyle();

    void OnEnable()
    {
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
    }
    /*
    public override void OnInspectorGUI()
    {
        TrackWaypointSystem tws = target as TrackWaypointSystem;

        List<Vector3> waypoints = tws.waypoints;
        for (int i = 0; i < waypoints.Count; i++)
        {
            EditorGUILayout.Vector3Field("waypoint " + i, waypoints[i]);
        }
    }
    */

    public void OnSceneGUI()
    {
        //if (Event.current.type == EventType.Layout)
           // HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
        TrackWaypointSystem tws = target as TrackWaypointSystem;

        List<CubicBezierCurve> segments = tws.segments;
        List<Vector3> waypoints = tws.waypoints;

        for (int i = 0; i < waypoints.Count; i++)
        {
            //enable dragging of vectors
            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.PositionHandle(waypoints[i], Quaternion.identity);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(target, "Move point");
                tws.updateWaypoint(i, newPos);
            }
            //draw sphere and line
            Handles.color = i == 0 ? Color.yellow : Color.red;
            Handles.DrawLine(waypoints[i], waypoints[(i + 1) % waypoints.Count]);
            Handles.SphereHandleCap(0, waypoints[i], Quaternion.identity, 1.0f, EventType.Repaint);   
        }

        for(int i = 0; i < segments.Count; i++) {
            Handles.SphereHandleCap(0, segments[i].getControlPoint(1), Quaternion.identity, 0.5f, EventType.Repaint);
            Handles.SphereHandleCap(0, segments[i].getControlPoint(2), Quaternion.identity, 0.5f, EventType.Repaint);

            Handles.DrawBezier(segments[i].getControlPoint(0), segments[i].getControlPoint(3), segments[i].getControlPoint(1), segments[i].getControlPoint(2), Color.cyan, null, 2.0f);
        }

        
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0) {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(worldRay, out hitInfo, Mathf.Infinity)) {
                if (Event.current.modifiers == EventModifiers.Shift) {
                    Undo.RecordObject(target, "Add waypoint");
                    tws.insertWaypoint(hitInfo.point);
                } else if (Event.current.modifiers == EventModifiers.Control) {
                    tws.appendWaypoint(hitInfo.point);
                }
                Event.current.Use();
            }
            
        }
        
    }
}
