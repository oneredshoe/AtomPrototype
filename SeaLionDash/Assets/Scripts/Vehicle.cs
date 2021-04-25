using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class Vehicle : MonoBehaviour
{
    public float speed;

    protected Vector3 vel;
    protected Vector3 acc;
    protected Vector3 pos;
    protected Vector3 dir;

    [SerializeField]
    [Range(0, 2)]
    protected float friction;

    [SerializeField]
    [Range(0.1f, 0.01f)]
    protected float minVel;

    public LayerMask blockMask;

    [Header("Obstacle Avoidance")]
    public LayerMask obstacleMask;
    [SerializeField]
    [Range(0.1f, 5)]
    private float obsDetecDistance;
    [SerializeField]
    [Range(0.1f, 2)]
    private float obsDetecWidth;
    [SerializeField]
    [Range(1f, 10f)]
    private float obsAvoidForce;

    [Header("Wall Follow")]
    public LayerMask wallMask;
    [SerializeField]
    [Range(0.1f, 5)]
    private float wallDetecDistance;
    [SerializeField]
    [Range(0.1f, 2)]
    private float wallDetecWidth;
    [SerializeField]
    [Range(1f, 10f)]
    private float wallFollowForce;

    [Header("Wandering")]
    [SerializeField]
    [Range(1f, 10f)]
    private float wanderDistance;
    [SerializeField]
    [Range(1f, 10f)]
    private float wanderRadius;
    [SerializeField]
    [Range(1f, 10f)]
    private float wanderChangeTime;
    [SerializeField]
    [Range(1f, 3f)]
    private float wanderTimeOffset;
    private float wanderTimer;
    private float wanderAngle;

    [SerializeField]
    [Range(0.1f, 10f)]
    private float angleChangeTime;
    [SerializeField]
    [Range(0.1f, 10f)]
    private float angleSize;
    private float angleTimer;
    private bool wanderClockwise;

    private CharacterController controller;
    private float height;

    // Start is called before the first frame update
    protected void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        height = transform.position.y;
        wanderTimer = wanderChangeTime;
        wanderAngle = Random.Range(0, 360);
        wanderChangeTime += Random.Range(-wanderTimeOffset, wanderTimeOffset);
    }

    // Update is called once per frame
    protected void Update()
    {

    }

    //Teleport this object to pos
    void MoveTo(Vector3 pos)
    {
        transform.position = pos;
    }

    //Seek pos/obj
    protected Vector3 Seek(Vector3 target)
    {
        Vector3 targetVel = target - transform.position;

        targetVel = targetVel.normalized * speed;

        return targetVel - vel;
    }
    protected Vector3 Seek(GameObject target)
    {
        return Seek(target.transform.position);
    }

    //Flee pos/obj
    protected Vector3 Flee(Vector3 target)
    {
        Vector3 targetVel = transform.position - target;

        targetVel = targetVel.normalized * speed;

        return targetVel - vel;
    }
    protected Vector3 Flee(GameObject target)
    {
        return Flee(target.transform.position);
    }

    protected void Pursuit(Transform target)
    {

    }

    protected void ApplyFriction(float coeff)
    {
        Vector3 friction = -vel;
        friction = friction * coeff;
        acc += friction;
    }

    //Move based on acc
    virtual protected void Move()
    {
        acc = new Vector3(acc.x, 0, acc.z);

        if (vel.magnitude > minVel)
            ApplyFriction(friction);

        vel += acc * Time.deltaTime;


        dir = vel.normalized;

        transform.rotation = Quaternion.Euler(0, Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg, 0);

        vel = Vector3.ClampMagnitude(vel, speed);


        Debug.DrawLine(transform.position, transform.position + acc, Color.green);

        Debug.DrawLine(transform.position, transform.position + vel, Color.blue);

        vel.y = -2f;

        controller.Move(vel * Time.deltaTime);

        acc = Vector3.zero;

        pos = transform.position;
    }

    virtual protected void SeekTarget(GameObject target)
    {
        acc += ObstacleAvoidance();

        acc += Seek(target) * 2f;

        Move();
    }

    virtual protected void FleeTarget(GameObject target)
    {
        acc += ObstacleAvoidance();

        acc += Flee(target) * 2f;

        Move();
    }

    virtual protected void Wandering()
    {
        acc += ObstacleAvoidance();

        acc += Wander();

        Move();
    }

    /// <summary>
    /// Dectect obstacles in the front and calculate avoiding force if nessesary
    /// </summary>
    /// <returns></returns>
    /// 

    //Obs Avoidance
    protected Vector3 ObstacleAvoidance()
    {
        Vector3 avoidForce = Vector3.zero;
        Collider[] hits = Physics.OverlapBox(transform.position + transform.forward * obsDetecDistance / 2, new Vector3(obsDetecWidth / 2, .5f, obsDetecDistance / 2), gameObject.transform.rotation, obstacleMask);

        if (hits.Length == 0)
        {
            return Vector3.zero;
        }
        else
        {
            foreach (Collider c in hits)
            {
                //Calculate dot product
                float dot = Vector3.Dot(c.gameObject.transform.position - transform.position, transform.right);


                Vector3 right = transform.right + transform.forward;
                Vector3 left = -transform.right + transform.forward;

                right.Normalize();
                left.Normalize();

                //If on right side
                if (dot > 0)
                {
                    avoidForce += (-transform.right * (obsAvoidForce / Vector3.Distance(transform.position, c.gameObject.transform.position))) * vel.magnitude / 2;
                }

                if (dot < 0)
                {
                    avoidForce += (transform.right * (obsAvoidForce / Vector3.Distance(transform.position, c.gameObject.transform.position))) * vel.magnitude / 2;
                }
            }
        }

        return avoidForce + vel;
    }

    //Wall Follow
    protected Vector3 WallFollow()
    {
        Vector3 followForce = Vector3.zero;

        RaycastHit hit;

        //Right Wing
        if (Physics.Raycast(transform.position + transform.right * wallDetecWidth / 2, transform.forward, out hit, wallDetecDistance, wallMask))
        {
            //followForce += (hit.normal * (wallFollowForce / Vector3.Distance(transform.position, hit.point))) * vel.magnitude;
            //followForce += Vector3.Cross(Vector3.up, hit.normal);

            Vector3 dir = Vector3.Cross(Vector3.up, hit.normal);
            followForce = dir * wallFollowForce - acc;
        }

        //Left Wing
        if (Physics.Raycast(transform.position - transform.right * wallDetecWidth / 2, transform.forward, out hit, wallDetecDistance, wallMask))
        {
            //followForce += (hit.normal * (wallFollowForce / Vector3.Distance(transform.position, hit.point))) * vel.magnitude;
            //followForce += Vector3.Cross(Vector3.down, hit.normal);

            Vector3 dir = Vector3.Cross(Vector3.down, hit.normal);
            followForce = dir * wallFollowForce - acc;
        }



        return followForce;
    }

    //Wandering
    protected Vector3 Wander()
    {
        Vector3 center = transform.position + dir * wanderDistance;

        //Change angle randomly every few seconds
        if (wanderTimer > 0f)
        {
            wanderTimer -= Time.deltaTime;
        }
        else
        {
            wanderAngle = Random.Range(0, 360);
            wanderTimer = wanderChangeTime;
            wanderClockwise = !wanderClockwise;
        }

        //Update angle
        if (angleTimer > 0f)
        {
            angleTimer -= Time.deltaTime;
        }
        else
        {
            angleTimer = angleChangeTime;
            if (wanderClockwise)
            {
                wanderAngle += angleSize;
            }
            else
            {
                wanderAngle -= angleSize;
            }
        }

        //Calculate target location
        float x = center.x + Mathf.Cos(wanderAngle) * wanderRadius;
        float y = transform.position.y;
        float z = center.z + Mathf.Sin(wanderAngle) * wanderRadius;

        Vector3 loc = new Vector3(x, y, z);
        Debug.DrawLine(transform.position, loc, Color.cyan);

        return Seek(loc);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + transform.right * obsDetecWidth / 2, transform.position + transform.forward * obsDetecDistance + transform.right * obsDetecWidth / 2);
        Gizmos.DrawLine(transform.position - transform.right * obsDetecWidth / 2, transform.position + transform.forward * obsDetecDistance - transform.right * obsDetecWidth / 2);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + transform.right * wallDetecWidth / 2, transform.position + transform.forward * wallDetecDistance + transform.right * wallDetecWidth / 2);
        Gizmos.DrawLine(transform.position - transform.right * wallDetecWidth / 2, transform.position + transform.forward * wallDetecDistance - transform.right * wallDetecWidth / 2);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * wanderDistance);

    }
}
