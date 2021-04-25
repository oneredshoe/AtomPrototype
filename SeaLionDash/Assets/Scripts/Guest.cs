using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guest : Vehicle
{
    public enum GuestState
    {
        Idle,
        Wander,
        Attracted
    }

    [Header("Guest Stats")]
    public GuestState state;

    [SerializeField]
    [Range(3f, 10f)]
    private float attractDuration;
    private float attractTimer;

    private SensePlayer senser;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        senser = gameObject.GetComponent<SensePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        //for debug
        if (Input.GetKeyDown(KeyCode.K))
        {
            state = GuestState.Attracted;
        }

        switch (state)
        {
            case GuestState.Idle:

                //make sure to face player
                FacePlayer();

                //update attraction timer
                UpdateAttaction();

                break;
            case GuestState.Wander:

                //wander
                Wandering();

                break;
            case GuestState.Attracted:

                //seek player
                SeekTarget(senser.player);

                //if player in range, go to idle stage
                if (senser.PlayerInRadius())
                {
                    state = GuestState.Idle;
                }

                //update attraction timer
                UpdateAttaction();

                break;
            default:
                break;
        }
    }

    //Force object face player
    private void FacePlayer()
    {
        if (Vector3.Dot(transform.forward, (senser.player.transform.position - transform.position).normalized) < 0.98f)
        {
            acc += Seek(senser.player);
        }
        Move();
    }

    //Update attraction timer
    void UpdateAttaction()
    {
        attractTimer -= Time.deltaTime;
        if (attractTimer <= 0)
        {
            state = GuestState.Wander;
            attractTimer = attractDuration;
        }
    }

    //Spawn food
    public void GiveFood()
    {
        if (state != GuestState.Wander)
        {
            //do things
        }
    }
}
