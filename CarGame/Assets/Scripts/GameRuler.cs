using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRuler : MonoBehaviour
{
    // Parameters of GameRuler class
    public GameObject ControlledCar;
    public GameObject CopyCar;

    public Text StartText;
    public Text EndText;

    bool level_1_started;
    bool level_2_started;

    bool level_1_ended;
    bool level_2_ended;

    bool gameEnded;

    // Start is called before the first frame update
    void Start()
    {
        // Initializing Parameters
        Debug.Log(GameObject.Find("Car"));
        ControlledCar = GameObject.Find("Car");
        ControlledCar.SetActive(false);
        CopyCar = GameObject.Find("CopyCar");
        CopyCar.SetActive(false);
        
        StartText = GameObject.Find("Start").GetComponent<Text>();
        EndText = GameObject.Find("End").GetComponent<Text>();


        level_1_started = false;
        level_2_started = false;

        level_1_ended = false;
        level_2_ended = false;

        gameEnded = false;

        Start_level_1();

    }

    // Update is called once per frame
    void Update()
    {
        if (level_1_ended)
        {
            Start_level_2();
        }
    }

    void Start_level_1()
    {
        if (!level_1_started)
        {
            level_1_started = true;
            ControlledCar.SetActive(true);
        }
    }

    void Start_level_2()
    {
        if (!level_2_started)
        {
            // Start level 2 protocol
            level_2_started = true;
            CopyCar.SetActive(true);
            Level_2_protocol();
        }
        else if(level_2_started && level_2_ended)
        {

        }
    }

    /* *
     * Changes the locations of CopyCar and ControlledCar
     */
    void Level_2_protocol()
    {
        CopyCar.transform.position = new Vector3(StartText.transform.position.x, StartText.transform.position.y, -1);
        CarMovementCopy trial = CopyCar.GetComponent(typeof(CarMovementCopy)) as CarMovementCopy;

        trial.LoadRotations();

        StartText.transform.position = new Vector3(-5, -3, -1);
        EndText.transform.position = new Vector3(7, 2, -1);
        ControlledCar.transform.position = new Vector3(StartText.transform.position.x, StartText.transform.position.y, -1);
        ControlledCar.transform.rotation = new Quaternion(0, 0, 0, 0);
        ControlledCar.SetActive(false);
        ControlledCar.SetActive(true);
    }

    public void SetLevel1Ended(bool state)
    {
        level_1_ended = state;
        Debug.Log("Level-1 State changed to " + state);

    }

    public void SetLevel2Ended(bool state)
    {
        level_2_ended = state;
        Debug.Log("Level-2 State changed to " + state);
    }

    public bool getLevel1Ended()
    {
        return level_1_ended;
    }

    public bool getLevel2Ended()
    {
        return level_2_ended;
    }

    public bool getLevel2Started()
    {
        return level_2_started;
    }

    public bool IsGameEnded()
    {
        if (!level_1_ended && level_2_started == false)
        {
            return false;
        }
        else if(level_1_ended && !level_2_ended)
        {
            return false;
        }

        return true;
    }

    public void SetGameEnd(bool state)
    {
        gameEnded = state;
    }
}