using System.Collections.Generic;
using UnityEngine;
using System;

public class StartGameManager : MonoBehaviour
{
    public GameObject cameraPrefab;
    public GameObject backgroundCameraPrefab;
    public GameObject playerGUIPrefab;
    private GameObject bgCam;

    public Transform[] startingPositions;

    private WaypointSystem waypointSystem;

    private int frameCount = 2;
    [Serializable]
    public struct NamedPrefab {
        public string name;
        public GameObject prefab;
    }

    public NamedPrefab[] prefabs;
    private Dictionary<string, GameObject> nameToPrefab = new Dictionary<string, GameObject>();


    void Start()
    {
        waypointSystem = GameObject.FindObjectOfType<WaypointSystem>();
        Cursor.visible = false;
        
        foreach(NamedPrefab namedPrefab in prefabs) {
            nameToPrefab.Add(namedPrefab.name, namedPrefab.prefab);
        }

        List<string> carNames = new List<string>();
        for(int i = 1; i <= 4; i++) {
            string name = PlayerPrefs.GetString("Player" + i);
            if(!string.IsNullOrEmpty(name)) {
                carNames.Add(name);
            }
        }

        List<GameObject> cars = InstantiateCars(carNames);
        
        List<GameObject> players = new List<GameObject>();
        foreach(var car in cars) {
            players.Add(car.GetComponentInChildren<BasicCarController>().gameObject);
        }

        List<Camera> cameras = SetupCameras(players);
        SetupGUI(players, cameras);
    }

    private List<GameObject> InstantiateCars(List<string> carNames) {
        List<GameObject> cars = new List<GameObject>();
        GameObject prefab;
        for (int i = 0; i < carNames.Count; i++) {
            if (nameToPrefab.TryGetValue(carNames[i], out prefab)) {
                GameObject car = Instantiate(prefab, startingPositions[i]);
                car.GetComponentInChildren<BasicCarController>().playerID = i + 1;
                cars.Add(car);
                waypointSystem.TrackObject(cars[i].GetComponentInChildren<BasicCarController>().gameObject, carNames[i]);
            }
        }

        return cars;
    }

    private List<Camera> SetupCameras(List<GameObject> players) {
        int playerCount = players.Count;
        float viewportWidth = playerCount > 2 ? 0.5f : 1.0f;
        float viewportHeight = playerCount != 1 ? 0.5f : 1.0f;

        if(playerCount == 3) { //because then there is unused screen space
            bgCam = Instantiate(backgroundCameraPrefab);
            bgCam.GetComponent<Camera>().depth = -1;
        }

        List<Camera> cameras = new List<Camera>();
        for (int i = 0; i < playerCount; i++) {
            GameObject camera = Instantiate(cameraPrefab);
            cameras.Add(camera.GetComponent<Camera>());
            camera.GetComponent<CameraFollow>().objectToFollow = players[i].transform;

            float viewportX;
            float viewportY;
            if (playerCount == 1) {
                viewportX = 0;
                viewportY = 0;
            } else if(playerCount == 2) {
                viewportX = 0.0f;
                viewportY = i == 0 ? 0.5f : 0.0f;
            } else {
                viewportX = 0.5f * (i % 2);
                if (playerCount == 3 && i == 2) {
                    viewportX = 0.25f;
                }
                viewportY = i < 2 ? 0.5f : 0.0f;
            }

            camera.GetComponent<Camera>().rect = new Rect(viewportX, viewportY, viewportWidth, viewportHeight);
        }

        return cameras;
    }

    private void SetupGUI(List<GameObject> players, List<Camera> cameras) {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        for(int i = 0; i < players.Count; i++) {
            GameObject playerGUI = Instantiate(playerGUIPrefab);
            playerGUI.GetComponentInChildren<PlayerGUIData>().player = players[i];

            RectTransform[] rects = playerGUI.GetComponentsInChildren<RectTransform>();

            Vector2 cameraPos = cameras[i].rect.position;
            Vector2 cameraSize = cameras[i].rect.size;
            foreach(RectTransform rect in rects) {
                rect.anchorMin = Vector2.Scale(rect.anchorMin, cameraSize) + cameraPos;
                rect.anchorMax = Vector2.Scale(rect.anchorMax, cameraSize) + cameraPos;
            }
        }
    }
}
