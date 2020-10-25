using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class CarMovementCopy : MonoBehaviour
{
    // Properties of Car
    public float MaxSpeed;
    public float acc;
    public float steering;
    float X;
    float Y = 1;

    Rigidbody2D rb;

    private List<float> rotations;

    int index = 0;

    // bool isGameEnded = false;

    // Property of GameRuler, which checks if everything is going on according to game rules
    GameRuler GameRuler;

    private void Start()
    {
        // Initializations of parameters
        rb = GetComponent<Rigidbody2D>();
        GameRuler = GameObject.Find("GameRuler").GetComponent<GameRuler>();
    }

    private void Update()
    {
        // As long as game ends, movement continuous
        if (!GameRuler.IsGameEnded()) // if (!isGameEnded)
        {
            move();
        }
    }

    public static List<T> LoadRotations<T>(string fileName)
    {
        // Loading the rotation as "save.dat" file
        var list = new List<T>();
        if (File.Exists(fileName))
        {
            Debug.Log("It's exist!!!");

            try
            {
                using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    var formatter = new BinaryFormatter();
                    list = (List<T>)
                        formatter.Deserialize(stream);
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }

        }
        else
        {
            Debug.Log("It's not exist!");
        }
        return list;
    }

    // When car hits any object, this function triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        rb.velocity = rb.velocity.normalized * 0;
        // isGameEnded = true;
    }

    /* *
     * When the game starts, CopyCar initialized to
     * previous driving, therefore when level 2 starts,
     * this function called first, so that it uses latest save!
     */
    public void LoadRotations()
    {
        rotations = LoadRotations<float>("save.dat");
    }

    // Moving method I've found from online tutorials
    // Rotations are taken from rotations list, checking by index!
    void move()
    {
        if (index < rotations.Count)
        {
            X = rotations[index]; //Input.GetAxis("Horizontal");
            index++;

            Vector2 speed = transform.up * (Y * acc);
            rb.AddForce(speed);

            float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));

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
        else
        {

        }
    }
}
