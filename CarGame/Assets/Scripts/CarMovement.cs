using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;


/* *
 * This is Car Movement class. There are 2 classes of moving classes, Car Movement and Car Movement Copy.
 *
 * Car movement takes input from buttons and changes the movement of controlled car, which is yellow one!
 *
 * 
*/

public class CarMovement : MonoBehaviour
{

    // Properties of Car 
    public float MaxSpeed;
    public float acc;
    public float steering;
    float X;
    float Y = 1;

    Rigidbody2D rb;

    // Property of GameRuler, which checks if everything is going on according to game rules
    GameRuler GameRuler;

    // Rotations list records the rotations of car, for CopyCar
    private List<float> rotations = new List<float>();

    // StartText decides the start location of ControlledCar
    private Text startText;

    // Buttons for rotation
    private Button leftBtn;
    private Button rightBtn;


    private void Start()
    {
        // Initializations of parameters
        X = 0;
        startText = GameObject.Find("Start").GetComponent<Text>();

        rb = GetComponent<Rigidbody2D>();
        rb.transform.position = new Vector3(startText.transform.position.x, startText.transform.position.y, -1);

        GameRuler = GameObject.Find("GameRuler").GetComponent<GameRuler>();

        leftBtn = GameObject.FindGameObjectWithTag("LeftBtn").GetComponent<Button>();
        rightBtn = GameObject.FindGameObjectWithTag("RightBtn").GetComponent<Button>();

        // Adding listener to buttons. Normalize function makes X=0, to stabilize the rotation of ControlledCar
        leftBtn.onClick.AddListener( () => {
            X = -1;
            Invoke("Normalize", 0.08f);
        } );

        rightBtn.onClick.AddListener(() => {
            X = 1;
            Invoke("Normalize", 0.08f);
        });


    }


    private void Update()
    {
        // As long as game ends, movement continuous

        if (!GameRuler.IsGameEnded()) 
        {

            move();
            
        }
    }

    public static void SaveRotations<T>(string fileName, List<T> list)
    {
        // Saving the rotation as "save.dat" file
        try
        {
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, list);
                Debug.Log("Way saved");
            }

        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    // When car hits any object, this function triggers
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        Debug.Log(collision.tag);
        rb.velocity = rb.velocity.normalized * 0;

        // Leveling check for EndText. If everything is okey, it saves rotations
        if(collision.tag == "End")
        {
            Debug.Log("Inside end:" + GameRuler.getLevel1Ended());

            if (!GameRuler.getLevel1Ended())
            {
                SaveRotations("save.dat", rotations);

                WeirdDelay(5000); // Invoke does not stops to stop process, like changing other levels, I decided to use for loop as delay
               
                GameRuler.SetLevel1Ended(true);
                Debug.Log("Way saved");

            }
            else if(!GameRuler.getLevel2Ended())
            {
                GameRuler.SetLevel2Ended(true);
            }
        }
        else if(collision.tag == "Obstacle" || collision.tag == "Wall")
        {
            GameRuler.SetLevel1Ended(true);
            GameRuler.SetLevel2Ended(true);
        }

    }

    // Making virtual delay using for loop
    void WeirdDelay(int limit)
    {
        for (int i = 0; i < limit; i++)
        {
            //Debug.Log("Weird Delay: " + i);
        }
    }

    // Makes X=0
    void Normalize()
    {
        X = 0;
    }

    // Moving method I've found from online tutorials
    void move()
    {
        //X = Input.GetAxis("Horizontal");
        Vector2 speed = transform.up * (Y * acc);
        rb.AddForce(speed);

        float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));

        rotations.Add(X);

        if (acc > 0)
        {
            if (direction > 0)
            {
                rb.rotation -= X * steering * (rb.velocity.magnitude / MaxSpeed);
            }
            else
            {
                rb.rotation += X * steering * (rb.velocity.magnitude / MaxSpeed);
            }
        }

        if (rb.velocity.magnitude > MaxSpeed)
        {
            rb.velocity = rb.velocity.normalized * MaxSpeed;
        }
    }
}
