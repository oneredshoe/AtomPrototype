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

    public KeeperState state;
    public SensePlayer senser;

    // Start is called before the first frame update
    void Start()
    {
        senser = gameObject.GetComponent<SensePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {

        }   
    }
}
