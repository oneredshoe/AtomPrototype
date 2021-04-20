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
    [Range(5f, 20f)]
    private float attractDuration;
    [SerializeField]
    [Range(3f, 20f)]
    private float watchDistance;
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
        if (Input.GetKeyDown(KeyCode.K))
        {
            state = GuestState.Attracted;
        }
        switch (state)
        {
            case GuestState.Idle:
                FacePlayer();
                Move();
                UpdateAttaction();
                break;
            case GuestState.Wander:
                Wandering();
                break;
            case GuestState.Attracted:
                SeekTarget(player);
                if(Vector3.Distance(transform.position, player.transform.position) < watchDistance)
                {
                    state = GuestState.Idle;
                }
                UpdateAttaction();
                break;
        }
    }
    private void StayNearPlayer()
    {
        acc += Seek(player.transform.position + (player.transform.position - transform.position).normalized * watchDistance);

        Move();
    }

    private void FacePlayer()
    {
        if(Vector3.Dot(transform.forward, (player.transform.position - transform.position).normalized) < 0.95f)
        {
            acc += Seek(player);
        }
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
    void GiveFood()
    {
        if (state != GuestState.Wander)
        {
            //do things
        }
    }

    private void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, watchDistance);
    }
}
