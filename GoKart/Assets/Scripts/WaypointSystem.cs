using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class WaypointSystem : MonoBehaviour
{
    public int lapCount = 3;
    public float raceEndDelay = 5.0f;
    public static float checkpointDistance = 30.0f;
    public List<Vector3> waypoints;

    private List<GameObject> trackedObjects = new List<GameObject>();

    private Dictionary<GameObject, float> activeSegments = new Dictionary<GameObject, float>();
    private List<GameObject> positions = new List<GameObject>();
    private List<GameObject> finalPositions = new List<GameObject>();

    private ScoreManager scoreManager;
    private float timer;

    private bool raceActive = false;

    private string playerTag = "Player";

    public void TrackObject(GameObject objectToTrack, string name) {
        trackedObjects.Add(objectToTrack);
        activeSegments.Add(objectToTrack, -1);
        positions.Add(objectToTrack);
        scoreManager.SetScore(playerTag + trackedObjects.Count, "car", name);
        if(!raceActive) {
            objectToTrack.GetComponent<BasicCarController>().SetInputEnabled(false);
        }
    }

    private void Awake() {
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        StartRace();
    }

    /*private void Start() {
        
        int playerIndex = 1;
        foreach(GameObject trackedObject in trackedObjects) {
            activeSegments.Add(trackedObject, -1);
            positions.Add(trackedObject);
            scoreManager.SetScore(playerTag + playerIndex, "car", trackedObject.transform.root.name);
            playerIndex++;
            trackedObject.GetComponent<BasicCarController>().SetInputEnabled(false);
        }
        */
        //waypoint debug draw
        /*foreach(Vector3 wp in waypoints) {
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = wp;
            sphere.transform.localScale = new Vector3(checkpointDistance, checkpointDistance, checkpointDistance) ;
        }*/
        //SceneManager.LoadScene("EndScreenScene");
        /*
        StartRace();
    }
    */

    public void StartRace() {
        raceActive = true;
        foreach(GameObject trackedObject in trackedObjects) {
            trackedObject.GetComponent<BasicCarController>().SetInputEnabled(true);
        }
    }

    private float getParameterForPoint(int segment, Vector3 point) {
        Vector3 w1 = waypoints[mod(segment, waypoints.Count)];
        Vector3 w2 = waypoints[mod((segment + 1), waypoints.Count)];

        Vector3 direction = (w2 - w1);
        float magnitude = direction.magnitude;
        direction.Normalize();

        float dotP = Mathf.Clamp(Vector3.Dot(point - w1, direction), 0, magnitude);

        return dotP / magnitude;
    }

    public int GetPosition(GameObject trackedObject) {
        if(finalPositions.Contains(trackedObject)) {
            return finalPositions.IndexOf(trackedObject) + 1;
        } else {
            return finalPositions.Count + positions.IndexOf(trackedObject) + 1;
        }
    }

    public int GetLap(GameObject trackedObject) {
        return Mathf.Max(0, Mathf.FloorToInt(activeSegments[trackedObject] / waypoints.Count));
    }

    public int GetPlayerCount() {
        return trackedObjects.Count;
    }

    public int GetLapCount() {
        return lapCount;
    }

    public float getRaceTime(GameObject trackedObject) {
        if(finalPositions.Contains(trackedObject)) {
            return float.Parse(scoreManager.GetScore(playerTag + (trackedObjects.IndexOf(trackedObject) + 1), "time"));
        } else { 
            return timer;
        }
    }

    public Vector3 GetResetPosition(GameObject trackedobject) {
        float totalParam = activeSegments[trackedobject];
        int segment = Mathf.FloorToInt(totalParam);
        float currentParam = getParameterForPoint(segment, trackedobject.transform.position);
        Vector3 w1 = waypoints[mod((segment), waypoints.Count)];
        Vector3 w2 = waypoints[mod((segment + 1), waypoints.Count)];

        return (w2 - w1) * currentParam + w1;
    }

    public Quaternion GetResetRotation(GameObject trackedObject) {
        float totalParam = activeSegments[trackedObject];
        int segment = Mathf.FloorToInt(totalParam);
        Vector3 rotDirection = (waypoints[mod((segment + 1), waypoints.Count)] - waypoints[mod((segment), waypoints.Count)]).normalized;
        return Quaternion.LookRotation(rotDirection);
    }

    private void Update() {
        if(!raceActive) {
            return;
        }
        timer += Time.deltaTime;
        foreach(GameObject trackedObject in trackedObjects) {
            float totalParam = activeSegments[trackedObject];
            int floored = Mathf.FloorToInt(totalParam);
            float currentParam = getParameterForPoint(floored, trackedObject.transform.position);
            if(currentParam < 1.0f || Vector3.Distance(trackedObject.transform.position, waypoints[mod((floored + 1), waypoints.Count)]) < checkpointDistance) {
                activeSegments[trackedObject] = floored + currentParam;
                if (GetLap(trackedObject) == GetLapCount() && !finalPositions.Contains(trackedObject)) {
                    finalPositions.Add(trackedObject);
                    positions.Remove(trackedObject);
                    scoreManager.SetScore(playerTag + (trackedObjects.IndexOf(trackedObject) + 1), "time", timer.ToString());
                    trackedObject.GetComponent<BasicCarController>().SetInputEnabled(false);
                }
            }
        }
        if(positions.Count == 0) {
            raceActive = false;
            StartCoroutine("StartEndScreenScene");
        }
        positions.Sort((a, b) => Comparer<float>.Default.Compare(activeSegments[b], activeSegments[a]));
    }

    public IEnumerator StartEndScreenScene() {
        yield return new WaitForSeconds(raceEndDelay);
        SceneManager.LoadScene("EndScreenScene");
    }

    public void insertWaypoint(Vector3 waypoint) {
        int candidateIndex = 0;
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < waypoints.Count; i++) {
            Vector3 w1 = waypoints[i];
            Vector3 w2 = waypoints[mod(i + 1, waypoints.Count)];

            //float distance = HandleUtility.DistancePointLine(waypoint, w1, w2);
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
    }

    public void appendWaypoint(Vector3 waypoint) {
        insertWaypoint(waypoints.Count, waypoint);
    }

    public void updateWaypoint(int index, Vector3 newPos) {
        Vector3 diff = newPos - waypoints[index];
        waypoints[index] = newPos;
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
