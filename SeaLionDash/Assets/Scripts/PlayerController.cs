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
    public Vector3 startPos;
    public const float CD_TIME = 1.0f;
    public float cdTimer;
    public bool canAction;


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
        startPos = Vector3.zero;
        cdTimer = 0.0f;
        canAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;
        isMoving = false;
        Move();

        // if the timer has reset itself
        if(canAction == true)
        {
            ProcessActions();
        }
        else
        {
            cdTimer += Time.deltaTime;
        }

        if(cdTimer >= CD_TIME)
        {
            cdTimer = 0.0f;
            canAction = true;
        }
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
            // check the sphere with range, whoever is in that spehere acts accoringly, also play sound
            Collider[] closeObjects = Physics.OverlapSphere(transform.position, soundRadius, 0);

            // loops trhough the objects and changes their state
            for(int i = 0; i < closeObjects.Length; i++)
            {
                // if get component zookeeper, then change state
                if(closeObjects[i].GetComponent<Zookeeper>() != null)
                {
                    // runs away in fear
                    closeObjects[i].GetComponent<Zookeeper>().state = Zookeeper.KeeperState.Flee;
                }

                // if get componnet guest then change state
                if (closeObjects[i].GetComponent<Guest>() != null)
                {
                    // throws food for the player to eat
                    closeObjects[i].GetComponent<Guest>().GiveFood();
                }
            }

            //play sound byte
            //audioSource.clip = barkClip;
            //audioSource.Play();

            // set canAction to false so the CD starts
            canAction = false;

            Debug.Log("Barked");
        }

        // clapping
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Collider[] closeObjects = Physics.OverlapSphere(transform.position, soundRadius, 0);

            // for guest change state to attracted
            // loops trhough the objects and changes their state
            for (int i = 0; i < closeObjects.Length; i++)
            {
                // if get component zookeeper, then change state
                if (closeObjects[i].GetComponent<Zookeeper>() != null)
                {
                    // chases the player
                    closeObjects[i].GetComponent<Zookeeper>().state = Zookeeper.KeeperState.Chase;
                }

                // if get componnet guest then change state
                if (closeObjects[i].GetComponent<Guest>() != null)
                {
                    // attracted to player
                    closeObjects[i].GetComponent<Guest>().state = Guest.GuestState.Attracted;
                }
            }

            // play sound byte
            //audioSource.clip = clapClip;
            //audioSource.Play();

            // set canAction to false so the CD starts
            canAction = false;

            Debug.Log("Clapped");
        }

        // Eating
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // here is where I pop in a method to eat food and gain points or something
            Eat();

            // set canAction to false so the CD starts
            canAction = false;

            Debug.Log("Ate");
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
        // might need to add button check in OnCollisionEnter()
        // also in onCollisionenter, teleport player to their cage
    }
}
