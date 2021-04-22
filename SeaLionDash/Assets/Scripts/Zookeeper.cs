using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zookeeper : Vehicle
{
    public enum KeeperState
    {
        Wander,
        Chase,
        Flee
    }

    [Header("Keeper Stats")]
    public KeeperState state;

    [SerializeField]
    [Range(3f, 10f)]
    private float chaseDuration;
    private float chaseTimer;
    [SerializeField]
    [Range(3f, 10f)]
    private float fleeDuration;
    private float fleeTimer;

    private SensePlayer senser;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        senser = gameObject.GetComponent<SensePlayer>();
        chaseTimer = chaseDuration;
        fleeTimer = fleeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case KeeperState.Chase:

                //seek player
                SeekTarget(senser.player);

                //refresh timer if player is in range
                if (senser.PlayerInFOV() && senser.PlayerInRadius())
                {
                    chaseTimer = chaseDuration;
                }

                //update timer, if timer ends, enter wander state
                if (chaseTimer > 0)
                {
                    chaseTimer -= Time.deltaTime;
                }
                else
                {
                    chaseTimer = chaseDuration;
                    state = KeeperState.Wander;
                }

                break;
            case KeeperState.Flee:

                //flee player
                FleeTarget(senser.player);

                //update timer, if timer ends, enter wander state
                if(fleeTimer > 0)
                {
                    fleeTimer -= Time.deltaTime;
                }
                else
                {
                    fleeTimer = fleeDuration;
                    state = KeeperState.Wander;
                }

                break;
            case KeeperState.Wander:
                
                //wander
                Wandering();

                //if player in sight, enter chase state
                if (senser.PlayerInFOV() && senser.PlayerInRadius())
                {
                    state = KeeperState.Chase;
                }

                break;
            default:
                break;
        }   
    }
}
