using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions;

public class TrackWaypointSystem : MonoBehaviour
{

    public List<Vector3> waypoints;
    public List<CubicBezierCurve> segments = new List<CubicBezierCurve>();

    public void insertWaypoint(Vector3 waypoint) {
        int candidateIndex = 0;
        float minDistance = Mathf.Infinity;
        for(int i = 0; i < waypoints.Count; i++) {
            Vector3 w1 = waypoints[i];
            Vector3 w2 = waypoints[mod(i + 1, waypoints.Count)];
            
           // float distance = HandleUtility.DistancePointLine(waypoint, w1, w2);
            float distance = DistancePointLine(waypoint, w1, w2);
            if (distance < minDistance) {
                minDistance = distance;
                candidateIndex = mod(i + 1, waypoints.Count + 1);
            }
        }
        insertWaypoint(candidateIndex, waypoint);
    }

    public void insertWaypoint(int index, Vector3 waypoint) {
        waypoints.Insert(index, waypoint);

        if (waypoints.Count < 2) {
            return;
        }
        
        if (segments.Count == 0) {
            Assert.IsTrue(waypoints.Count == 2);

            segments.Add(new CubicBezierCurve(new Vector3[] {
                waypoints[0],
                waypoints[0] + new Vector3(1, 1, 1),
                waypoints[1] - new Vector3(1, 1, 1),
                waypoints[1]
            }));
            segments.Add(new CubicBezierCurve(new Vector3[] {
                waypoints[1],
                waypoints[1] + new Vector3(1, 1, 1),
                waypoints[0] - new Vector3(1, 1, 1),
                waypoints[0]
            }));

            return;
        }
        
        CubicBezierCurve previousSegment = segments[mod(index - 1, segments.Count)];
        CubicBezierCurve nextSegment = segments[mod(index, segments.Count)];

        Vector3 tangentDirection = (previousSegment.getControlPoint(3) - previousSegment.getControlPoint(0)).normalized;
        float tangentMagnitude = (previousSegment.getControlPoint(3) - previousSegment.getControlPoint(2)).magnitude;
        CubicBezierCurve newSegment = new CubicBezierCurve(new Vector3[] {
            waypoint,
            waypoint + tangentDirection * tangentMagnitude,
            previousSegment.getControlPoint(2),
            nextSegment.getControlPoint(0)
        });
        previousSegment.setControlPoint(2, waypoint - tangentDirection * tangentMagnitude);
        previousSegment.setControlPoint(3, waypoint);

        segments.Insert(index, newSegment);
    }

    public void appendWaypoint(Vector3 waypoint) {
        insertWaypoint(waypoints.Count, waypoint);
    }

    public void updateWaypoint(int index, Vector3 newPos) {
        Assert.IsTrue(segments.Count == waypoints.Count);
        Vector3 diff = newPos - waypoints[index];
        waypoints[index] = newPos;
        segments[index].setControlPoint(0, newPos);
        segments[index].setControlPoint(1, segments[index].getControlPoint(1) + diff);
        segments[mod(index - 1, segments.Count)].setControlPoint(3, newPos);
        segments[mod(index - 1, segments.Count)].setControlPoint(2, segments[mod(index - 1, segments.Count)].getControlPoint(2) + diff);
    }

    private int mod(int a, int b) {
        int remainder = a % b;
        return remainder >= 0 ? remainder : (remainder + b);
    }
    // Calculate distance between a point and a line.
    public static float DistancePointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        return Vector3.Magnitude(ProjectPointLine(point, lineStart, lineEnd) - point);
    }
    // Project /point/ onto a line.
    public static Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 relativePoint = point - lineStart;
        Vector3 lineDirection = lineEnd - lineStart;
        float length = lineDirection.magnitude;
        Vector3 normalizedLineDirection = lineDirection;
        if (length > .000001f)
            normalizedLineDirection /= length;

        float dot = Vector3.Dot(normalizedLineDirection, relativePoint);
        dot = Mathf.Clamp(dot, 0.0F, length);

        return lineStart + normalizedLineDirection * dot;
    }
}