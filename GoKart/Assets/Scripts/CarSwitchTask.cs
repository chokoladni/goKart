using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSwitchTask : MonoBehaviour
{

    private GameObject Jeep;
    private GameObject PickupVan;
    private GameObject RedCar;
    private GameObject PlayerNotPlaying;
    public GameObject CarsParent;
    private bool[] inputAllowed = new bool[4] { true , true , true , true };

    int counter = 0;
    

    public void Start()
    {
        PlayerPrefs.SetString("Player1", "");
        PlayerPrefs.SetString("Player2", "");
        PlayerPrefs.SetString("Player3", "");
        PlayerPrefs.SetString("Player4", "");
    }
    private void Update()
    {
        char playerIndexChar =  CarsParent.transform.tag[CarsParent.transform.tag.Length - 1];
        
        int index = (int)(playerIndexChar - '0'); 
        
        bool pressedLeft = false;
        bool pressedRight = false;
        switch (index)
        {

            case 1:
                if (!inputAllowed[0])
                {
                    pressedLeft = false;
                    pressedRight = false;
                    return;
                }
                pressedLeft = Input.GetKeyDown(KeyCode.Joystick1Button4);
                pressedRight = Input.GetKeyDown(KeyCode.Joystick1Button5);
                float horizontalInput = Input.GetAxis("Horizontal1");
                if (Input.GetAxis("Horizontal1Key") != 0)
                {
                    horizontalInput = Input.GetAxis("Horizontal1Key");
                }
                if (horizontalInput < -0.5)
                {
                    pressedLeft = true;
                    
                }
                if (horizontalInput > 0.5)
                {
                    pressedRight = true;
                }
                if (pressedLeft || pressedRight)
                {
                    StartCoroutine(waitInput(0));
                }
                break;

            case 2:
                if (!inputAllowed[1])
                {
                    pressedLeft = false;
                    pressedRight = false;
                    return;
                }
                pressedLeft = Input.GetKeyDown(KeyCode.Joystick2Button4);
                pressedRight = Input.GetKeyDown(KeyCode.Joystick2Button5);
                float horizontalInput2 = Input.GetAxis("Horizontal2");
                if (Input.GetAxis("Horizontal2Key") != 0)
                {
                    horizontalInput2 = Input.GetAxis("Horizontal2Key");
                }
                if (horizontalInput2 < -0.5)
                {
                    pressedLeft = true;

                }
                if (horizontalInput2 > 0.5)
                {
                    pressedRight = true;
                }
                if (pressedLeft || pressedRight)
                {
                    StartCoroutine(waitInput(1));
                }
                break;
            case 3:
                if (!inputAllowed[2])
                {
                    pressedLeft = false;
                    pressedRight = false;
                    return;
                }
                pressedLeft = Input.GetKeyDown(KeyCode.Joystick3Button4);
                pressedRight = Input.GetKeyDown(KeyCode.Joystick3Button5);
                float horizontalInput3 = Input.GetAxis("Horizontal3");
                if (Input.GetAxis("Horizontal3Key") != 0)
                {
                    horizontalInput3 = Input.GetAxis("Horizontal3Key");
                }
                if (horizontalInput3 < -0.5)
                {
                    pressedLeft = true;

                }
                if (horizontalInput3 > 0.5)
                {
                    pressedRight = true;
                }
                if (pressedLeft || pressedRight)
                {
                    StartCoroutine(waitInput(2));
                }
                break;
            case 4:
                if (!inputAllowed[3])
                {
                    pressedLeft = false;
                    pressedRight = false;
                    return;
                }
                pressedLeft = Input.GetKeyDown(KeyCode.Joystick4Button4);
                pressedRight = Input.GetKeyDown(KeyCode.Joystick4Button5);
                float horizontalInput4 = Input.GetAxis("Horizontal4");
                if (Input.GetAxis("Horizontal4Key") != 0)
                {
                    horizontalInput4 = Input.GetAxis("Horizontal4Key");
                }
                if (horizontalInput4 < -0.5)
                {
                    pressedLeft = true;

                }
                if (horizontalInput4 > 0.5)
                {
                    pressedRight = true;
                }
                if (pressedLeft || pressedRight)
                {
                    StartCoroutine(waitInput(3));
                }
                break;

        }
        if (pressedLeft)
        {
            SwitchCarLeft(CarsParent);
        }
        else if (pressedRight)
        {
            SwitchCarRight(CarsParent);
        }
    }
    private void SwitchCar(GameObject Cars , int Direction)
    {
        GameObject[] Array = new GameObject[5];
        foreach (Transform t in Cars.transform)
        {
            Array[counter] = t.gameObject;
            counter++;
        }


        counter = 0;
        foreach (GameObject t in Array)
        {
            if (t.gameObject.activeSelf)
            {
                /* Left-0, Right-1 */
                counter = Direction == 0 ? --counter : ++counter;
                
                t.gameObject.SetActive(false);
                Array[((counter % 5) + 5) % 5].SetActive(true);
                /* If player is not playing, value string is empty */
                if(Array[((counter % 5) + 5) % 5].name == "PlayerNotPlaying")
                {
                    PlayerPrefs.SetString(Cars.transform.tag, "");
                }
                else
                {
                    PlayerPrefs.SetString(Cars.transform.tag, Array[((counter % 5) + 5) % 5].name);
                }
                break;
            }
            counter++;
        }
        counter = 0;
    }

    public void SwitchCarRight(GameObject Cars)
    {
        SwitchCar(Cars, 1);
    }

    public void SwitchCarLeft(GameObject Cars)
    {
        SwitchCar(Cars, 0);
    }
    IEnumerator waitInput(int i)
    {
        inputAllowed[i] = false;
        yield return new WaitForSeconds(0.3f);
        inputAllowed[i] = true;
    }
}
