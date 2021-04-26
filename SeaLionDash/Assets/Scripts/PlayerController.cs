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
    public int fishEaten;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        audioSource = gameObject.GetComponent<AudioSource>();
        isMoving = false;
        gameManagerObj = GameObject.Find("GameManager");
        speed = 5.0f;
        maxSpeed = 7.5f;
        soundRadius = 15.0f;
        startPos = Vector3.zero;
        cdTimer = 0.0f;
        canAction = true;
        fishEaten = 0;
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
            speed = 6.5f;
            maxSpeed = 10.0f;
        }
        else
        {
            isRunning = false;
            speed = 5.0f;
            maxSpeed = 7.5f;
        }


        // mocing forward
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * speed);
            isMoving = true;
        }

        // rotating counter-clockwise
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0.0f, -0.25f, 0.0f));
        }

        // moving backwards
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * speed);
            isMoving = true;
        }

        // rotating clockwise
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0.0f, 0.25f, 0.0f));
        }

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
            // need to use LayerMask.GetMask to get the proper int
            int LM = LayerMask.GetMask("Npc");
            Collider[] closeObjects = Physics.OverlapSphere(transform.position, soundRadius, LM);

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

            // Debug.Log("Barked");
        }

        // clapping
        if(Input.GetKeyDown(KeyCode.Q))
        {
            // need to use LayerMask.GetMask to get the proper int
            int LM = LayerMask.GetMask("Npc");
            Collider[] closeObjects = Physics.OverlapSphere(transform.position, soundRadius, LM);

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

            // Debug.Log("Clapped");
        }
    }

    /// <summary>
    /// public void Eat()
    /// Params: None
    /// Returns: None
    /// This is a helper method to check collisions with food, delte the food, and give the player points. 
    /// </summary>
    public void Eat(GameObject food)
    {
        // might need to add button check in OnCollisionEnter()

        canAction = false;
        Destroy(food);
        fishEaten++;
    }

    // does all collision detection between player and zookeeper
    // also checks key press for eating
    public void OnCollisionEnter(Collision collision)
    {
        // if colliding with the zookeeper, just sends you back to the start location
        if(collision.collider.tag == "Zookeeper")
        {
            transform.position = startPos;
        }

        // if the eat key is pressed and it's collidiing with food, call Eat()
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(collision.collider.tag == "Food")
            {
                Eat(collision.gameObject);
            }
        }
    }
}
