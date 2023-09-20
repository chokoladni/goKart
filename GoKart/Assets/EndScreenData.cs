using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenData : MonoBehaviour
{
    private Dictionary<string, Dictionary<string, string>> playerData = new Dictionary<string, Dictionary<string, string>>();

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetCar(string player, GameObject gameObject) {

    }
}
