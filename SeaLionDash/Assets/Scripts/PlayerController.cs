using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    // public UnityEngine.Camera cam;
    public Vector3 position;
    public bool isMoving;
    public AudioSource audioSource;
    public GameObject gameManagerObj;
    public float speed;
    public bool isRunning;
    public float maxSpeed;
    public float soundRadius;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        audioSource = gameObject.GetComponent<AudioSource>();
        isMoving = false;
        gameManagerObj = GameObject.Find("GameManager");
        speed = 3.0f;
        maxSpeed = 6.0f;
        soundRadius = 15.0f;
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;
        isMoving = false;
        Move();
        ProcessActions();
    }

    /// <summary>
    /// Move()
    /// moves the player object
    /// Params: None
    /// Returns: None
    /// </summary>
    private void Move()
    {
        // running logic
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            speed = 4.5f;
            maxSpeed = 9.0f;
        }
        else
        {
            isRunning = false;
            speed = 3.0f;
            maxSpeed = 6.0f;
        }


        // mocing forward
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * speed);
            isMoving = true;
        }

        // moving left
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-transform.right * speed);
            isMoving = true;
        }

        // moving backwards
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * speed);
            isMoving = true;
        }


        // moving right
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * speed);
            isMoving = true;
        }

        /*
        // jumping not working atm
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up * 5.0f);
            isMoving = true;
        }
        */

        // clamps velocity
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        // deceleration when not moving
        if (isMoving == false)
        {
            rb.velocity *= 0.95f;
            if(rb.velocity.magnitude <= 0.0001f)
            {
                rb.velocity = Vector3.zero;
            }
        }

    }

    /// <summary>
    /// public void ProcessActions()
    /// Params: None
    /// Reutrns: None
    /// This processes inputs other than movement. Takes care of barking, clapping, and eating
    /// </summary>
    public void ProcessActions()
    {
        // Barking
        if (Input.GetKey(KeyCode.E))
        {
            // here is where I play a sound byte and send info to NPC's inside my range
        }

        // clapping
        if(Input.GetKeyDown(KeyCode.Q))
        {
            // here is where I play a sound byte and send info to NPC's inside my range
        }

        // Eating
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // here is where I pop in a method to eat food and gain points or something
            Eat();
        }
    }

    /// <summary>
    /// public void Eat()
    /// Params: None
    /// Returns: None
    /// This is a helper method to check collisions with food, delte the food, and give the player points. 
    /// </summary>
    public void Eat()
    {

    }
}
