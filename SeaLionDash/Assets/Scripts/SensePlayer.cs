using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensePlayer : MonoBehaviour
{
    public GameObject player;
    [SerializeField]
    [Range(1f,90f)]
    public float FOV;
    public float Radius;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool PlayerInFOV()
    {
        return Vector3.Dot(transform.forward, (player.transform.position - transform.position).normalized) > Mathf.Cos(FOV);
    }

    public bool PlayerInRadius()
    {
        return Vector3.Distance(player.transform.position, transform.position) < Radius;
    }
}
