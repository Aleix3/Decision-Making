using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingAgent : MonoBehaviour
{
    public NavMeshAgent agent;               
    public Transform[] waypoints;            
    public GameObject ghostAgent;            
    public float followDistance = 10f;       
    public float patrolSpeed = 3.5f;         
    public float followSpeed = 5f;           

    private int currentWaypointIndex;        
    private bool isForward;                  
    private bool isFollowingGhost = false;   

    void Start()
    {
        
        if (waypoints.Length < 2)
        {
            Debug.LogError("Se necesitan al menos 2 waypoints para patrullar.");
            return;
        }

        agent = GetComponent<NavMeshAgent>();

        
        currentWaypointIndex = Random.Range(0, waypoints.Length);
        isForward = Random.value > 0.5f;  // Randomly decide to go forward or backward

        
        agent.speed = patrolSpeed;

        // Move to the initial waypoint
        agent.SetDestination(waypoints[currentWaypointIndex].position);

        // Disable the ghost's MeshRenderer (make it invisible)
        ghostAgent.GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        // Calculate distance to the ghost
        float distanceToGhost = Vector3.Distance(transform.position, ghostAgent.transform.position);

        
        if (distanceToGhost < followDistance)
        {
            isFollowingGhost = true;
            agent.speed = followSpeed;  // Increase speed when following the ghost
            agent.SetDestination(ghostAgent.transform.position);
        }
        else
        {
            // If the ghost is too far, stop following and go back to patrolling
            isFollowingGhost = false;
            agent.speed = patrolSpeed;

            // If the agent has reached its current waypoint, move to the next one
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                UpdateWaypoint();
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
    }

    
    void UpdateWaypoint()
    {
        if (isForward)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                
                currentWaypointIndex = waypoints.Length - 1;
                isForward = false;
            }
        }
        else
        {
            currentWaypointIndex--;
            if (currentWaypointIndex < 0)
            {
               
                currentWaypointIndex = 0;
                isForward = true;
            }
        }
    }
}