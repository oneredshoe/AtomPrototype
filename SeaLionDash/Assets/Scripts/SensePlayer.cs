using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class SensePlayer : MonoBehaviour
{
    public GameObject player;
    [SerializeField]
    [Range(1f,90f)]
    public float FOV;
    [SerializeField]
    [Range(1f, 20f)]
    public float Radius;

    void Start()
    {
        // sets player via code
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //SensePlayer is a modual used to check if player is in a specified range of a object

    public bool PlayerInFOV()
    {
        return Vector3.Dot(transform.forward, (player.transform.position - transform.position).normalized) > Mathf.Cos(FOV * Mathf.Deg2Rad);
    }

    public bool PlayerInRadius()
    {
        return Vector3.Distance(player.transform.position, transform.position) < Radius;
    }


    //Code from Youtube channle Sebastian Lague https://www.youtube.com/watch?v=rQG9aUWarwE&t=776s
    private Vector3 DirFromAngle(float angleInDegrees, bool anlgeIsGlobal)
    {
        if (!anlgeIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Radius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + DirFromAngle(FOV / 2, false) * Radius);
        Gizmos.DrawLine(transform.position, transform.position + DirFromAngle(-FOV / 2, false) * Radius);
    }
}
